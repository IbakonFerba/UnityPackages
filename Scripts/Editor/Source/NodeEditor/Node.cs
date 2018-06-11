using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace FK.Editor.NodeEditor
{
    /// <summary>
    /// Base for a Node of the Node Editor
    /// 
    /// The abstract function Init() is called on a new node when it is created in the editor.
    /// In the abstract function DrawContent() you can draw all the content of the Node using EditorGUILayout
    /// 
    /// This was created using this Tutorial as a base: http://gram.gs/gramlog/creating-node-based-editor-unity/
    /// 
    /// v2.1 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class Node<T, TData> : ICloneable where T : Node<T, TData>, new() where TData : NodeDataBase, new()
    {
        // ######################## ENUMS & DELEGATES ######################## //
        /// <summary>
        /// Delegate that is called when a node should be removed
        /// </summary>
        /// <param name="node">Node to be removes</param>
        public delegate void DelOnRemoveNode(T node);
        /// <summary>
        /// Delegate for when the node is dragged
        /// </summary>
        /// <param name="newPos"></param>
        public delegate void DelOnDrag(Vector2 newPos);

        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// The ID of the Node.
        /// </summary>
        public string ID = "ID";
        /// <summary>
        /// Title of the Node that is displayed at the top of the node
        /// </summary>
        public string Title = "Title";

        /// <summary>
        /// The Data of this Node in the asset we are editing
        /// </summary>
        public TData Data;

        /// <summary>
        /// The Rect of the node
        /// </summary>
        public Rect NodeRect;
        /// <summary>
        /// Inner Border of the Node. Basically the offset between the Node boundaries and the content area
        /// </summary>
        public Rect Border = new Rect(12, 12, 12, 12);

        /// <summary>
        /// All In Points
        /// </summary>
        public ConnectionPoint<T, TData>[] InPoints;
        /// <summary>
        /// All Out Points
        /// </summary>
        public ConnectionPoint<T, TData>[] OutPoints;

        /// <summary>
        /// Delegate that is invoked when the Node should be removed
        /// </summary>
        public DelOnRemoveNode OnRemoveNode;
        /// <summary>
        /// Delegate that is invoked when the Node is dragged
        /// </summary>
        public DelOnDrag OnDrag;

        // ######################## PROTECTED VARS ######################## //
        /// <summary>
        /// Reference to the Editor this Node belongs to so you can access the connections for example
        /// </summary>
        protected NodeEditorBase<T, TData> Editor;

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// Context Menu entries with display name and function to execute
        /// </summary>
        private Dictionary<string, GenericMenu.MenuFunction> _contextMenuEntries;

        /// <summary>
        /// The min height of the node
        /// </summary>
        private float _preferredHeight;
        /// <summary>
        /// Current style of the node
        /// </summary>
        private GUIStyle _style;
        /// <summary>
        /// Default style of the node
        /// </summary>
        private GUIStyle _defaultNodeStyle;
        /// <summary>
        /// Style of the node when selected
        /// </summary>
        private GUIStyle _selectedNodeStyle;

        /// <summary>
        /// Gui style for labels with white text
        /// </summary>
        private GUIStyle _whiteText;

        private bool _isDragged;
        private bool _isSelected;

        // ######################## INITS ######################## //
        public Node() { }

        /// <summary>
        /// Initializes values of the Node
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="numOfInPoints"></param>
        /// <param name="numOfOutPoints"></param>
        public void SetUp(NodeEditorBase<T, TData> editor, Vector2 position, float width, float height, int numOfInPoints, int numOfOutPoints)
        {
            ID = null;

            Editor = editor;
            NodeRect = new Rect(position.x, position.y, width, height);

            InPoints = new ConnectionPoint<T, TData>[numOfInPoints];
            OutPoints = new ConnectionPoint<T, TData>[numOfOutPoints];

            _whiteText = new GUIStyle();
            _whiteText.normal.textColor = Color.white;

            _preferredHeight = height;

            GUI.changed = true;
        }

        /// <summary>
        /// Initializes values of the Node
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="id"></param>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="numOfInPoints"></param>
        /// <param name="numOfOutPoints"></param>
        public void SetUp(NodeEditorBase<T, TData> editor, string id, Vector2 position, float width, float height, int numOfInPoints, int numOfOutPoints)
        {
            SetUp(editor, position, width, height, numOfInPoints, numOfOutPoints);
            ID = id;
        }

        public Node(NodeEditorBase<T, TData> editor, Vector2 position, float width, float height, int numOfInPoints, int numOfOutPoints)
        {
            SetUp(editor, position, width, height, numOfInPoints, numOfOutPoints);
        }

        public Node(NodeEditorBase<T, TData> editor, string id, Vector2 position, float width, float height, int numOfInPoints, int numOfOutPoints)
        {
            SetUp(editor, id, position, width, height, numOfInPoints, numOfOutPoints);
        }

        /// <summary>
        /// Initializes the Node. This is called by the NodeEditor automatically when a Node is created
        /// </summary>
        /// <param name="position"></param>
        /// <param name="style"></param>
        /// <param name="selectedStyle"></param>
        /// <param name="inPointStyle"></param>
        /// <param name="outPointStyle"></param>
        /// <param name="OnClickInPoint"></param>
        /// <param name="OnClickOutPoint"></param>
        /// <param name="OnRemove"></param>
        public void Init(Vector2 position, GUIStyle style, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, ConnectionPoint<T, TData>.DelOnClickConnectionPoint OnClickInPoint, ConnectionPoint<T, TData>.DelOnClickConnectionPoint OnClickOutPoint, DelOnRemoveNode OnRemove)
        {
            // set position
            NodeRect.position = position;

            // set styles
            _defaultNodeStyle = style;
            _selectedNodeStyle = selectedStyle;
            _style = _defaultNodeStyle;

            // add delegate
            OnRemoveNode += OnRemove;

            _contextMenuEntries = new Dictionary<string, GenericMenu.MenuFunction>();
            AddContextMenuEntrie("Remove Node", () => OnClickRemoveNode());

            // setup connection points
            InPoints = new ConnectionPoint<T, TData>[InPoints.Length];
            OutPoints = new ConnectionPoint<T, TData>[OutPoints.Length];
            for (int i = 0; i < InPoints.Length; ++i)
            {
                InPoints[i] = new ConnectionPoint<T, TData>(this as T, ConnectionPointType.IN, i, InPoints.Length, inPointStyle, OnClickInPoint);
            }

            for (int i = 0; i < OutPoints.Length; ++i)
            {
                OutPoints[i] = new ConnectionPoint<T, TData>(this as T, ConnectionPointType.OUT, i, OutPoints.Length, outPointStyle, OnClickOutPoint);
            }

            // if the node has no ID yet, get one from the editor
            if (string.IsNullOrEmpty(ID))
            {
                ID = Editor.NextNodeID;
            }

            // Initialize the Data
            TData dat = new TData();
            dat.ID = ID;
            dat.Width = NodeRect.width;
            dat.PreferredHeight = _preferredHeight;
            Data = Editor.AddNodeToData(dat);
            Data.PositionInEditor = NodeRect.position;

            // if the data has no inputs yet, create them
            if (Data.Inputs == null)
            {
                Data.Inputs = new ConnectionPointData[InPoints.Length];
            }

            // if the data has no outputs yet, create them
            if (Data.Outputs == null)
            {
                Data.Outputs = new ConnectionPointData[OutPoints.Length];
            }

            // add some callbacks
            OnDrag += newPos => Data.PositionInEditor = newPos;
            OnRemoveNode += n => Editor.DeleteNodeFromData(Data);

            Editor.OnConnectionMade += SaveConnection;
            Editor.OnConnectionRemoved += DeleteConnection;


            Init();
        }

        /// <summary>
        /// Contains any initialization that the Node might need appart from the general things
        /// </summary>
        protected virtual void Init() { }

        /// <summary>
        /// Clones the Node and returns the clone
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Moves the Node by delta
        /// </summary>
        /// <param name="delta"></param>
        public void Drag(Vector2 delta)
        {
            NodeRect.position += delta;

            if (OnDrag != null)
                OnDrag(NodeRect.position);
        }

        public void Draw()
        {
            // draw connection points
            foreach (ConnectionPoint<T, TData> point in InPoints)
            {
                point.Draw();
            }

            foreach (ConnectionPoint<T, TData> point in OutPoints)
            {
                point.Draw();
            }

            // draw the Node Backgroud
            GUI.Box(NodeRect, "", _style);

            // calculate Content Rect
            Rect contentRect = new Rect(NodeRect.position.x + Border.x, NodeRect.position.y + Border.y, NodeRect.width - (Border.x + Border.width), NodeRect.height - (Border.y + Border.height));

            // Begin Content Area
            GUILayout.BeginArea(contentRect);
            Rect newContentRect = EditorGUILayout.BeginVertical();

            // Add Title
            EditorGUILayout.LabelField(Title, EditorStyles.boldLabel);

            // draw custom content
            DrawContent();

            // End Content Area
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();

            // if we are repainting, calculate the Node height
            if (Event.current.type == EventType.Repaint)
                NodeRect.height = Border.y + newContentRect.height + Border.height;

            // if the height of the node is less then the preferred height, set it to the preferred height
            if (NodeRect.height < _preferredHeight)
                NodeRect.height = _preferredHeight;
        }

        /// <summary>
        /// Processes the given Event
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    // the user pressed left mouse button
                    if (e.button == 0)
                    {
                        // check whether the node should be selected and dragged
                        if (NodeRect.Contains(e.mousePosition))
                        {
                            // select and start drag
                            _isSelected = true;
                            _isDragged = true;
                            _style = _selectedNodeStyle;

                            // unfocus any GUI Elements
                            GUI.FocusControl(string.Empty);
                            GUI.changed = true;
                        }
                        else
                        {
                            // deselect
                            _isSelected = false;
                            _style = _defaultNodeStyle;
                            GUI.changed = true;
                        }
                    }

                    // user pressed right mouse button
                    if (e.button == 1)
                    {
                        // check whether the node should be selected and context menu should be shown
                        if (NodeRect.Contains(e.mousePosition))
                        {
                            // select
                            _isSelected = true;
                            _style = _selectedNodeStyle;

                            ProcessContextMenu();
                            e.Use();

                            // unfocus any GUI Elements
                            GUI.FocusControl(string.Empty);
                            GUI.changed = true;
                            return true;
                        }
                    }
                    break;

                case EventType.MouseUp:
                    _isDragged = false;
                    break;

                case EventType.MouseDrag:
                    // drag if left mouse button is held
                    if (e.button == 0 && _isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;

                case EventType.KeyDown:
                    // delete node if selected and Delete Key is pressed
                    if (e.keyCode == KeyCode.Delete && _isSelected)
                    {
                        OnClickRemoveNode();
                        GUI.changed = true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Draws all custom content of the node
        /// </summary>
        protected virtual void DrawContent() { }

        /// <summary>
        /// Processes and displays the Node Context Menu
        /// </summary>
        private void ProcessContextMenu()
        {
            // create the menu
            GenericMenu genericMenu = new GenericMenu();

            // fill in entries
            foreach (KeyValuePair<string, GenericMenu.MenuFunction> pair in _contextMenuEntries)
            {
                genericMenu.AddItem(new GUIContent(pair.Key), false, pair.Value);
            }

            // show menu
            genericMenu.ShowAsContext();
        }

        /// <summary>
        /// Called when the node should be removed
        /// </summary>
        private void OnClickRemoveNode()
        {
            if (OnRemoveNode != null)
                OnRemoveNode(this as T);
        }

        /// <summary>
        /// Adds an entry to the right click context menu
        /// </summary>
        /// <param name="displayName">string to display in the menu</param>
        /// <param name="function">Function to execute</param>
        protected void AddContextMenuEntrie(string displayName, GenericMenu.MenuFunction function)
        {
            if (_contextMenuEntries.ContainsKey(displayName))
                return;

            _contextMenuEntries.Add(displayName, function);
        }



        // ######################## DATA ######################## //
        /// <summary>
        /// Saves a created Connection
        /// </summary>
        /// <param name="connection"></param>
        private void SaveConnection(Connection<T, TData> connection)
        {
            // if the inPoint of the Connection belongs to this node, save the Node of the Outpoint at the appropriate place
            if (connection.InPoint.PointNode == this)
            {
                ConnectionPointData Inputs = Data.Inputs[connection.InPoint.Index];
                if (Inputs.Connections == null)
                    Inputs.Connections = new List<NodeConnectionData>();

                if (!Inputs.Contains(connection.OutPoint.PointNode.ID, connection.OutPoint.Index))
                {
                    Inputs.Add(connection.OutPoint.PointNode.ID, connection.OutPoint.Index);
                }
            }

            // if the outPoint of the Connection belongs to this node, save the Node of the inPoint at the appropriate place
            if (connection.OutPoint.PointNode == this)
            {
                ConnectionPointData Outputs = Data.Outputs[connection.OutPoint.Index];
                if (Outputs.Connections == null)
                    Outputs.Connections = new List<NodeConnectionData>();

                if (!Outputs.Contains(connection.InPoint.PointNode.ID, connection.InPoint.Index))
                {
                    Outputs.Add(connection.InPoint.PointNode.ID, connection.InPoint.Index);
                }
            }

            Editor.SetDataDirty();
        }

        /// <summary>
        /// Deletes a Connection from the Data
        /// </summary>
        /// <param name="connection"></param>
        private void DeleteConnection(Connection<T, TData> connection)
        {
            // if the inPoint of the Connection belongs to this node, delete the reference to the Node of the outpoint 
            if (connection.InPoint.PointNode == this)
            {
                Data.Inputs[connection.InPoint.Index].Remove(connection.OutPoint.PointNode.ID, connection.OutPoint.Index);
            }

            // if the outPoint of the Connection belongs to this node, delete the reference to the Node of the inPoint 
            if (connection.OutPoint.PointNode == this)
            {
                Data.Outputs[connection.OutPoint.Index].Remove(connection.InPoint.PointNode.ID, connection.InPoint.Index);
            }

            Editor.SetDataDirty();
        }
    }
}