using UnityEditor;
using UnityEngine;
using System.Collections;

/// <summary>
/// <para>Custom Editor for Volumetric Objects</para>
///
/// v2.1 08/2018
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

        EditorGUI.BeginChangeCheck();
        Color color = EditorGUILayout.ColorField("Color", vo.Color);
        float density = EditorGUILayout.FloatField(new GUIContent("Density", "Determines how far you can look through the fog"), vo.Density);


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Noise", EditorStyles.boldLabel);
        bool enableNoise = EditorGUILayout.Toggle(new GUIContent("Enable Noise", "If true, a simplex noise is added to the volume"), vo.EnableNoise);

        Matrix4x4 noiseSto = vo.NoiseSTO;
        if (enableNoise)
        {
            EditorGUI.indentLevel++;
            bool globalNoise = EditorGUILayout.Toggle(new GUIContent("Global", "If true, the noise is calculated in world space"), vo.GlobalNoise);
            Vector3 noiseScale = EditorGUILayout.Vector3Field("Noise Scale", new Vector3(vo.NoiseSTO[0, 0], vo.NoiseSTO[0, 1], vo.NoiseSTO[0, 2]));
            Vector3 noiseTransform = EditorGUILayout.Vector3Field("Noise Transform", new Vector3(vo.NoiseSTO[1, 0], vo.NoiseSTO[1, 1], vo.NoiseSTO[1, 2]));
            float noiseStrenght = EditorGUILayout.Slider("Strength", vo.NoiseSTO[2, 1], 0, 1);
            EditorGUI.indentLevel--;

            noiseSto[0, 0] = noiseScale.x;
            noiseSto[0, 1] = noiseScale.y;
            noiseSto[0, 2] = noiseScale.z;
            noiseSto[1, 0] = noiseTransform.x;
            noiseSto[1, 1] = noiseTransform.y;
            noiseSto[1, 2] = noiseTransform.z;
            noiseSto[2, 0] = 1;
            noiseSto[2, 1] = noiseStrenght;
            noiseSto[2, 2] = globalNoise ? 1 : 0;
        }
        else
        {
            noiseSto[2, 0] = 0;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Object", EditorStyles.boldLabel);

        VolumetricObject.Types type = (VolumetricObject.Types) EditorGUILayout.EnumPopup("Type", t);

        EditorGUILayout.Space();
        Vector3 dimensions = vo.Dimensions;
        float radius = vo.Radius;
        Matrix4x4 bounds = vo.Bounds;
        switch (t)
        {
            case VolumetricObject.Types.BOX:
                dimensions = EditorGUILayout.Vector3Field(new GUIContent("Dimensions", "The X, Y and Z Dimensions of the Box"), vo.Dimensions);
                break;
            case VolumetricObject.Types.SPHERE:
                radius = EditorGUILayout.FloatField("Radius", vo.Radius);
                break;
            case VolumetricObject.Types.CAPSULE:
                Vector3 bounds1 = EditorGUILayout.Vector3Field(new GUIContent("Point 1", "The center of the first Capsule Sphere"), new Vector3(vo.Bounds[0, 0], vo.Bounds[0, 1], vo.Bounds[0, 2]));
                Vector3 bounds2 = EditorGUILayout.Vector3Field(new GUIContent("Point 2", "The center of the second Capsule Sphere"), new Vector3(vo.Bounds[1, 0], vo.Bounds[1, 1], vo.Bounds[1, 2]));

                bounds[0, 0] = bounds1.x;
                bounds[0, 1] = bounds1.y;
                bounds[0, 2] = bounds1.z;
                bounds[1, 0] = bounds2.x;
                bounds[1, 1] = bounds2.y;
                bounds[1, 2] = bounds2.z;

                radius = EditorGUILayout.FloatField("Radius", vo.Radius);
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(vo, $"Changed {vo.name}");

            vo.Color = color;
            vo.Density = density;
            vo.NoiseSTO = noiseSto;
            vo.Type = type;
            vo.Dimensions = dimensions;
            vo.Radius = radius;
            vo.Bounds = bounds;
        }
    }
}