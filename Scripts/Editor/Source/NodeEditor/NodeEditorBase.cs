using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using FK.Utility.ArraysAndLists;

namespace FK.Editor.NodeEditor
{
    /// <summary>
    /// <para>This is the Base for a node based Editor with multiple types of nodes and variable numbers of connections per node.</para>
    /// <para>To create your own Node Based Editor, create an Editor class that inherits from this one. Make sure to call OnEnable and OnGui of this script if you overwrite them.</para>
    /// 
    /// <para>This was created using this Tutorial as a base: http://gram.gs/gramlog/creating-node-based-editor-unity/ </para>
    /// 
    /// v2.2 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TNodeData"></typeparam>
    public class NodeEditorBase<TNode, TNodeData> : EditorWindow where TNode : Node<TNode, TNodeData>, new() where TNodeData : NodeDataBase, new()
    {
        // ######################## DELEGATES ######################## //
        public delegate void DelOnConnectionMade(Connection<TNode, TNodeData> connection);
        public delegate void DelOnConnectionRemoved(Connection<TNode, TNodeData> connection);

        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// All connections between nodes
        /// </summary>
        public List<Connection<TNode, TNodeData>> Connections;

        /// <summary>
        /// This Delegate is invoked whenever a connection is made
        /// </summary>
        public DelOnConnectionMade OnConnectionMade;
        /// <summary>
        /// This Delegate is invoked whenever a connection is removed 
        /// </summary>
        public DelOnConnectionRemoved OnConnectionRemoved;

        /// <summary>
        /// Returns the next ID that can be used
        /// </summary>
        public int NextNodeID
        {
            get
            {
                int nextID = ++_data.LastNodeId;
                SetDataDirty();
                return nextID;
            }
        }

        // ######################## PROTECTED VARS ######################## //
        protected const int DEFAULT_NODE_WIDTH = 200;
        protected const int DEFAULT_NODE_HEIGHT = 50;

        /// <summary>
        /// All nodes
        /// </summary>
        protected List<TNode> Nodes;

        /// <summary>
        /// Offset for drawing the background grid
        /// </summary>
        protected Vector2 Offset;

        /// <summary>
        /// The Data we are currently editing
        /// </summary>
        protected NodeEditableObject<TNodeData> _data;

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// Context Menu entries with display name and function to execute
        /// </summary>
        private Dictionary<string, GenericMenu.MenuFunction2> _contextMenuEntries = new Dictionary<string, GenericMenu.MenuFunction2>();

        /// <summary>
        /// Currently selected In Point
        /// </summary>
        private ConnectionPoint<TNode, TNodeData> _selectedInPoint;
        /// <summary>
        /// Currently Selected Out Point
        /// </summary>
        private ConnectionPoint<TNode, TNodeData> _selectedOutPoint;

        /// <summary>
        /// Drag delta for drawing the background grid
        /// </summary>
        private Vector2 _drag;

        /// <summary>
        /// GUI Style for In Points
        /// </summary>
        private GUIStyle _inPointStyle;
        /// <summary>
        /// GUI Style for Out Points
        /// </summary>
        private GUIStyle _outPointStyle;
        /// <summary>
        /// GUI Style for Nodes that are not selected
        /// </summary>
        private GUIStyle _nodeStyle;
        /// <summary>
        /// GUI Style for selected nodes
        /// </summary>
        private GUIStyle _selectedNodeStyle;

        /// <summary>
        /// Background Color as 1x1 Texture2D
        /// </summary>
        private Texture2D Background;



        // ######################## UNITY START & UPDATE ######################## //
        protected virtual void OnEnable()
        {
            //create GUI Styles
            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0.png") as Texture2D;
            _nodeStyle.border = new RectOffset(12, 12, 12, 12);

            _selectedNodeStyle = new GUIStyle();
            _selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0 on.png") as Texture2D;
            _selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

            _inPointStyle = new GUIStyle();
            _inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            _inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            _inPointStyle.border = new RectOffset(4, 4, 12, 12);

            _outPointStyle = new GUIStyle();
            _outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            _outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            _outPointStyle.border = new RectOffset(4, 4, 12, 12);

            CreateBackground();
        }

        protected virtual void OnLostFocus()
        {
            AssetDatabase.SaveAssets();
        }

        protected virtual void OnGUI()
        {
            // draw the grid in the background
            if (Background == null)
                CreateBackground();

            GUI.DrawTexture(new Rect(0, 0, position.width, position.height), Background, ScaleMode.StretchToFill);
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            // Do nothing of the following if we have no data
            if (_data == null)
                return;

            // draw connections
            DrawConnections();
            DrawConnectionLine(Event.current);

            // draw nodes
            DrawNodes();

            // provess all events
            ProcessEvents(Event.current);

            // save the offset
            if(_data.EditorOffset != Offset)
            {
                _data.EditorOffset = Offset;
                SetDataDirty();
            }

            // repaint if anything changed
            if (GUI.changed)
                Repaint();
        }

        // ######################## INITS ######################## //
        /// <summary>
        /// Initializes the Editor with the provided Data
        /// </summary>
        /// <param name="data"></param>
        protected void Init(NodeEditableObject<TNodeData> data)
        {
            // clear anything that might have been in the window
            Clear();

            // save a reference to the data
            _data = data;

            // set offset from Data
            Offset = data.EditorOffset;

            // if there are no nodes in the data, let the programmer decide what to do, else load the data
            if (_data.Nodes == null || _data.Nodes.Count == 0)
            {
                InitEmpty();
                SetDataDirty();
            }
            else
            {
                LoadData();
            }

            // clear any selection left over from automatic connection creation
            ClearConnectionSelection();
        }

        /// <summary>
        /// Is called when empty data is loaded
        /// </summary>
        protected virtual void InitEmpty() { }

        /// <summary>
        /// Clears the viewport of the editor by removing all nodes, connections and resetting the offset
        /// </summary>
        protected void Clear()
        {
            Nodes = new List<TNode>();
            ClearConnectionSelection();
            Connections = new List<Connection<TNode, TNodeData>>();
            Offset = Vector2.zero;
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Loads all Nodes an Connections from the data
        /// </summary>
        private void LoadData()
        {
            // Load Nodes first
            foreach (NodeDataBase node in _data.Nodes)
            {
                // add the node
                TNode n = new TNode();
                n.SetUp(this, node.ID, Vector2.zero, node.Width, node.PreferredHeight, node.Inputs.Length, node.Outputs.Length);
                AddNode(node.PositionInEditor, n);
            }

            // now load the connections. To do this, we go through all nodes and look at the Inputs of them. 
            // Each Node that is referenced in an Input must have the node we are looking at as an output, we check that and create the connection between the correct points
            foreach (TNode node in Nodes)
            {
                // look at all Inputs of this node
                for (int i = 0; i < node.Data.Inputs.Length; ++i)
                {
                    // if there are no nodes connected to this input, look ath th next one
                    if (node.Data.Inputs[i].Connections == null || node.Data.Inputs[i].Connections.Count == 0)
                        continue;

                    // go through each ID at this Input
                    foreach (NodeConnectionData connection in node.Data.Inputs[i].Connections)
                    {
                        // now look ar all other nodes. If any node matches the ID that is saved at the Current input, these two should have a connection
                        foreach (TNode otherNode in Nodes)
                        {
                            if (otherNode.ID == connection.NodeID)
                            {
                                // go through all outputs of the other node
                                for (int j = 0; j < otherNode.Data.Outputs.Length; ++j)
                                {
                                    // if there are no nodes connected to this output, look at th next one
                                    if (otherNode.Data.Outputs[j].Connections == null || otherNode.Data.Outputs[j].Connections.Count == 0)
                                        continue;

                                    // of this output contains the node we are currently checking, make the connection
                                    if (otherNode.Data.Outputs[j].Contains(node.ID, i))
                                    {
                                        CreateConnection(node.InPoints[i], otherNode.OutPoints[j]);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws all nodes from oldest to newest
        /// </summary>
        private void DrawNodes()
        {
            // Do nothing if there are no nodes
            if (Nodes == null)
                return;

            foreach (TNode node in Nodes)
            {
                node.Draw();
            }
        }

        /// <summary>
        /// Draws Connections from newest to oldest so it can cope with deleting connections
        /// </summary>
        private void DrawConnections()
        {
            if (Connections == null)
                return;

            for (int i = Connections.Count - 1; i >= 0; --i)
            {
                Connections[i].Draw();
            }
        }

        /// <summary>
        /// Draws a Connetion between the selected Connection Point and the Mouse
        /// </summary>
        /// <param name="e"></param>
        private void DrawConnectionLine(Event e)
        {
            if (_selectedInPoint != null && _selectedOutPoint == null)
            {
                DrawBezier(_selectedInPoint.PointRect.center, e.mousePosition, true);
            }

            if (_selectedOutPoint != null && _selectedInPoint == null)
            {
                DrawBezier(_selectedOutPoint.PointRect.center, e.mousePosition, false);
            }
        }

        /// <summary>
        /// Passes an event on to Nodes and processes it
        /// </summary>
        /// <param name="e"></param>
        private void ProcessEvents(Event e)
        {
            // pass event to nodes
            ProcessNodeEvents(e);

            // reset drag
            _drag = Vector2.zero;

            // process event
            switch (e.type)
            {
                case EventType.MouseDown:
                    // if the user clicked the right mouse button, show Context Menu
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }

                    // if the user clicked the left mouse button, clear connection selection
                    if (e.button == 0)
                    {
                        ClearConnectionSelection();
                    }
                    break;

                case EventType.MouseDrag:
                    // if the user is dragging the left mouse button, drag canvas
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }
                    break;
            }
        }

        /// <summary>
        /// Makes all nodes process the event
        /// </summary>
        /// <param name="e"></param>
        private void ProcessNodeEvents(Event e)
        {
            // if there are no nodes, do nothing
            if (Nodes == null)
                return;

            // pass the event to all nodes
            for (int i = Nodes.Count - 1; i >= 0; --i)
            {
                GUI.changed = Nodes[i].ProcessEvents(e) ? true : GUI.changed;
            }
        }

        /// <summary>
        /// Displays and mamnages a Context Menu
        /// </summary>
        /// <param name="mousePosition"></param>
        private void ProcessContextMenu(Vector2 mousePosition)
        {
            // create the menu
            GenericMenu genericMenu = new GenericMenu();

            // fill in entries
            foreach (KeyValuePair<string, GenericMenu.MenuFunction2> pair in _contextMenuEntries)
            {
                genericMenu.AddItem(new GUIContent(pair.Key), false, pair.Value, mousePosition);
            }

            // show menu
            genericMenu.ShowAsContext();
        }

        /// <summary>
        /// Adds a new node from the provided template at the given position
        /// </summary>
        /// <param name="mousePosition">Position to create the new node at</param>
        /// <param name="template">Node Template to use</param>
        protected TNode AddNode(Vector2 mousePosition, TNode template)
        {
            // create nodes list if necessary
            if (Nodes == null)
                Nodes = new List<TNode>();

            // create a new node from the template
            TNode node = template.Clone() as TNode;

            // initialize the node and add it to the list
            node.Init(mousePosition, _nodeStyle, _selectedNodeStyle, _inPointStyle, _outPointStyle, OnClickInPoint, OnClickOutPoint, RemoveNode);
            Nodes.Add(node);

            SetDataDirty();
            return node;
        }

        /// <summary>
        /// Removes the provided node
        /// </summary>
        /// <param name="node"></param>
        protected void RemoveNode(TNode node)
        {
            // if there are connections, we need to test if the node we want to delete has any and if it has, delete them
            if (Connections != null)
            {
                // go through all connections backwards so we can delete them
                for (int i = Connections.Count - 1; i >= 0; --i)
                {
                    // If the node has one of the Connection points, we need to delete the connection
                    if (node.InPoints.Search(Connections[i].InPoint) != -1 || node.OutPoints.Search(Connections[i].OutPoint) != -1)
                    {
                        Connections.RemoveAt(i);
                    }
                }
            }

            // if the selected in point belongs to the node to delete, unselect it
            if (node.InPoints.Search(_selectedInPoint) != -1)
                _selectedInPoint = null;

            // if the selected out point belongs to the node to delete, unselect it
            if (node.OutPoints.Search(_selectedOutPoint) != -1)
                _selectedOutPoint = null;

            // delete the node
            Nodes.Remove(node);

            SetDataDirty();
        }

        /// <summary>
        /// Executes dragging the canvas
        /// </summary>
        /// <param name="delta"></param>
        private void OnDrag(Vector2 delta)
        {
            _drag = delta;

            // if there are nodes, we need to move them as well
            if (Nodes != null)
            {
                foreach (Node<TNode, TNodeData> node in Nodes)
                {
                    node.Drag(delta);
                }
            }

            GUI.changed = true;

            SetDataDirty();
        }

        /// <summary>
        /// Function for when user clicks In Point. It selets it and creates a connection if an Out Point is selected
        /// </summary>
        /// <param name="inPoint"></param>
        private void OnClickInPoint(ConnectionPoint<TNode, TNodeData> inPoint)
        {
            // select point
            _selectedInPoint = inPoint;

            // if there is a seleceted Out point, check whether it belongs to another node and if yes, create the connection
            if (_selectedOutPoint != null)
            {
                if (_selectedOutPoint.PointNode != _selectedInPoint.PointNode)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        /// <summary>
        ///  Function for when user clicks Out Point. It selets it and creates a connection if an In Point is selected
        /// </summary>
        /// <param name="outPoint"></param>
        private void OnClickOutPoint(ConnectionPoint<TNode, TNodeData> outPoint)
        {
            // select point
            _selectedOutPoint = outPoint;

            // if there is a seleceted In point, check whether it belongs to another node and if yes, create the connection
            if (_selectedInPoint != null)
            {
                if (_selectedOutPoint.PointNode != _selectedInPoint.PointNode)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        /// <summary>
        /// Creates a connection between _selectedInPoint and _selectedOutPoint;
        /// </summary>
        protected void CreateConnection()
        {
            if (Connections == null)
                Connections = new List<Connection<TNode, TNodeData>>();


            foreach(Connection<TNode, TNodeData> conection in Connections)
            {
                if (conection.InPoint == _selectedInPoint && conection.OutPoint == _selectedOutPoint)
                    return;
            }

            Connection<TNode, TNodeData> con = new Connection<TNode, TNodeData>(_selectedInPoint, _selectedOutPoint, RemoveConnection);
            Connections.Add(con);

            if (OnConnectionMade != null)
                OnConnectionMade(con);
        }

        /// <summary>
        /// Creates a connection between the given points
        /// </summary>
        /// <param name="inPoint"></param>
        /// <param name="outPoint"></param>
        protected void CreateConnection(ConnectionPoint<TNode, TNodeData> inPoint, ConnectionPoint<TNode, TNodeData> outPoint)
        {
            if (inPoint.Type != ConnectionPointType.IN || outPoint.Type != ConnectionPointType.OUT)
            {
                Debug.LogError("Can only create connection from out point to in point");
            }

            if (inPoint.PointNode == outPoint.PointNode)
            {
                Debug.LogError("Cannot create connection from node to itself");
                return;
            }

            _selectedInPoint = inPoint;
            _selectedOutPoint = outPoint;
            CreateConnection();
        }

        private void RemoveConnection(Connection<TNode, TNodeData> connection)
        {
            if (OnConnectionRemoved != null)
                OnConnectionRemoved(connection);

            Connections.Remove(connection);
        }

        private void ClearConnectionSelection()
        {
            _selectedInPoint = null;
            _selectedOutPoint = null;
        }

        // ######################## DATA ######################## //
        /// <summary>
        /// Marks the data as dirty
        /// </summary>
        public void SetDataDirty()
        {
            EditorUtility.SetDirty(_data);
        }

        /// <summary>
        /// Attempts to add the provided node to the Data. If a node with the same ID already exists, it returns that node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TNodeData AddNodeToData(TNodeData node)
        {
            SetDataDirty();
            return _data.AddNode(node) as TNodeData;
        }

        /// <summary>
        /// Returns a node by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TNodeData GetNodeFromData(int ID)
        {
            return _data.GetNode(ID) as TNodeData;
        }

        /// <summary>
        /// Deletes the provided node, also deleting any references in other nodes to it
        /// </summary>
        /// <param name="node"></param>
        public void DeleteNodeFromData(TNodeData node)
        {
            SetDataDirty();
            _data.DeleteNode(node);
        }


        // ######################## UTILITIES ######################## //
        /// <summary>
        /// Registers a node type for the Create Node Context menu
        /// </summary>
        /// <param name="displayName">Display Name of the Node Type</param>
        /// <param name="template">Template Node</param>
        protected void AddNodeType(string displayName, TNode template)
        {
            _contextMenuEntries.Add("New " + displayName, position => AddNode((Vector2)position, template));
        }

        /// <summary>
        /// Adds an entry to the right click context menu
        /// </summary>
        /// <param name="displayName">string to display in the menu</param>
        /// <param name="function">Function to execute, userData contains the mousePosition of the click that opened the menu</param>
        protected void AddContextMenuEntrie(string displayName, GenericMenu.MenuFunction2 function)
        {
            _contextMenuEntries.Add(displayName, function);
        }

        /// <summary>
        /// Draws a grid on the GUI
        /// </summary>
        /// <param name="gridSpacing"></param>
        /// <param name="gridOpacity"></param>
        /// <param name="gridColor"></param>
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            Offset += _drag * 0.5f;
            Vector3 newOffset = new Vector3(Offset.x % gridSpacing, Offset.y % gridSpacing, 0);

            for (int i = 0; i <= widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height + gridSpacing, 0f) + newOffset);
            }

            for (int j = 0; j <= heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width + gridSpacing, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        /// <summary>
        /// Draws a Bezier Courve with Orizontal Tangents from start to end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="fromInPoint"></param>
        private void DrawBezier(Vector2 start, Vector2 end, bool fromInPoint)
        {
            Vector2 startTangent = Vector2.zero;
            Vector2 endTangent = Vector2.zero;
            if(fromInPoint)
            {
                startTangent = start + Vector2.left * 100f;
                endTangent = end - Vector2.left * 100f;
            } else
            {
                startTangent = start - Vector2.left * 100f;
                endTangent = end + Vector2.left * 100f;
            }

            Handles.DrawBezier(
                    start,
                    end,
                    startTangent,
                    endTangent,
                    Color.white,
                    null,
                    2f
                );

            GUI.changed = true;
        }

        /// <summary>
        /// Creates the Background Texture
        /// </summary>
        private void CreateBackground()
        {
            Background = new Texture2D(1, 1);
            Background.SetPixel(1, 1, new Color(51f / 255, 51f / 255, 51f / 255, 1));
            Background.Apply();
        }
    }
}