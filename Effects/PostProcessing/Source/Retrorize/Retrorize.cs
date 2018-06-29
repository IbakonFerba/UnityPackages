using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// <para>Settings class for the Post Processing Effect</para>
/// 
/// v1.0 06/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[System.Serializable]
[PostProcess(typeof(RetrorizeRenderer), PostProcessEvent.AfterStack, "Custom/Retrorize")]
public sealed class Retrorize : PostProcessEffectSettings
{
    public IntParameter BitsForColors = new IntParameter {value = 16};
    public Vector2Parameter Resolution = new Vector2Parameter {value = new Vector2(256, 240)};
}

/// <summary>
/// <para>Renderer for the Retrorize Post Processing Effect</para>
/// 
/// v1.0 06/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public sealed class RetrorizeRenderer : PostProcessEffectRenderer<Retrorize>
{
    public override void Render(PostProcessRenderContext context)
    {
        PropertySheet sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Retrorize"));
        sheet.properties.SetFloat("_Bits", settings.BitsForColors);
        sheet.properties.SetVector("_Resolution", settings.Resolution);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}