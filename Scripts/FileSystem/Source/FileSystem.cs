using UnityEngine;
using System.Collections.Generic;
using System.IO;
using FK.Utility.ArraysAndLists;

namespace FK.IO
{
    /// <summary>
    /// Information of a File System entry
    /// </summary>
    public class FileSystemEntry
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
    /// 
    /// v1.1 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
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

                // get all entries. If the function returns an exeption or null, we cannot access the entries because either the directory does not exist or we don't have access rights
                string[] entries;
                try
                {
                    entries = TryGetEntries(PathURL);
                }
                catch (DirectoryNotFoundException)
                {
                    throw new DirectoryNotFoundException();
                }

                // if this not is a Directory the User has no Right to access (indicated by entries being null) or it is a File, we cannot load any children
                if ((Type == NodeType.DIRECTORY && entries == null) || Type == NodeType.FILE)
                    throw new System.UnauthorizedAccessException("Cannot Acces Node that is no Directory with Access permission");

                // create a new List of children
                Children = new List<Node>();

                // create nodes for all entries that can get one
                foreach (string entry in entries)
                {
                    // First check whether the entry is a Directory the User has Access to, if yes, make it a node
                    FileAttributes att = File.GetAttributes(entry);
                    if (att.HasFlag(FileAttributes.Directory) && TryGetEntries(entry) != null)
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

        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// The Current Node
        /// </summary>
        public FileSystemEntry Current
        {
            get { return new FileSystemEntry(_current.DisplayName, _current.PathURL, _current.Extension, _current.Type); }
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
        /// Does the FileSystem contain the given Path? The FileSystem goes down the path and loads everything along it until it either finds a dead end or the given entry
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Contains(string path)
        {
            return FindNode(_root, path) != null;
        }

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

            // set current to root because the old Current is not valid anymore
            _current = _root;

            // reload everything down from the root
            ReloadRecursive(_root);

            // change back to the path we were in before
            ChangeDirectory(path, false);

            Debug.Log($"Reloaded File System from Root {_root.PathURL}");
        }

        /// <summary>
        /// Reloads all Directories downward from the Current that where Loaded before
        /// </summary>
        public void ReloadCurrent()
        {
            ReloadRecursive(_current);
            Debug.Log($"Reloaded current folder {_current.PathURL} with Children");
        }

        /// <summary>
        /// Goes back one step in the history and returns all entries in that directory
        /// </summary>
        /// <param name="newPath">Takes the new Path we are at after going back</param>
        /// <returns></returns>
        public FileSystemEntry[] GoBackInHistory(out string newPath)
        {
            // if there is no history, return null
            if (_history.Count <= 0)
            {
                newPath = _current.PathURL;
                return null;
            }

            // change to the last entry in the history and remove the history entry
            ChangeDirectory(_history[_history.Count - 1], false);
            _history.RemoveAt(_history.Count - 1);

            newPath = _current.PathURL;

            return GetCurrentContent();
        }

        /// <summary>
        /// Goes up one step in the File System Hierarchy
        /// </summary>
        /// <param name="newPath">Takes the new Path we are at after going up</param>
        /// <returns></returns>
        public FileSystemEntry[] GoUpInHierarchy(out string newPath)
        {
            // if there is no parent, do nothing
            if (_current.Parent == null)
            {
                newPath = _current.PathURL;
                return null;
            }

            // go up
            _history.Add(_current.PathURL);
            _current = _current.Parent;

            newPath = _current.PathURL;

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
                if (_current.Children == null)
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
        /// Returns a specific entry of the Filesystem
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileSystemEntry GetEntry(string path)
        {
            // if the requested entry is the current node, return it
            if (path == _current.PathURL)
            {
                return Current;
            }

            // if the Path is not below the root, return nothing
            if (!path.StartsWith(_root.PathURL))
            {
                Debug.LogError($"Provided path is no child of provided scope {_root.PathURL}");
                return null;
            }

            // set the start node for the search. If the requested path is below the current node, search from the current node, else search from the root node
            Node start = _root;
            if (path.StartsWith(_current.PathURL)) // check whether the requested path starts with the path of the current node
            {
                // make sure that the path really is below the current node and not just a node on the same hierarchy level with a similar name 
                // by checking if the path is longer thn the current one and if after  the current path there comes a separation symbol
                if (path.Length > _current.PathURL.Length)
                {
                    if (path[_current.PathURL.Length] == _separationSymbol)
                    {
                        start = _current;
                    }
                }
            }

            // search the node
            Node found = FindNode(start, path);

            // if we found a node, return it, else return null
            if (found != null)
                return new FileSystemEntry(found.DisplayName, found.PathURL, found.Extension, found.Type);

            return null;
        }

        /// <summary>
        /// Enters the Directory at the given path if it exists and is a child of the provided base path and returns the contents of that Directory
        /// </summary>
        /// <param name="path"></param>
        /// <param name="updateHistory">Should this change be tracked in the History?</param>
        /// <returns></returns>
        private FileSystemEntry[] ChangeDirectory(string path, bool updateHistory)
        {
            // if we allready are in the requested directory, just return the contents
            if (path == _current.PathURL)
            {
                return GetCurrentContent();
            }

            // if the Path is not below the root, return nothing
            if (!path.StartsWith(_root.PathURL))
            {
                Debug.LogError($"Provided path is no child of provided scope {_root.PathURL}");
                return null;
            }

            // set the start node for the search. If the requested path is below the current node, search from the current node, else search from the root node
            Node start = _root;
            if (path.StartsWith(_current.PathURL)) // check whether the requested path starts with the path of the current node
            {
                // make sure that the path really is below the current node and not just a node on the same hierarchy level with a similar name 
                // by checking if the path is longer thn the current one and if after  the current path there comes a separation symbol
                if(path.Length > _current.PathURL.Length)
                {
                    if (path[_current.PathURL.Length] == _separationSymbol)
                    {
                        start = _current;
                    }
                }
            }

            // search the node
            Node found = FindNode(start, path);

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
        /// A Recursive Reloading Function. It goes to the lowest loaded nodes and reloads from the lowest to the root
        /// </summary>
        /// <param name="current"></param>
        private void ReloadRecursive(Node current)
        {
            // if the current node has not loaded yet or us no directory, we reached an end Node
            if (!current.LoadedChildren || current.Type != NodeType.DIRECTORY)
                return;

            // go down the branches
            for (int i = current.Children.Count - 1; i >= 0; --i)
            {
                ReloadRecursive(current.Children[i]);
            }

            try
            {
                // reload node
                current.LoadChildren();
            }
            catch (DirectoryNotFoundException)
            {
                current.Parent.Children.Remove(current);
            }
        }

        /// <summary>
        /// Traverses the tree recursively, loads nodes along the path and returns the node at the requested path if it exists
        /// </summary>
        /// <param name="current"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private Node FindNode(Node current, string path)
        {
            // if the current node is a directory but has no loaded children, try to load the children
            if (current.Type == NodeType.DIRECTORY && current.Children == null)
            {
                current.LoadChildren();
            }

            // if the path of thec current node is the requested path, return the node
            if (current.PathURL == path)
            {
                return current;
            }

            // Only go on if the node has children
            if (current.Children != null)
            {
                // check each child
                foreach (Node child in current.Children)
                {
                    // if the path starts with the path of the child node, this might be a candidat further down the path.
                    if (path.StartsWith(child.PathURL))
                    {
                        // if the path is longer than the child path, we have not reached the end yet
                        if (path.Length > child.PathURL.Length)
                        {
                            // if the char adter the child path is a separation symbol, the path continues below the child, so continue searching from the child
                            if (path[child.PathURL.Length] == _separationSymbol)
                            {
                                return FindNode(child, path);
                            }
                        }
                        else // if the path is not longer than the child path, the child is the node we are looking for, return it
                        {
                            return child;
                        }
                    }
                }
            }

            // if we reached this, we did not find anything
            return null;
        }


        // ######################## UTILITIES ######################## //
        /// <summary>
        /// Tries to load the contents of the path. If the path does not exist, this throws a DirectoryNotFoundException, if the user has no rights to access the path, entries will be null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] TryGetEntries(string path)
        {
            try
            {
                return Directory.GetFileSystemEntries(path);
            }
            catch (System.UnauthorizedAccessException)
            {
                return null;
            }
        }

    }
}
