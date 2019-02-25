using UnityEditor;
using UnityEngine;

namespace FK.JLoc
{
    /// <summary>
    /// <para>Custom Inspector for the StringsLoader</para>
    ///
    /// v1.0 02/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [CustomEditor(typeof(StringsLoader))]
    public class StringsLoaderInspector : UnityEditor.Editor
    {
        // ######################## PROPERTIES ######################## //
        private GUIStyle FileButtonStyle
        {
            get
            {
                _fileButtonStyle = null;
                if (_fileButtonStyle == null)
                {
                    _fileButtonStyle = new GUIStyle(GUI.skin.button);
                    _fileButtonStyle.alignment = TextAnchor.MiddleRight;
                }

                return _fileButtonStyle;
            }
        }

        // ######################## PRIVATE VARS ######################## //
        private GUIStyle _fileButtonStyle;


        // ######################## UNITY EVENT FUNCTIONS ######################## //
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            SerializedProperty loadOn = serializedObject.FindProperty("LoadOn");
            loadOn.enumValueIndex =
                (int) (StringsLoader.UnityEvents) EditorGUILayout.EnumPopup(new GUIContent("Load On:", "The Unity Event the file will be loaded at"),
                    (StringsLoader.UnityEvents) loadOn.enumValueIndex);
            EditorGUILayout.EndVertical();


            EditorGUILayout.Space();


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            SerializedProperty unloadOn = serializedObject.FindProperty("UnloadOn");
            unloadOn.enumValueIndex =
                (int) (StringsLoader.UnityEvents) EditorGUILayout.EnumPopup(new GUIContent("Unload On:", "The Unity Event the file will be unloaded at"),
                    (StringsLoader.UnityEvents) unloadOn.enumValueIndex);
            EditorGUILayout.EndVertical();


            EditorGUILayout.Space();


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            SerializedProperty loadAysnc = serializedObject.FindProperty("LoadAsync");
            loadAysnc.boolValue = EditorGUILayout.Toggle(new GUIContent("Load Asynchronous:", "If true, loading will happen asynchronously in a seperate thread"),
                loadAysnc.boolValue);

            SerializedProperty unloadOtherLoadedStrings = serializedObject.FindProperty("UnloadOtherLoadedStrings");
            unloadOtherLoadedStrings.boolValue = EditorGUILayout.Toggle(new GUIContent("Unload Other Strings on Load:", "If true, all other string files are unloaded once this one loads"),
                unloadOtherLoadedStrings.boolValue);
            EditorGUILayout.EndVertical();


            EditorGUILayout.Space();


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            SerializedProperty filePath = serializedObject.FindProperty("FilePath");
            EditorGUILayout.PrefixLabel("File:");
            if (GUILayout.Button(string.IsNullOrEmpty(filePath.stringValue) ? "Select a file" : filePath.stringValue, FileButtonStyle))
            {
                string path = EditorUtility.OpenFilePanel("Select a strings file", Application.streamingAssetsPath, "json");
                if (!string.IsNullOrEmpty(path))
                {
                    if (LanguageManagerEditor.CheckPath(path))
                    {
                        filePath.stringValue = path;
                    }
                }
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}