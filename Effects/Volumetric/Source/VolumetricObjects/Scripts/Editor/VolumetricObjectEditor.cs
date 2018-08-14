using UnityEditor;
using UnityEngine;

/// <summary>
/// <para>Custom Editor for Volumetric Objects</para>
///
/// v1.0 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CustomEditor(typeof(VolumetricObject))]
public class VolumetricObjectEditor : Editor
{
    // ######################## UNITY EVENT FUNCTIONS ######################## //
    public override void OnInspectorGUI()
    {
        VolumetricObject vo = (VolumetricObject) target;
        VolumetricObject.Types t = vo.Type;

        vo.Color = EditorGUILayout.ColorField("Color", vo.Color);
        vo.Density = EditorGUILayout.FloatField(new GUIContent("Density", "Determines how far you can look through the fog"), vo.Density);

        EditorGUILayout.Space();

        vo.Type = (VolumetricObject.Types) EditorGUILayout.EnumPopup("Type", t);

        EditorGUILayout.Space();
        switch (t)
        {
            case VolumetricObject.Types.BOX:
                vo.Dimensions = EditorGUILayout.Vector3Field(new GUIContent("Dimensions", "The X, Y and Z Dimensions of the Box"), vo.Dimensions);
                break;
            case VolumetricObject.Types.SPHERE:
                vo.Radius = EditorGUILayout.FloatField("Radius", vo.Radius);
                break;
            case VolumetricObject.Types.CAPSULE:
                Vector3 bounds1 = EditorGUILayout.Vector3Field(new GUIContent("Point 1", "The center of the first Capsule Sphere"), new Vector3(vo.Bounds[0, 0], vo.Bounds[0, 1], vo.Bounds[0, 2]));
                Vector3 bounds2 = EditorGUILayout.Vector3Field(new GUIContent("Point 2", "The center of the second Capsule Sphere"), new Vector3(vo.Bounds[1, 0], vo.Bounds[1, 1], vo.Bounds[1, 2]));

                vo.Bounds[0, 0] = bounds1.x;
                vo.Bounds[0, 1] = bounds1.y;
                vo.Bounds[0, 2] = bounds1.z;
                vo.Bounds[1, 0] = bounds2.x;
                vo.Bounds[1, 1] = bounds2.y;
                vo.Bounds[1, 2] = bounds2.z;

                vo.Radius = EditorGUILayout.FloatField("Radius", vo.Radius);
                break;
        }
    }
}
