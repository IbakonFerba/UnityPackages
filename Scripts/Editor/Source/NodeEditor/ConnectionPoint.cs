using UnityEngine;


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
    /// v2.0 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TNodeData"></typeparam>
    public class ConnectionPoint<TNode, TNodeData> where TNode : Node<TNode, TNodeData>, new() where TNodeData : NodeDataBase, new()
    {
        // ######################## ENUMS & DELEGATES ######################## //
        public delegate void DelOnClickConnectionPoint(ConnectionPoint<TNode, TNodeData> point);


        // ######################## PUBLIC VARS ######################## //
        public ConnectionPointType Type;

        public Rect PointRect;
        /// <summary>
        /// The Node this point belongs to
        /// </summary>
        public TNode PointNode;

        /// <summary>
        /// Label that is shown in the Editor
        /// </summary>
        public string Label;

        /// <summary>
        /// The Index of the point on the Node
        /// </summary>
        public int Index;

        /// <summary>
        /// Delegate that is invoked when the user clicks on the point
        /// </summary>
        public DelOnClickConnectionPoint OnClickConnectionPoint;

        // ######################## PRIVATE VARS ######################## //
        private Rect _labelRect;

        private GUIStyle _style;
        private GUIStyle _labelStyle;

        /// <summary>
        /// The total number of Connection Points of the same type on the parent node
        /// </summary>
        private int _numOfPointsOnNode;

        // ######################## INITS ######################## //
        public ConnectionPoint(TNode node, ConnectionPointType type, int index, int numOfSamePoints, GUIStyle style, DelOnClickConnectionPoint OnClick)
        {
            PointNode = node;
            Type = type;
            Index = index;
            _numOfPointsOnNode = numOfSamePoints;
            _style = style;
            OnClickConnectionPoint += OnClick;
            PointRect = new Rect(0, 0, 10f, 20f);

            _labelStyle = new GUIStyle();
            _labelStyle.normal.textColor = Color.white;
            _labelStyle.stretchWidth = true;

            _labelRect = new Rect(PointRect.position.x, PointRect.position.x, 0, PointRect.height);
        }

        // ######################## FUNCTIONALITY ######################## //
        public void Draw()
        {
            // set y position according to the total number of same points and the index of this point on the node
            PointRect.y = PointNode.NodeRect.y + (PointNode.NodeRect.height * Index / _numOfPointsOnNode) + (PointNode.NodeRect.height / _numOfPointsOnNode) / 2 - PointRect.height * 0.5f;
            _labelRect.y = PointRect.y-PointRect.height/2 - 4f;

            switch (Type)
            {
                case ConnectionPointType.IN:
                    // on the left
                    PointRect.x = PointNode.NodeRect.x - PointRect.width + 8f;
                    _labelStyle.alignment = TextAnchor.UpperRight;
                    _labelRect.x = PointRect.x + PointRect.width - 4f;
                    break;

                case ConnectionPointType.OUT:
                    // on the right
                    PointRect.x = PointNode.NodeRect.x + PointNode.NodeRect.width - 8f;
                    _labelStyle.alignment = TextAnchor.UpperLeft;
                    _labelRect.x = PointRect.x + 4f;
                    break;
            }

            // Button to make point clickable
            if (GUI.Button(PointRect, "", _style))
            {
                if (OnClickConnectionPoint != null)
                    OnClickConnectionPoint(this);
            }

            GUI.Label(_labelRect, Label, _labelStyle);
        }
    }
}