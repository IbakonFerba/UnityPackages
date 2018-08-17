using UnityEditor;
using UnityEngine;

/// <summary>
/// <para>Material Editor for Standard Clip Volume Shader</para>
///
/// v3.0 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class ClipVolumeStandardEditor : ShaderGUI
{
    // ######################## UNITY EVENT FUNCTIONS ######################## //
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        ShowGeneralProperties1(materialEditor);
        ShowSurfaceShaderProperties(materialEditor);
        ShowGeneralProperties2(materialEditor);
        ShowInsideColor(materialEditor);
        ShowGeneralProperties3(materialEditor);
    }

    // ######################## FUNCTIONALITY ######################## //
    protected void ShowGeneralProperties1(MaterialEditor materialEditor)
    {
        EditorGUI.BeginChangeCheck();
        MaterialProperty color = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_Color");
        EditorGUI.showMixedValue = color.hasMixedValue;
        Color col = EditorGUILayout.ColorField("Color", color.colorValue);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            color.colorValue = col;
        }

        EditorGUI.BeginChangeCheck();
        MaterialProperty mainTex = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_MainTex");
        EditorGUI.showMixedValue = mainTex.hasMixedValue;
        Texture mt = (Texture) EditorGUILayout.ObjectField("Albedo (RGB)", mainTex.textureValue, typeof(Texture), false);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            mainTex.textureValue= mt;
        }
    }

    protected void ShowSurfaceShaderProperties(MaterialEditor materialEditor)
    {
        EditorGUI.BeginChangeCheck();
        MaterialProperty smoothnessMap = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_SmoothnessMap");
        EditorGUI.showMixedValue = smoothnessMap.hasMixedValue;
        Texture sm = (Texture) EditorGUILayout.ObjectField("Smoothness (R), Metallic (G)", smoothnessMap.textureValue, typeof(Texture), false);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            smoothnessMap.textureValue = sm;
        }

        EditorGUI.BeginChangeCheck();
        MaterialProperty glossiness = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_Glossiness");
        EditorGUI.showMixedValue = glossiness.hasMixedValue;
        float g = EditorGUILayout.Slider("Smoothness", glossiness.floatValue, 0, 1);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            glossiness.floatValue = g;
        }

        EditorGUI.BeginChangeCheck();
        MaterialProperty metallic = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_Metallic");
        EditorGUI.showMixedValue = metallic.hasMixedValue;
        float m = EditorGUILayout.Slider("Metallic", metallic.floatValue, 0, 1);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            metallic.floatValue = m;
        }

        EditorGUI.BeginChangeCheck();
        MaterialProperty bumpMap = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_BumpMap");
        EditorGUI.showMixedValue = bumpMap.hasMixedValue;
        Texture bm = (Texture) EditorGUILayout.ObjectField("Normal Map (RGB)", bumpMap.textureValue, typeof(Texture), false);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            bumpMap.textureValue = bm;
        }

        EditorGUI.BeginChangeCheck();
        MaterialProperty occlusionMap = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_OcclusionMap");
        EditorGUI.showMixedValue = occlusionMap.hasMixedValue;
        Texture om = (Texture) EditorGUILayout.ObjectField("Occlusion (R)", occlusionMap.textureValue, typeof(Texture), false);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            occlusionMap.textureValue = om;
        }

        EditorGUI.BeginChangeCheck();
        MaterialProperty occlusionStrenght = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_OcclusionStrength");
        EditorGUI.showMixedValue = occlusionStrenght.hasMixedValue;
        float os = EditorGUILayout.Slider("Occlusion Strength", occlusionStrenght.floatValue, 0, 1);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            occlusionStrenght.floatValue = os;
        }

        EditorGUI.BeginChangeCheck();
        MaterialProperty emissionColor = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_EmissionColor");
        EditorGUI.showMixedValue = emissionColor.hasMixedValue;
        Color ec = EditorGUILayout.ColorField(new GUIContent("Emission Color"), emissionColor.colorValue, true, true, true);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            emissionColor.colorValue = ec;
        }

        EditorGUI.BeginChangeCheck();
        MaterialProperty emissionMap = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_EmissionMap");
        EditorGUI.showMixedValue = emissionMap.hasMixedValue;
        Texture em = (Texture) EditorGUILayout.ObjectField("Emission (RGB)", emissionMap.textureValue, typeof(Texture), false);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            emissionMap.textureValue = em;
        }
    }

    protected void ShowGeneralProperties2(MaterialEditor materialEditor)
    {
        EditorGUILayout.Space();
        EditorGUILayout.TextArea("Map Tiling", EditorStyles.boldLabel);


        EditorGUI.BeginChangeCheck();
        MaterialProperty mainTex = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_MainTex");
        EditorGUI.showMixedValue = mainTex.hasMixedValue;
        Vector2 textureTiling = EditorGUILayout.Vector2Field("Tiling", new Vector2(mainTex.textureScaleAndOffset.x, mainTex.textureScaleAndOffset.y));
        Vector2 textureOffset = EditorGUILayout.Vector2Field("Offset", new Vector2(mainTex.textureScaleAndOffset.z, mainTex.textureScaleAndOffset.w));
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            mainTex.textureScaleAndOffset = new Vector4(textureTiling.x, textureTiling.y, textureOffset.x, textureOffset.y);;
        }


        EditorGUILayout.Space();
        EditorGUILayout.TextArea("Effects", EditorStyles.boldLabel);
    }

    protected void ShowInsideColor(MaterialEditor materialEditor)
    {
        EditorGUI.BeginChangeCheck();
        MaterialProperty insideColor = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_InsideColor");
        EditorGUI.showMixedValue = insideColor.hasMixedValue;
        Color ic = EditorGUILayout.ColorField("Inside Color", insideColor.colorValue);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            insideColor.colorValue = ic;
        }
    }

    protected void ShowGeneralProperties3(MaterialEditor materialEditor)
    {
        MaterialProperty fadeBorder = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_FadeBorder");
        EditorGUI.showMixedValue = fadeBorder.hasMixedValue;
        materialEditor.ShaderProperty(fadeBorder, "Fade Border");
        EditorGUI.showMixedValue = false;

        if (fadeBorder.floatValue > 0)
        {
            EditorGUI.indentLevel++;


            EditorGUI.BeginChangeCheck();
            MaterialProperty borderHardness = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_BorderGradientHardness");
            EditorGUI.showMixedValue = borderHardness.hasMixedValue;
            float bh = EditorGUILayout.FloatField("Border Hardness", borderHardness.floatValue);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                borderHardness.floatValue = bh;
            }

            EditorGUI.BeginChangeCheck();
            MaterialProperty noiseMap = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_NoiseMap");
            EditorGUI.showMixedValue = noiseMap.hasMixedValue;
            Texture nm = (Texture) EditorGUILayout.ObjectField("Dither Pattern", noiseMap.textureValue, typeof(Texture), false);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                noiseMap.textureValue = nm;
            }

            EditorGUI.BeginChangeCheck();
            MaterialProperty noiseScale = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_NoiseScale");
            EditorGUI.showMixedValue = noiseScale.hasMixedValue;
            float ns = EditorGUILayout.FloatField("Dither Pattern Scale", noiseScale.floatValue);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                noiseScale.floatValue = ns;
            }

            EditorGUI.BeginChangeCheck();
            MaterialProperty noiseThreshold = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_NoiseThreshold");
            EditorGUI.showMixedValue = noiseThreshold.hasMixedValue;
            float nt = EditorGUILayout.Slider("Dither Pattern Threshold", noiseThreshold.floatValue, 0, 1);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                noiseThreshold.floatValue = nt;
            }


            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.TextArea("Clip Volume Bounds", EditorStyles.boldLabel);


        EditorGUI.BeginChangeCheck();
        MaterialProperty clipVolumeMin = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_ClipVolumeMin");
        EditorGUI.showMixedValue = clipVolumeMin.hasMixedValue;
        Vector3 cm = EditorGUILayout.Vector3Field("Min", clipVolumeMin.vectorValue);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            clipVolumeMin.vectorValue = cm;
        }

        EditorGUI.BeginChangeCheck();
        MaterialProperty clipVolumeMax = MaterialEditor.GetMaterialProperty(materialEditor.targets, "_ClipVolumeMax");
        EditorGUI.showMixedValue = clipVolumeMax.hasMixedValue;
        Vector3 cmx = EditorGUILayout.Vector3Field("Max", clipVolumeMax.vectorValue);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            clipVolumeMax.vectorValue = cmx;
        }
    }
}