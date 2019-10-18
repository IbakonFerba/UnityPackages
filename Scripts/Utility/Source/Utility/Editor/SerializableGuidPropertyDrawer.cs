using UnityEditor;
using UnityEngine;

namespace FK.Utility.Editor
{
    /// <summary>
    /// <para>A custom property drawer for the serializable guid. This uses IMGUI because the default inspector does not yet support UIElements</para>
    ///
    /// v1.1 10/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableGuid))]
    public class SerializableGuidPropertyDrawer : PropertyDrawer
    {
        // ######################## UNITY EVENT FUNCTIONS ######################## //
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Draw fields
            EditorGUI.SelectableLabel(position, property.FindPropertyRelative("_serializedGuid").stringValue);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}