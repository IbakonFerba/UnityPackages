using UnityEngine;
using System.Collections.Generic;

namespace FK.Editor.NodeEditor
{
    /// <summary>
    /// <para>A List of connections to a Connection Point of another Node</para>
    /// 
    /// v2.1 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [System.Serializable]
    public class ConnectionPointData
    {
        /// <summary>
        /// All Connections with this Connection Point
        /// </summary>
        public List<NodeConnectionData> Connections;

        /// <summary>
        /// Returns true if the Connection Point Contains a Connection to the given point
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="pointIndex"></param>
        /// <returns></returns>
        public bool Contains(int nodeID, int pointIndex)
        {
            foreach(NodeConnectionData connection in Connections)
            {
                if(connection.NodeID == nodeID && connection.PointIndex == pointIndex)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the Connection Point Contains any conenction to the given node
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        public bool Contains(int nodeID)
        {
            foreach (NodeConnectionData connection in Connections)
            {
                if (connection.NodeID == nodeID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a Connection to the provided point
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="pointIndex"></param>
        public void Add(int nodeID, int pointIndex)
        {
            Connections.Add(new NodeConnectionData(nodeID, pointIndex));
        }

        /// <summary>
        /// If a connection to the provided Point exists, it will be removed
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="pointIndex"></param>
        public void Remove(int nodeID, int pointIndex)
        {
            foreach (NodeConnectionData connection in Connections)
            {
                if (connection.NodeID == nodeID && connection.PointIndex == pointIndex)
                {
                    Connections.Remove(connection);
                    return;
                }
            }
        }

        /// <summary>
        /// Removes all Connections to the provided node
        /// </summary>
        /// <param name="nodeID"></param>
        public void Remove(int nodeID)
        {
            for(int i = Connections.Count -1; i >= 0; --i)
            {
                if (Connections[i].NodeID == nodeID)
                {
                    Connections.RemoveAt(i);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// <para>Data for a Connection to a Connection Point</para>
    /// 
    /// v2.0 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [System.Serializable]
    public class NodeConnectionData
    {
        /// <summary>
        /// The ID of the Node this Connection Goes to
        /// </summary>
        public int NodeID;
        /// <summary>
        /// The Index of the Connection Point of the Node this Connection Goes to
        /// </summary>
        public int PointIndex;

        public NodeConnectionData(int nodeID, int pointIndex)
        {
            NodeID = nodeID;
            PointIndex = pointIndex;
        }
    }

    /// <summary>
    /// <para>Data for a NodeEditor Node</para>
    /// 
    /// v2.0 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [System.Serializable]
    public class NodeDataBase
    {
        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// ID of the Node, should be unique
        /// </summary>
        public int ID;

        /// <summary>
        /// The Node IDs of nodes connected to the Inputs. The index of the ConnectionPointData Object is the Input the node is connected to, it contains a list of Connections because more than one node can be connected to the same input
        /// </summary>
        public ConnectionPointData[] Inputs = null;
        /// <summary>
        /// The Node IDs of nodes connected to the Outputs. The index of the ConnectionPointData Object is the Output the node is connected to, it contains a list of Connections because more than one node can be connected to the same input
        /// </summary>
        public ConnectionPointData[] Outputs = null;

        /// <summary>
        /// The position of the node in the editor window
        /// </summary>
        public Vector2 PositionInEditor;

        /// <summary>
        /// Width of the Node in the Editor
        /// </summary>
        public float Width;
        /// <summary>
        /// Preferred Height of the Node in the Editor
        /// </summary>
        public float PreferredHeight;



        // ######################## INITS ######################## //
        public NodeDataBase() { }
        public NodeDataBase(int id)
        {
            ID = id;
        }
    }
}