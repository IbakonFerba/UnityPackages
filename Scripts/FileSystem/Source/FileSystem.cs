using UnityEngine;
using System.Collections.Generic;
using System.IO;
using FK.Utility.ArraysAndLists;

namespace FK.IO
{
    /// <summary>
    /// Information of a File System entry
    /// </summary>
    public struct FileSystemEntry
    {
        /// <summary>
        /// The NAme without the Path
        /// </summary>
        public string DisplayName;
        /// <summary>
        /// The complete Path of the entry
        /// </summary>
        public string Path;
        /// <summary>
        /// The Extension of the Entry
        /// </summary>
        public string Extension;
        /// <summary>
        /// Type of the Entry (DIRECTORY or FILE)
        /// </summary>
        public NodeType Type;

        public FileSystemEntry(string displayName, string path, string extension, NodeType type)
        {
            DisplayName = displayName;
            Path = path;
            Extension = extension;
            Type = type;
        }
    }

    public enum NodeType
    {
        DIRECTORY,
        FILE
    }

    /// <summary>
    /// A FileSystem that acesses the File System of the used Device and loads entries as they are needed, while saving them when they where visited so they don't have to be loaded every time
    /// It provides easy traversal of the File Tree while only Displaying Files you want to see if you give it a list of extensions.
    /// You can define a path you want to go to and it loads that path and gives you all entries and it provides a history of entered directories.
    /// If needed, it can recursively reload all the Nodes that are loaded
    /// </summary>
    public class FileSystem
    {
        // ######################## STRUCTS & HELPER CLASSES ######################## //
        /// <summary>
        /// A File System Node representing a Directory or File
        /// </summary>
        private class Node
        {
            /// <summary>
            /// The Path URL of this Node
            /// </summary>
            public string PathURL { get; private set; }
            /// <summary>
            /// The Name of the Element this Node represents without the Path
            /// </summary>
            public string DisplayName { get; private set; }
            /// <summary>
            /// The File extension of the Element if it has one, if not this is empty
            /// </summary>
            public string Extension { get; private set; }
            /// <summary>
            /// The Parent Node if there is one
            /// </summary>
            public Node Parent { get; private set; }
            /// <summary>
            /// All Child Nodes
            /// </summary>
            public List<Node> Children { get; private set; }
            /// <summary>
            /// Is this Element a Directory or a File?
            /// </summary>
            public NodeType Type;

            /// <summary>
            /// Was this Node asked to load its children before?
            /// </summary>
            public bool LoadedChildren;

            /// <summary>
            /// Path separation Symbol used
            /// </summary>
            private char _separationSymbol;
            /// <summary>
            /// If not all Files should be displayed, this is not null and contains a list of supported File Extensions
            /// </summary>
            private string[] _supportedExtensions;


            /// <summary>
            /// Creates a new Node without loading its children
            /// </summary>
            /// <param name="path">The Path URL to the Node</param>
            /// <param name="parent">Parent Node</param>
            /// <param name="separationSymbol">Path separation Symbol that should be used</param>
            /// <param name="supportedExtensions">A List of supported File Extensions. If all Files should be displayed, set this to null</param>
            public Node(string path, Node parent, char separationSymbol, string[] supportedExtensions)
            {
                PathURL = path;
                // get the display name by looking for the last occurence of the separation simbol that is not the last symbol in the string, then taking a substring from that location and removing all separation symbols in it
                DisplayName = path.Substring(path.LastIndexOf(separationSymbol, path.Length - 2) + 1).Replace(separationSymbol.ToString(), string.Empty);
                Extension = Path.GetExtension(PathURL);
                Parent = parent;

                FileAttributes att = File.GetAttributes(PathURL);
                Type = att.HasFlag(FileAttributes.Directory) ? Type = NodeType.DIRECTORY : Type = NodeType.FILE;

                _separationSymbol = separationSymbol;
                _supportedExtensions = supportedExtensions;
            }

            /// <summary>
            /// Loads the Children of this Node if this Node is a Directory and the User has Access Rights
            /// </summary>
            public void LoadChildren()
            {
                // save that we attemted to load children
                LoadedChildren = true;

                // if this not is a Directory the User has no Right to access or it is a File, we cannot load any children
                if ((Type == NodeType.DIRECTORY && !HasAccessRights(PathURL)) || Type == NodeType.FILE)
                    throw new System.UnauthorizedAccessException("Cannot Acces Node that is no Directory with Access permission");

                // create a new List of children
                Children = new List<Node>();

                // get all entries in the File System
                string[] entries = Directory.GetFileSystemEntries(PathURL);

                // create nodes for all entries that can get one
                foreach (string entry in entries)
                {
                    // First check whether the entry is a Directory the User has Access to, if yes, make it a node
                    FileAttributes att = File.GetAttributes(entry);
                    if (att.HasFlag(FileAttributes.Directory) && HasAccessRights(entry))
                    {
                        Children.Add(new Node(entry, this, _separationSymbol, _supportedExtensions));
                    }
                    else if (_supportedExtensions != null) // if the entry was not a directory and we want to see only some files, check teh extension and only make a Node out of the entry if it has one of the supported extensions
                    {
                        string extension = Path.GetExtension(entry);

                        if (_supportedExtensions.Search(extension) != -1)
                        {
                            Children.Add(new Node(entry, this, _separationSymbol, _supportedExtensions));
                        }
                    }
                    else if (_supportedExtensions == null && !att.HasFlag(FileAttributes.Directory)) // if the entry was not a directory but we want to see all files, make a Node out of the entry
                    {
                        Children.Add(new Node(entry, this, _separationSymbol, _supportedExtensions));
                    }
                }
            }
        }

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// The Root of the File System, we can't access anything beyond that
        /// </summary>
        private Node _root;
        /// <summary>
        /// The Node we are currently at
        /// </summary>
        private Node _current;

        /// <summary>
        /// Path separation Symbol used
        /// </summary>
        private char _separationSymbol;
        /// <summary>
        /// If not all Files should be displayed, this is not null and contains a list of supported File Extensions
        /// </summary>
        private string[] _supportedFileExtensions;

        /// <summary>
        /// The Paths of the Nodes we accessed in the Past
        /// </summary>
        private List<string> _history;



        // ######################## INITS ######################## //
        /// <summary>
        /// Creates a File System
        /// </summary>
        /// <param name="basePath">The base Path of the File system. You cannot go beyond this</param>
        /// <param name="separationSymbol">Path separation Symbol that should be used</param>
        /// <param name="supportedFileExtensions">A List of supported File Extensions. If all Files should be displayed leave this emtpy</param>
        public FileSystem(string basePath, char separationSymbol, string[] supportedFileExtensions = null)
        {
            _separationSymbol = separationSymbol;
            _supportedFileExtensions = supportedFileExtensions;

            // create the root node, laod it and set it as the current node
            _root = new Node(basePath, null, separationSymbol, supportedFileExtensions);
            _root.LoadChildren();
            _current = _root;

            _history = new List<string>();
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Enters the Directory at the given path if it exists and is a child of the provided base path and returns the contents of that Directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileSystemEntry[] ChangeDirectory(string path)
        {
            return ChangeDirectory(path, true);
        }

        /// <summary>
        /// Reloads all loaded Directories (A Directory is loaded if it was visited before)
        /// </summary>
        public void ReloadAllLoaded()
        {
            // save the path of the current node
            string path = _current.PathURL;

            // reload everything down from the root
            ReloadRecursive(_root);

            // set current to root because the old Current is not valid anymore
            _current = _root;

            // change back to the path we were in before
            ChangeDirectory(path, false);

            Debug.Log("Reloaded File System");
        }

        /// <summary>
        /// Reloads all Directories downward from the Current that where Loaded before
        /// </summary>
        public void ReloadCurrent()
        {
            ReloadRecursive(_current);
            Debug.Log("Reloaded current folder with Chidren");
        }

        /// <summary>
        /// Goes back one step in the history and returns all entries in that directory
        /// </summary>
        /// <returns></returns>
        public FileSystemEntry[] GoBackInHistory()
        {
            // if there is no history, return null
            if (_history.Count <= 0)
                return null;

            // change to the last entry in the history and remove the history entry
            ChangeDirectory(_history[_history.Count - 1], false);
            _history.RemoveAt(_history.Count - 1);

            return GetCurrentContent();
        }

        /// <summary>
        /// Goes up one step in the File System Hierarchy
        /// </summary>
        /// <returns></returns>
        public FileSystemEntry[] GoUpInHierarchy()
        {
            // if there is no parent, do nothing
            if (_current.Parent == null)
                return null;

            // go up
            _current = _current.Parent;
            return GetCurrentContent();
        }

        /// <summary>
        /// Returns all Contents of the Current Directory
        /// </summary>
        /// <returns></returns>
        public FileSystemEntry[] GetCurrentContent()
        {
            // if the Current node is no Directory, do nothing
            if (_current.Type != NodeType.DIRECTORY)
            {
                Debug.LogError($"Cannot get Content {_current.PathURL} because it is not a Directory");
                return null;
            }
            else if (_current.Children == null) // if the current node has no children, try to load them
            {
                // if there was no loading attempt yet, laod
                if (!_current.LoadedChildren)
                    _current.LoadChildren();

                // if there are still no children, return an emtpy list
                if(_current.Children == null)
                {
                    Debug.LogWarning($"{_current.PathURL} has no Content");
                    return new FileSystemEntry[0];
                }
            }

            // create the return array
            FileSystemEntry[] entries = new FileSystemEntry[_current.Children.Count];

            // fill return array
            for (int i = 0; i < _current.Children.Count; ++i)
            {
                Node child = _current.Children[i];
                entries[i] = new FileSystemEntry(child.DisplayName, child.PathURL, child.Extension, child.Type);
            }

            return entries;
        }

        /// <summary>
        /// Enters the Directory at the given path if it exists and is a child of the provided base path and returns the contents of that Directory
        /// </summary>
        /// <param name="path"></param>
        /// <param name="updateHistory">Should this change be tracked in the History?</param>
        /// <returns></returns>
        private FileSystemEntry[] ChangeDirectory(string path, bool updateHistory)
        {
            // if the Path is not below the root, return
            if (!path.StartsWith(_root.PathURL))
            {
                Debug.LogError($"Provided path is no child of provided scope {_root.PathURL}");
                return null;
            }

            // set the start node for the search. If the requested path is below the current node, search from the current node, else search from the root node
            Node start = path.StartsWith(_current.PathURL) ? _current : _root;

            // split the path into node names and remove emtpy strings
            string[] pathNodeNames = path.Split(_separationSymbol);
            List<string> cleanNodeNames = new List<string>();
            foreach (string s in path.Split(_separationSymbol))
            {
                if (!string.IsNullOrEmpty(s))
                {
                    cleanNodeNames.Add(s);
                }
            }
            pathNodeNames = cleanNodeNames.ToArray();

            // search the correct node
            Node found = FindNode(start, pathNodeNames, pathNodeNames.Search(start.DisplayName));

            // if a node was found, change to that node and return the contents
            if (found != null)
            {
                // update history if needed
                if (updateHistory)
                {
                    _history.Add(_current.PathURL);
                }

                // change to found node
                _current = found;

                // Load children if they were not loaded yet
                if (_current.Type == NodeType.DIRECTORY && !_current.LoadedChildren)
                {
                    _current.LoadChildren();
                }

                return GetCurrentContent();
            }

            // if we reached here, the path does not exist or is inaccessible to us
            Debug.Log($"No entry found at {path}");
            return null;
        }

        /// <summary>
        /// A Recursive Reloading Function. It goes to the lowest Loaded nodes and reloads from the lowest to the root
        /// </summary>
        /// <param name="current"></param>
        private void ReloadRecursive(Node current)
        {
            // if the current node has not loaded yet or us no directory, we reached an end Node
            if (!current.LoadedChildren || current.Type != NodeType.DIRECTORY)
                return;

            // go down the branches
            foreach(Node child in current.Children)
            {
                ReloadRecursive(child);
            }

            // reload node
            current.LoadChildren();
        }

        /// <summary>
        /// Traverses the Tree to Find the node at the end of the provided path
        /// </summary>
        /// <param name="current">The Node to look at</param>
        /// <param name="pathNodeNames">The Node names that are along the path, sorted from root to leaf</param>
        /// <param name="nodeNameIndex">Current Node Name index</param>
        /// <returns></returns>
        private Node FindNode(Node current, string[] pathNodeNames, int nodeNameIndex)
        {
            // if the current node is a directory but has no loaded childlren, try to load the chidren
            if (current.Type == NodeType.DIRECTORY &&  current.Children == null)
            {
                current.LoadChildren();
            }

            // if the display name of the current node is the current path node name, check wehther we reached the end
            if(current.DisplayName == pathNodeNames[nodeNameIndex])
            {
                // if we reached the end of the path, return the current node
                if (nodeNameIndex == pathNodeNames.Length - 1)
                {
                    return current;
                } else // if we did not reach the end of the path, stay at the same node to check children, but go to next path node name
                {
                    return FindNode(current, pathNodeNames, nodeNameIndex + 1);
                }
            }

            // look at the children
            foreach (Node child in current.Children)
            {
                // if the display name of the child node is the current path node name, check wehther we reached the end
                if (child.DisplayName == pathNodeNames[nodeNameIndex])
                {
                    // if we reached the end of the path, return the child node
                    if (nodeNameIndex == pathNodeNames.Length - 1)
                    {
                        return child;
                    }
                    // if we did not reach the end of the path, check the child node wih the next path name
                    return FindNode(child, pathNodeNames, nodeNameIndex + 1);
                }
            }

            // if we never found anything, return null
            return null;
        }

        // ######################## COROUTINES ######################## //


        // ######################## UTILITIES ######################## //
        /// <summary>
        /// Tries to load the contents of the path to find out wheter user has access rights
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool HasAccessRights(string path)
        {
            try
            {
                Directory.GetFileSystemEntries(path);
                return true;
            }
            catch (System.UnauthorizedAccessException)
            {
                return false;
            }
        }

    }
}
