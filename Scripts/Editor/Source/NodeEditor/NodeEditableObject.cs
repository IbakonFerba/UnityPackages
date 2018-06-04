using UnityEngine;
using System.Collections.Generic;
using FK.Editor.NodeEditor;

/// <summary>
/// Base Class for an asset that can be edited with an Node Editor
///
/// v2.0 06/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
/// <typeparam name="T"></typeparam>
public class NodeEditableObject<T> : ScriptableObject where T : NodeDataBase
{
    // ######################## PUBLIC VARS ######################## //
    /// <summary>
    /// All Nodes of this Object
    /// </summary>
    [HideInInspector]
    public List<T> Nodes = new List<T>();
    /// <summary>
    /// The last Node ID that was used when a node was created
    /// </summary>
    [HideInInspector]
    public int LastNodeId;
    /// <summary>
    /// Offset of the contents of the editor window
    /// </summary>
    [HideInInspector]
    public Vector2 EditorOffset;


    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Attempts to add the provided node to the List of nodes. If a node with the same ID already exists, it returns that node.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public T AddNode(T node)
    {
        foreach (T n in Nodes)
        {
            if (n.ID == node.ID)
                return n;
        }

        Nodes.Add(node);
        return node;
    }

    /// <summary>
    /// Deletes the provided node, also deleting any references in other nodes to it
    /// </summary>
    /// <param name="node"></param>
    public void DeleteNode(T node)
    {
        // if the node does not exist, do nothing
        if (!Nodes.Contains(node))
            return;

        // look at every node
        foreach (T n in Nodes)
        {
            // look at every input of the current node and check whether it is connected to the node we want to remove, if yes delete the conection
            foreach (ConnectionPointData input in n.Inputs)
            {
                if (input.Connections != null)
                {
                    if (input.Contains(node.ID))
                        input.Remove(node.ID);
                }
            }

            // look at every output of the current node and check whether it is connected to the node we want to remove, if yes delete the conection
            foreach (ConnectionPointData output in n.Outputs)
            {
                if (output.Connections != null)
                {
                    if (output.Contains(node.ID))
                        output.Remove(node.ID);
                }
            }
        }

        // delete the node
        Nodes.Remove(node);
    }

    /// <summary>
    /// Returns a node by its ID
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public T GetNode(string ID)
    {
        foreach (T node in Nodes)
        {
            if (node.ID == ID)
                return node;
        }

        return null;
    }
}
