using UnityEditor;
using UnityEngine;

namespace FK.JLoc
{
    /// <summary>
    /// <para>Custom Inspector for Language Text Config</para>
    ///
    /// v1.0 02/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LanguageTextConfig))]
    public class LanguageTextConfigInspector : UnityEditor.Editor
    {
        // ######################## UNITY EVENT FUNCTIONS ######################## //
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            SerializedProperty category = serializedObject.FindProperty("Category");
            EditorGUI.showMixedValue = category.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            string cat = EditorGUILayout.TextField(new GUIContent("Category:", "The category in the string files where the string of this text resides"), category.stringValue);
            if (EditorGUI.EndChangeCheck())
                category.stringValue = cat;

            SerializedProperty field = serializedObject.FindProperty("Field");
            EditorGUI.showMixedValue = field.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            string fld = EditorGUILayout.TextField(new GUIContent("Field Name:", "The name of the field inside the given category in the string files where the string of this text resides"),
                field.stringValue);
            if (EditorGUI.EndChangeCheck())
                field.stringValue = fld;
            EditorGUILayout.EndVertical();


            EditorGUILayout.Space();


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            SerializedProperty updateOnFileLoad = serializedObject.FindProperty("UpdateOnFileLoad");
            EditorGUI.showMixedValue = updateOnFileLoad.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            bool update = EditorGUILayout.Toggle(new GUIContent("Update on File Load:", "If true, this text will attempt to update every time a string file is loaded"),
                updateOnFileLoad.boolValue);
            if (EditorGUI.EndChangeCheck())
                updateOnFileLoad.boolValue = update;
            EditorGUILayout.EndVertical();

            EditorGUI.showMixedValue = false;

            serializedObject.ApplyModifiedProperties();
        }
    }
}