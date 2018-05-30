﻿using UnityEngine;


namespace FK.Editor.NodeEditor
{
    public enum ConnectionPointType
    {
        IN,
        OUT
    }

    /// <summary>
    /// A connetion point for Nodes of my NodeEditor. It knows which Node it belongs to, so you can use it to follow conenctions
    /// 
    /// This was created using this Tutorial as a base: http://gram.gs/gramlog/creating-node-based-editor-unity/
    /// 
    /// 05/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class ConnectionPoint
    {
        // ######################## ENUMS & DELEGATES ######################## //
        public delegate void DelOnClickConnectionPoint(ConnectionPoint point);


        // ######################## PUBLIC VARS ######################## //
        public ConnectionPointType Type;

        public Rect PointRect;
        /// <summary>
        /// The Node this point belongs to
        /// </summary>
        public Node PointNode;

        /// <summary>
        /// The Index of the point on the Node
        /// </summary>
        public int Index;

        /// <summary>
        /// Delegate that is invoked when the user clicks on the point
        /// </summary>
        public DelOnClickConnectionPoint OnClickConnectionPoint;

        // ######################## PRIVATE VARS ######################## //
        private GUIStyle _style;

        /// <summary>
        /// The total number of Connection Points of the same type on the parent node
        /// </summary>
        private int _numOfPointsOnNode;

        // ######################## INITS ######################## //
        public ConnectionPoint(Node node, ConnectionPointType type, int index, int numOfSamePoints, GUIStyle style, DelOnClickConnectionPoint OnClick)
        {
            PointNode = node;
            Type = type;
            Index = index;
            _numOfPointsOnNode = numOfSamePoints;
            _style = style;
            OnClickConnectionPoint += OnClick;
            PointRect = new Rect(0, 0, 10f, 20f);
        }

        // ######################## FUNCTIONALITY ######################## //
        public void Draw()
        {
            // set y position according to the total number of same points and the index of this point on the node
            PointRect.y = PointNode.NodeRect.y + (PointNode.NodeRect.height * Index / _numOfPointsOnNode) + (PointNode.NodeRect.height / _numOfPointsOnNode) / 2 - PointRect.height * 0.5f;

            switch (Type)
            {
                case ConnectionPointType.IN:
                    // on the left
                    PointRect.x = PointNode.NodeRect.x - PointRect.width + 8f;
                    break;

                case ConnectionPointType.OUT:
                    // on the right
                    PointRect.x = PointNode.NodeRect.x + PointNode.NodeRect.width - 8f;
                    break;
            }

            // Button to make point clickable
            if (GUI.Button(PointRect, "", _style))
            {
                if (OnClickConnectionPoint != null)
                    OnClickConnectionPoint(this);
            }
        }
    }
}