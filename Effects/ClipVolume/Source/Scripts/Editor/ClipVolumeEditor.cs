using UnityEditor;
using UnityEngine;
using FK.Editor;

/// <summary>
/// <para>Custom Editor for Clip Volumes</para>
///
/// v1.2 09/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CustomEditor(typeof(ClipVolume))]
[CanEditMultipleObjects]
public class ClipVolumeEditor : Editor
{
    // ######################## UNITY EVENT FUNCTIONS ######################## //
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUI.BeginChangeCheck();
        MultiObjectEditorGUI.Vector3Field(serializedObject, "_size", "Size");
        if (EditorGUI.EndChangeCheck())
            UpdateVolumes();
        
        EditorGUI.BeginChangeCheck();
        SerializedProperty pivot = serializedObject.FindProperty("_pivot");
        EditorGUI.showMixedValue = pivot.hasMultipleDifferentValues;
        Vector3 p = EditorGUILayout.Vector3Field("Pivot", pivot.vector3Value);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            p.x = Mathf.Clamp01(p.x);
            p.y = Mathf.Clamp01(p.y);
            p.z = Mathf.Clamp01(p.z);
            pivot.vector3Value = p;
            UpdateVolumes();
            
        }

        if (GUILayout.Button(new GUIContent("Update",
            "Click when you added Objects to the Clip Volume or when the Changes you did to the Size did not affect rendering for some reason to Update manually (note that this is purely for convenience in the Editor, as soon as you enter playmode everything will update automatically)"))
        )
            UpdateVolumes();
        serializedObject.ApplyModifiedProperties();
    }

    // ######################## FUNCTIONALITY ######################## //
    private void UpdateVolumes()
    {
        foreach (Object t in targets)
        {
            ClipVolume v = t as ClipVolume;
            v.UpdateShaderValues();
        }
    }
}