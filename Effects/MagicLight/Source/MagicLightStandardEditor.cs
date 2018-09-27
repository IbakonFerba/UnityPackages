using UnityEditor;
using FK.Editor;
using UnityEngine;

/// <summary>
/// <para>Material Editor for Standard Magic Light Shader</para>
///
/// v1.0 09/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class MagicLightStandardEditor : ShaderGUI
{
    // ######################## UNITY EVENT FUNCTIONS ######################## //
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        ShowGeneralProperties1(materialEditor);
        ShowSurfaceShaderProperties(materialEditor);
        ShowGeneralProperties2(materialEditor);
        ShowInsideColor(materialEditor);
        EditorGUILayout.Space();
        ShowGeneralProperties3(materialEditor, true);
        ShowAdvancedOptions(materialEditor);
    }

    // ######################## FUNCTIONALITY ######################## //
    protected void ShowAdvancedOptions(MaterialEditor me)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Advanced Options", EditorStyles.boldLabel);
        me.RenderQueueField();
        me.EnableInstancingField();
        me.DoubleSidedGIField();
    }
    protected void ShowGeneralProperties1(MaterialEditor me)
    {
        MultiMaterialEditorGUI.ColorField(me, "_Color", "Color");
        MultiMaterialEditorGUI.TextureField(me, "_MainTex", "Albedo", false);
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
        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);
    }

    protected void ShowInsideColor(MaterialEditor me)
    {
        MultiMaterialEditorGUI.ColorField(me, "_InsideColor", "Inside Color");
    }

    protected void ShowGeneralProperties3(MaterialEditor me, bool isSurfaceShader)
    {
        bool useMagicLight = MultiMaterialEditorGUI.ToggleLeft(me, "_UseMagicLight", "Use Magic Light");

        if (useMagicLight)
        {
            ++EditorGUI.indentLevel;
            MultiMaterialEditorGUI.ToggleLeft(me, "_Inverted", "Inverted");
            MultiMaterialEditorGUI.FloatField(me, "_RadiusScaleFactor", "Light Radius Scale Factor");
            EditorGUILayout.Space();
            MultiMaterialEditorGUI.TextureField(me, "_BorderTex", "Border Texture", true);
            EditorGUILayout.Space();
            MultiMaterialEditorGUI.FloatField(me, "_WobbleSpeed", "Animation Speed");
            MultiMaterialEditorGUI.FloatField(me, "_WobbleStrength", "Animation Strength");
            ShowBorderOptions(me, isSurfaceShader);
            --EditorGUI.indentLevel;
        }
    }

    protected void ShowBorderOptions(MaterialEditor me, bool isSurfaceShader)
    {
        // general border options
        EditorGUILayout.Space();
        MaterialProperty borderMode = MaterialEditor.GetMaterialProperty(me.targets, "_BorderMode");
        bool modeChanged = false;
        EditorGUI.showMixedValue = borderMode.hasMixedValue;
        EditorGUI.BeginChangeCheck();
        int sorting = EditorGUILayout.Popup("Sorting", (int) borderMode.vectorValue.w, new[] {"Soft Border on top", "Hard Border on top"});
        modeChanged = EditorGUI.EndChangeCheck();
        EditorGUI.showMixedValue = false;
        
        // soft border
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Soft Border", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = borderMode.hasMixedValue;
        int softBorderBlendMode = EditorGUILayout.Popup("Blend Mode", (int) borderMode.vectorValue.y, new[] {"Alpha Blended", "Additive"});
        modeChanged |= EditorGUI.EndChangeCheck();
        EditorGUI.showMixedValue = false;
        MultiMaterialEditorGUI.ColorField(me, "_BorderColor", "Color");
        MultiMaterialEditorGUI.FloatField(me, "_BorderThreshold", "Thickness");
        
        // hard border
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Hard Border", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = borderMode.hasMixedValue;
        bool hardBorderAsEmission = isSurfaceShader && EditorGUILayout.ToggleLeft("Hard Border As Emission", borderMode.vectorValue.x <= 0);
        int hardBorderBlendMode = (int) borderMode.vectorValue.z;
        if (!hardBorderAsEmission)
            hardBorderBlendMode = EditorGUILayout.Popup("Hard Border Blend Mode", (int) borderMode.vectorValue.z, new[] {"Alpha Blended", "Additive"});
        MultiMaterialEditorGUI.ColorField(me, "_BorderColorHard", "Color");
        MultiMaterialEditorGUI.FloatField(me, "_BorderThresholdHard", "Thickness");

        modeChanged |= EditorGUI.EndChangeCheck();
        EditorGUI.showMixedValue = false;

        // apply mode
        if (modeChanged)
            borderMode.vectorValue = new Vector4(hardBorderAsEmission ? 0 : 1, softBorderBlendMode, hardBorderBlendMode, sorting);
    }
}