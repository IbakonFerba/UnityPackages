using UnityEditor;
using FK.Editor;

/// <summary>
/// <para>Material Editor for Standard Clip Volume Shader</para>
///
/// v4.1 08/2018
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
    protected void ShowGeneralProperties1(MaterialEditor me)
    {
        MultiMaterialEditorGUI.ColorField(me, "_Color", "Color");
        MultiMaterialEditorGUI.TextureField(me, "_MainTex", "Albedo (RGB)", false);
    }

    protected void ShowSurfaceShaderProperties(MaterialEditor me)
    {
        MultiMaterialEditorGUI.TextureField(me, "_SmoothnessMap", "Smoothness (R), Metallic (G)", false);
        MultiMaterialEditorGUI.Slider(me, "_Glossiness", "Smoothness", 0, 1);
        MultiMaterialEditorGUI.Slider(me, "_Metallic", "Metallic", 0, 1);
        MultiMaterialEditorGUI.TextureField(me, "_BumpMap", "Normal Map (RGB)", false);
        MultiMaterialEditorGUI.TextureField(me, "_OcclusionMap", "Occlusion (R)", false);
        MultiMaterialEditorGUI.Slider(me, "_OcclusionStrength", "Occlusion Strength", 0, 1);
        MultiMaterialEditorGUI.ColorField(me, "_EmissionColor", "Emission Color", true, true, true);
        MultiMaterialEditorGUI.TextureField(me, "_EmissionMap", "Emission (RGB)", false);
    }

    protected void ShowGeneralProperties2(MaterialEditor me)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Map Tiling", EditorStyles.boldLabel);

        MultiMaterialEditorGUI.TextureScaleOffsetField(me, "_MainTex");

        EditorGUILayout.Space();
        EditorGUILayout.TextArea("Effects", EditorStyles.boldLabel);
    }

    protected void ShowInsideColor(MaterialEditor me)
    {
        MultiMaterialEditorGUI.ColorField(me, "_InsideColor", "Inside Color");
    }

    protected void ShowGeneralProperties3(MaterialEditor me)
    {
        MultiMaterialEditorGUI.ShaderPropertyField(me, "_FadeBorder", "Fade Border");
        
        MaterialProperty fadeBorder = MaterialEditor.GetMaterialProperty(me.targets, "_FadeBorder");

        if (fadeBorder.floatValue > 0)
        {
            EditorGUI.indentLevel++;

            MultiMaterialEditorGUI.FloatField(me, "_BorderGradientHardness", "Border Hardness");
            MultiMaterialEditorGUI.TextureField(me, "_NoiseMap", "Dither Pattern", false);
            MultiMaterialEditorGUI.FloatField(me, "_NoiseScale", "Dither Pattern Scale");
            MultiMaterialEditorGUI.Slider(me, "_NoiseThreshold", "Dither Pattern Threshold", 0, 1);

            EditorGUI.indentLevel--;
        }
    }
}