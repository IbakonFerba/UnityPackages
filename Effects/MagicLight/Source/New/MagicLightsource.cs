using UnityEngine;

/// <summary>
/// <para>A "Light" that affects which parts of Objects with the MagicLightShader will be displayed</para>
/// 
/// v2.0 09/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[ExecuteInEditMode]
public class MagicLightsource : MonoBehaviour
{
    // ######################## PUBLIC VARS ######################## //
    public float Radius = 1.0f;
    
    // ######################## PRIVATE VARS ######################## //
    private Vector3 _lastPos = Vector3.zero;
    private Vector3 _lastDir= Vector3.zero;
    private float _lastRad = 0.0f;

    // ######################## UNITY EVENT FUNCTIONS ######################## //
    private void Update()
    {
        if (transform.position != _lastPos)
        {
            Shader.SetGlobalVector("_MagicLightPos", transform.position);
            _lastPos = transform.position;
        }

        if (transform.forward != _lastDir)
        {
            Shader.SetGlobalVector("_MagicLightDir", transform.forward);
            _lastDir = transform.forward;
        }

        if (Mathf.Abs(Radius - _lastRad) > 0.00001f)
        {
            Shader.SetGlobalFloat("_MagicLightRad", Radius);
            _lastRad = Radius;
        }
    }
}
