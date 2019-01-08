using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// <para>Settings class for the Post Processing Effect</para>
/// 
/// v1.0 01/2019
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[System.Serializable]
[PostProcess(typeof(GlitchEffectRenderer), PostProcessEvent.AfterStack, "Custom/Glitch")]
public sealed class GlitchEffect : PostProcessEffectSettings
{
    public FloatParameter Strips = new FloatParameter {value = 10};
    public FloatParameter Strength = new FloatParameter {value = 0.38f};
    public FloatParameter StrengthRandomness = new FloatParameter {value = 1};
    [Range(0,1)]
    public FloatParameter Amount = new FloatParameter {value = 0.013f};
}

/// <summary>
/// <para>Renderer for the Retrorize Post Processing Effect</para>
/// 
/// v1.0 01/2019
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public sealed class GlitchEffectRenderer : PostProcessEffectRenderer<GlitchEffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        PropertySheet sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Glitch"));
		sheet.properties.SetFloat("_Size", settings.Strips);
		sheet.properties.SetFloat("_Strength", settings.Strength);
		sheet.properties.SetFloat("_Amount", settings.Amount);
		sheet.properties.SetFloat("_StrengthRandomness", settings.StrengthRandomness);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}