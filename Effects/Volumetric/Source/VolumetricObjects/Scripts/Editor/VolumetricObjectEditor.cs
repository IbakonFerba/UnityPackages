using UnityEditor;
using UnityEngine;
using System.Collections;

/// <summary>
/// <para>Custom Editor for Volumetric Objects</para>
///
/// v2.0 08/2018
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
        bool enableNoise = EditorGUILayout.Toggle(new GUIContent("Enable Noise", ""), vo.EnableNoise);

        Matrix4x4 noiseSt = vo.NoiseST;
        if (enableNoise)
        {
            Vector3 noiseScale = EditorGUILayout.Vector3Field("Noise Scale", new Vector3(vo.NoiseST[0, 0], vo.NoiseST[0, 1], vo.NoiseST[0, 2]));
            Vector3 noiseTransform = EditorGUILayout.Vector3Field("Noise Transform", new Vector3(vo.NoiseST[1, 0], vo.NoiseST[1, 1], vo.NoiseST[1, 2]));
            float noiseStrenght = EditorGUILayout.Slider("Strength", vo.NoiseST[2, 1], 0, 1);


            noiseSt[0, 0] = noiseScale.x;
            noiseSt[0, 1] = noiseScale.y;
            noiseSt[0, 2] = noiseScale.z;
            noiseSt[1, 0] = noiseTransform.x;
            noiseSt[1, 1] = noiseTransform.y;
            noiseSt[1, 2] = noiseTransform.z;
            noiseSt[2, 0] = 1;
            noiseSt[2, 1] = noiseStrenght;
        }
        else
        {
            noiseSt[2, 0] = 0;
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
            vo.EnableNoise = enableNoise;
            vo.NoiseST = noiseSt;
            vo.Type = type;
            vo.Dimensions = dimensions;
            vo.Radius = radius;
            vo.Bounds = bounds;
        }
    }
}