using UnityEditor;
using UnityEngine;
using FK.Editor;

/// <summary>
/// <para>Custom Editor for Clip Volumes</para>
///
/// v1.0 08/2018
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

        if (GUILayout.Button(new GUIContent("Update",
            "Click if the Changes you did to the Size did not affect rendering for some reason to Update manually (note that this is purely for convenience in the Editor, as soon as you enter playmode everything will update automatically)"))
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