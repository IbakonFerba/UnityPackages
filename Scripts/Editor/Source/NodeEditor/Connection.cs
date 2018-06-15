using UnityEngine;
using UnityEditor;

namespace FK.Editor.NodeEditor
{
    /// <summary>
    /// <para>A Connection of my NodeEditor</para>
    /// 
    /// <para>This was created using this Tutorial as a base: http://gram.gs/gramlog/creating-node-based-editor-unity/ </para>
    /// 
    /// v2.0 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TNodeData"></typeparam>
    public class Connection<TNode, TNodeData> where TNode : Node<TNode, TNodeData>, new() where TNodeData : NodeDataBase, new()
    {
        // ######################## ENUMS & DELEGATES ######################## //
        public delegate void DelOnClickRemoveConnection(Connection<TNode, TNodeData> connection);


        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// The In Point of this Connection
        /// </summary>
        public ConnectionPoint<TNode, TNodeData> InPoint;
        /// <summary>
        /// The Out Point of this connection
        /// </summary>
        public ConnectionPoint<TNode, TNodeData> OutPoint;

        /// <summary>
        /// Delegate that is invoked when the Connection should be removed
        /// </summary>
        public DelOnClickRemoveConnection OnClickRemoveConnection;


        // ######################## INITS ######################## //
        public Connection(ConnectionPoint<TNode, TNodeData> inPoint, ConnectionPoint<TNode, TNodeData> outPoint, DelOnClickRemoveConnection removeConnection)
        {
            InPoint = inPoint;
            OutPoint = outPoint;

            OnClickRemoveConnection += removeConnection;
        }

        // ######################## FUNCTIONALITY ######################## //
        public void Draw()
        {
            Handles.DrawBezier(
                InPoint.PointRect.center,
                OutPoint.PointRect.center,
                InPoint.PointRect.center + Vector2.left * 100f,
                OutPoint.PointRect.center - Vector2.left * 100f,
                Color.white,
                null,
                2f
                );

            // button to remove Connection
            if (Handles.Button((InPoint.PointRect.center + OutPoint.PointRect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                if (OnClickRemoveConnection != null)
                    OnClickRemoveConnection(this);
            }
        }
    }
}