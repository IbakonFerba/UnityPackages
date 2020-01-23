using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FK.QuantumVR.Editor
{
    /// <summary>
    /// <para>Custom Inspector for package definitions</para>
    ///
    /// v1.0 01/2020
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [CustomEditor(typeof(PackageDefinition))]
    public class PackageDefinitionInspector : UnityEditor.Editor
    {
        // ######################## PRIVATE VARS ######################## //
        private VisualTreeAsset _assetPathElementAsset;

        private VisualElement _assetListContainer;


        // ######################## UNITY EVENT FUNCTIONS ######################## //

        #region UNITY EVENT FUNCTIONS

        public override VisualElement CreateInspectorGUI()
        {
            // load uxml
            VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("PackageDefinitionInspector_layout");
            _assetPathElementAsset = Resources.Load<VisualTreeAsset>("PackageDefinitionInspector_assetPathElement");

            TemplateContainer visualTree = visualTreeAsset.CloneTree();

            // load styles
            visualTree.styleSheets.Add(Resources.Load<StyleSheet>("Styles"));
            visualTree.styleSheets.Add(Resources.Load<StyleSheet>("PackageDefinitionInspector_styles"));

            // get list container
            _assetListContainer = visualTree.Q("pathListAssetContainer");

            // get and set up target path button
            Button targetPathButton = visualTree.Q<Button>("targetPathButton");
            SerializedProperty targetPathProperty = serializedObject.FindProperty("_targetPath");
            targetPathButton.text = targetPathProperty.stringValue;
            targetPathButton.clickable.clicked += () =>
            {
                serializedObject.Update();
                targetPathProperty = serializedObject.FindProperty("_targetPath");
                targetPathProperty.stringValue = EditorUtility.OpenFolderPanel("Select a location", Application.dataPath, "");
                targetPathButton.text = targetPathProperty.stringValue;

                serializedObject.ApplyModifiedProperties();
            };

            // get and set up add asset button
            Button addAssetButton = visualTree.Q<Button>("addAssetButton");
            addAssetButton.clickable.clicked += () => AddAsset(false);

            // get and set up add folder button
            Button addFolderButton = visualTree.Q<Button>("addFolderButton");
            addFolderButton.clickable.clicked += () => AddAsset(true);

            // get and set up export button
            Button exportButton = visualTree.Q<Button>("exportButton");
            exportButton.clickable.clicked += (target as PackageDefinition).Export;

            visualTree.Bind(serializedObject);

            LoadAssets();
            return visualTree;
        }

        #endregion

        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        /// <summary>
        /// Adds a new asset or folder to the list
        /// </summary>
        /// <param name="folder"></param>
        private void AddAsset(bool folder)
        {
            // if the specified path is invalid, return
            string path = ValidatePath(
                folder ? EditorUtility.OpenFolderPanel("Select a location", Application.dataPath, "") : EditorUtility.OpenFilePanel("Select a location", Application.dataPath, ""), "");
            if (path == string.Empty)
                return;

            // add a new element
            serializedObject.Update();
            SerializedProperty pathsProperty = serializedObject.FindProperty("_paths");
            ++pathsProperty.arraySize;

            SerializedProperty pathProperty = pathsProperty.GetArrayElementAtIndex(pathsProperty.arraySize - 1);
            pathProperty.stringValue = path;
            serializedObject.ApplyModifiedProperties();

            TemplateContainer assetElement = AddAssetElement();
            TextElement assetPathDisplay = assetElement.Q<TextElement>("assetPathDisplay");
            assetPathDisplay.text = path;
        }

        /// <summary>
        /// Creates the visual element of an asset
        /// </summary>
        /// <returns></returns>
        private TemplateContainer AddAssetElement()
        {
            TemplateContainer assetElement = _assetPathElementAsset.CloneTree();
            _assetListContainer.Add(assetElement);

            Button deleteAssetButton = assetElement.Q<Button>("deleteAssetButton");
            deleteAssetButton.clickable.clicked += () =>
            {
                serializedObject.Update();
                SerializedProperty pathsProperty = serializedObject.FindProperty("_paths");
                pathsProperty.DeleteArrayElementAtIndex(_assetListContainer.IndexOf(assetElement));

                serializedObject.ApplyModifiedProperties();
                assetElement.RemoveFromHierarchy();
            };
            return assetElement;
        }

        /// <summary>
        /// Loads the visual elements for the asset list
        /// </summary>
        private void LoadAssets()
        {
            SerializedProperty pathsProperty = serializedObject.FindProperty("_paths");
            for (int i = 0; i < pathsProperty.arraySize; ++i)
            {
                TemplateContainer assetElement = AddAssetElement();
                TextElement assetPathDisplay = assetElement.Q<TextElement>("assetPathDisplay");
                assetPathDisplay.text = pathsProperty.GetArrayElementAtIndex(i).stringValue;
            }
        }
        
        /// <summary>
        /// makes sure a path is inside the project
        /// </summary>
        /// <param name="path"></param>
        /// <param name="oldValidPath"></param>
        /// <returns></returns>
        private string ValidatePath(string path, string oldValidPath)
        {
            if (!path.StartsWith(Application.dataPath))
            {
                Debug.LogError($"The path {path} does not lead to a directory relative to this Project!");
                return oldValidPath;
            }

            return $"Assets{path.Replace(Application.dataPath, "")}";
        }

        #endregion
    }
}