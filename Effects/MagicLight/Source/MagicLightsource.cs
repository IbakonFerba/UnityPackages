using UnityEngine;
using unexpected;

/// <summary>
/// <para>A spotlight that affects which parts of Objects with the MagicLightShader will be displayed</para>
/// 
/// v1.1 06/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[RequireComponent(typeof(Light))]
[ExecuteInEditMode]
public class MagicLightsource : Singleton<MagicLightsource>
{
    // ######################## PUBLIC VARS ######################## //
    /// <summary>
    /// Angle of the spotlight cone
    /// </summary>
    public float Angle = 45f;

    // ######################## PRIVATE VARS ######################## //
    /// <summary>
    /// The attacted real spotlight
    /// </summary>
    private Light _light;

    // ######################## UNITY EVENT FUNCTIONS ######################## //
    private void Reset()
    {
        InitSpotlight();
    }

    private void Start()
    {
        InitSpotlight();
    }

    private void Update()
    {
        // make sure spotlight angle is the same as this one
        _light.spotAngle = Angle;

        // set uniforms
        Shader.SetGlobalVector("_MagicLightPos", transform.position);
        Shader.SetGlobalVector("_MagicLightDir", transform.forward);
        Shader.SetGlobalFloat("_MagicLightAngle", Mathf.Deg2Rad * Angle);
    }

    // ######################## INITS ######################## //
    /// <summary>
    /// get the attached spotlight and set its values
    /// </summary>
    private void InitSpotlight()
    {
        Light light = GetComponent<Light>();
        light.type = LightType.Spot;
        _light.spotAngle = Angle;
    }
}
