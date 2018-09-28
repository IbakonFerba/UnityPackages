using UnityEditor;
using FK.Editor;
using UnityEngine;

/// <summary>
/// <para>Material Editor for Standard Magic Light Shader</para>
///
/// v2.0 09/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class MagicLightStandardEditor : ShaderGUI
{
    // ######################## UNITY EVENT FUNCTIONS ######################## //
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        ShowColAnMainTex(materialEditor);
        ShowSecondaryColor(materialEditor);
        ShowSurfaceShaderProperties(materialEditor);
        ShowTilingAndEffectsHeadline(materialEditor);
        ShowInsideColor(materialEditor);
        EditorGUILayout.Space();
        ShowMagicLightParams(materialEditor, true);
        ShowAdvancedOptions(materialEditor);
    }

    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Displays fields for main color and main texture
    /// </summary>
    /// <param name="me"></param>
    protected void ShowColAnMainTex(MaterialEditor me)
    {
        MultiMaterialEditorGUI.ColorField(me, "_Color", "Color");
        MultiMaterialEditorGUI.TextureField(me, "_MainTex", "Albedo (RGBA)", false);
    }

    /// <summary>
    /// Displays fields for secondary color
    /// </summary>
    /// <param name="me"></param>
    protected void ShowSecondaryColor(MaterialEditor me)
    {
        EditorGUILayout.Space();
        MultiMaterialEditorGUI.ColorField(me, "_Color2", "Secondary Color");
        MultiMaterialEditorGUI.Slider(me, "_Color2Strength", "Secondary Color Strength", 0.0f, 1.0f);
        EditorGUILayout.Space();
    }

    /// <summary>
    /// Displays fields for surface shader values
    /// </summary>
    /// <param name="me"></param>
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

    /// <summary>
    /// Displays Main Map tiling and the headline for the effects
    /// </summary>
    /// <param name="me"></param>
    protected void ShowTilingAndEffectsHeadline(MaterialEditor me)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Map Tiling", EditorStyles.boldLabel);

        MultiMaterialEditorGUI.TextureScaleOffsetField(me, "_MainTex");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);
    }

    /// <summary>
    /// Displays field for inside color
    /// </summary>
    /// <param name="me"></param>
    protected void ShowInsideColor(MaterialEditor me)
    {
        MultiMaterialEditorGUI.ColorField(me, "_InsideColor", "Inside Color");
    }

    /// <summary>
    /// Displays fields fpr all magic light parameters
    /// </summary>
    /// <param name="me"></param>
    /// <param name="isSurfaceShader"></param>
    protected void ShowMagicLightParams(MaterialEditor me, bool isSurfaceShader)
    {
        // Toggle to use magic light
        MultiMaterialEditorGUI.ShaderPropertyField(me, "_UseMagicLight", "Use Magic Light");
        MaterialProperty useMagicLight = MaterialEditor.GetMaterialProperty(me.targets, "_UseMagicLight");

        // only show the rest if the magic light is used
        if (useMagicLight.floatValue > 0)
        {
            ++EditorGUI.indentLevel;
            bool useStencil = MultiMaterialEditorGUI.ToggleLeft(me, "_UseStencil", "Use Stencil");

            // display stencil map if stencil is used
            if (useStencil)
            {
                ++EditorGUI.indentLevel;
                MultiMaterialEditorGUI.TextureField(me, "_StencilMap", new GUIContent("Stencil Map (RGB)", "R -> Cutout, G -> Border 1, B -> Border 2"), true);
                --EditorGUI.indentLevel;
            }
            else // display scale factor and invert option if no stencil is used
            {
                MultiMaterialEditorGUI.ToggleLeft(me, "_Inverted", "Inverted");
                MultiMaterialEditorGUI.FloatField(me, "_RadiusScaleFactor", "Light Radius Scale Factor");
            }

            ShowAnimationParams(me);
            ShowBorderOptions(me, useStencil, isSurfaceShader);

            --EditorGUI.indentLevel;
        }
    }

    /// <summary>
    /// Shows Queue, instancing and doublesided GI
    /// </summary>
    /// <param name="me"></param>
    protected void ShowAdvancedOptions(MaterialEditor me)
    {
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Advanced Options", EditorStyles.boldLabel);
        me.RenderQueueField();
        me.EnableInstancingField();
        me.DoubleSidedGIField();
    }


    /// <summary>
    /// Displays fields for animating the cutout
    /// </summary>
    /// <param name="me"></param>
    private void ShowAnimationParams(MaterialEditor me)
    {
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);

        MaterialProperty animationParams = MaterialEditor.GetMaterialProperty(me.targets, "_WobbleParams");

        // display speed and strength
        EditorGUI.showMixedValue = animationParams.hasMixedValue;
        EditorGUI.BeginChangeCheck();
        float animSpeed = EditorGUILayout.FloatField("Animation Speed", animationParams.vectorValue.x);
        float animStrength = EditorGUILayout.FloatField("Animation Strength", animationParams.vectorValue.y);
        if (EditorGUI.EndChangeCheck())
            animationParams.vectorValue = new Vector2(animSpeed, animStrength);
        EditorGUI.showMixedValue = false;
    }

    /// <summary>
    /// Shows options for rendering a border around the cutout
    /// </summary>
    /// <param name="me"></param>
    /// <param name="useStencil"></param>
    /// <param name="isSurfaceShader"></param>
    private void ShowBorderOptions(MaterialEditor me, bool useStencil, bool isSurfaceShader)
    {
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Borders", EditorStyles.boldLabel);

        MultiMaterialEditorGUI.TextureField(me, "_BorderTex", "Border Texture", true);

        // get properties for the options that are combined values
        MaterialProperty borderMode = MaterialEditor.GetMaterialProperty(me.targets, "_BorderMode");
        MaterialProperty useBorderTexture = MaterialEditor.GetMaterialProperty(me.targets, "_UseBorderTexture");
        MaterialProperty borderThresholds = MaterialEditor.GetMaterialProperty(me.targets, "_BorderThresholds");

        bool modeChanged = false;
        bool useTextureChanged = false;
        bool thresholdsChanged = false;

        // sorting
        EditorGUI.showMixedValue = borderMode.hasMixedValue;
        EditorGUI.BeginChangeCheck();
        int sorting = EditorGUILayout.Popup("Sorting", (int) borderMode.vectorValue.w, new[] {"Border 1 on top", "Border 2 on top"});
        modeChanged = EditorGUI.EndChangeCheck();
        EditorGUI.showMixedValue = false;

        // border1
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Border 1", EditorStyles.boldLabel);

        // blend mode
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = borderMode.hasMixedValue;
        int softBorderBlendMode = EditorGUILayout.Popup("Blend Mode", (int) borderMode.vectorValue.y, new[] {"Alpha Blended", "Additive"});
        modeChanged |= EditorGUI.EndChangeCheck();
        EditorGUI.showMixedValue = false;

        // use texture
        EditorGUI.showMixedValue = useBorderTexture.hasMixedValue;
        EditorGUI.BeginChangeCheck();
        bool useTexture1 = EditorGUILayout.ToggleLeft("Use Texture", useBorderTexture.vectorValue.x > 0);
        useTextureChanged = EditorGUI.EndChangeCheck();
        EditorGUI.showMixedValue = false;

        MultiMaterialEditorGUI.ColorField(me, "_Border1Color", "Color");

        // thickness
        float border1Threshold = borderThresholds.vectorValue.x;
        if (!useStencil)
        {
            EditorGUI.showMixedValue = borderThresholds.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            border1Threshold = EditorGUILayout.FloatField("Thickness", borderThresholds.vectorValue.x);
            thresholdsChanged = EditorGUI.EndChangeCheck();
            EditorGUI.showMixedValue = false;
        }

        // border 2
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Border 2", EditorStyles.boldLabel);

        // use with emission
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = borderMode.hasMixedValue;
        bool hardBorderAsEmission = isSurfaceShader && EditorGUILayout.ToggleLeft("Hard Border As Emission", borderMode.vectorValue.x <= 0);

        // blend mode
        int hardBorderBlendMode = (int) borderMode.vectorValue.z;
        if (!hardBorderAsEmission)
            hardBorderBlendMode = EditorGUILayout.Popup("Hard Border Blend Mode", (int) borderMode.vectorValue.z, new[] {"Alpha Blended", "Additive"});
        modeChanged |= EditorGUI.EndChangeCheck();
        EditorGUI.showMixedValue = false;

        // use texture
        bool useTexture2 = useBorderTexture.vectorValue.y > 0;
        if (!hardBorderAsEmission)
        {
            EditorGUI.showMixedValue = useBorderTexture.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            useTexture2 = EditorGUILayout.ToggleLeft("Use Texture", useBorderTexture.vectorValue.y > 0);
            useTextureChanged |= EditorGUI.EndChangeCheck();
            EditorGUI.showMixedValue = false;
        }

        MultiMaterialEditorGUI.ColorField(me, "_Border2Color", "Color");

        // thickness
        float border2Threshold = borderThresholds.vectorValue.y;
        if (!useStencil)
        {
            EditorGUI.showMixedValue = borderThresholds.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            border2Threshold = EditorGUILayout.FloatField("Thickness", borderThresholds.vectorValue.y);
            thresholdsChanged |= EditorGUI.EndChangeCheck();
            EditorGUI.showMixedValue = false;
        }


        // apply values
        if (modeChanged)
            borderMode.vectorValue = new Vector4(hardBorderAsEmission ? 0 : 1, softBorderBlendMode, hardBorderBlendMode, sorting);

        if (useTextureChanged)
            useBorderTexture.vectorValue = new Vector2(useTexture1 ? 1 : 0, useTexture2 ? 1 : 0);

        if (thresholdsChanged)
            borderThresholds.vectorValue = new Vector2(border1Threshold, border2Threshold);
    }
}