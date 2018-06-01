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
    /// 05/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public abstract class Node : ICloneable
    {
        // ######################## ENUMS & DELEGATES ######################## //
        /// <summary>
        /// Delegate that is called when a node should be removed
        /// </summary>
        /// <param name="node">Node to be removes</param>
        public delegate void DelOnRemoveNode(Node node);
        /// <summary>
        /// Delegate for when the node is dragged
        /// </summary>
        /// <param name="newPos"></param>
        public delegate void DelOnDrag(Vector2 newPos);

        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// The ID of the Node. You can use this any way you want, this value is not used internaly
        /// </summary>
        public string ID = "ID";
        /// <summary>
        /// Title of the Node that is displayed at the top of the node
        /// </summary>
        public string Title = "Title";

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
        public ConnectionPoint[] InPoints;
        /// <summary>
        /// All Out Points
        /// </summary>
        public ConnectionPoint[] OutPoints;

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
        protected NodeEditorBase Editor;

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
        public Node(NodeEditorBase editor, Vector2 position, float width, float height, int numOfInPoints, int numOfOutPoints)
        {
            Editor = editor;
            NodeRect = new Rect(position.x, position.y, width, height);

            InPoints = new ConnectionPoint[numOfInPoints];
            OutPoints = new ConnectionPoint[numOfOutPoints];

            _whiteText = new GUIStyle();
            _whiteText.normal.textColor = Color.white;

            _preferredHeight = height;

            GUI.changed = true;
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
        public void Init(Vector2 position, GUIStyle style, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, ConnectionPoint.DelOnClickConnectionPoint OnClickInPoint, ConnectionPoint.DelOnClickConnectionPoint OnClickOutPoint, DelOnRemoveNode OnRemove)
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
            InPoints = new ConnectionPoint[InPoints.Length];
            OutPoints = new ConnectionPoint[OutPoints.Length];
            for (int i = 0; i < InPoints.Length; ++i)
            {
                InPoints[i] = new ConnectionPoint(this, ConnectionPointType.IN, i, InPoints.Length, inPointStyle, OnClickInPoint);
            }

            for (int i = 0; i < OutPoints.Length; ++i)
            {
                OutPoints[i] = new ConnectionPoint(this, ConnectionPointType.OUT, i, OutPoints.Length, outPointStyle, OnClickOutPoint);
            }

            Init();
        }

        /// <summary>
        /// Contains any initialization that the Node might need appart from the general things
        /// </summary>
        protected abstract void Init();

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
            foreach (ConnectionPoint point in InPoints)
            {
                point.Draw();
            }

            foreach (ConnectionPoint point in OutPoints)
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
        protected abstract void DrawContent();

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
                OnRemoveNode(this);
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
    }
}