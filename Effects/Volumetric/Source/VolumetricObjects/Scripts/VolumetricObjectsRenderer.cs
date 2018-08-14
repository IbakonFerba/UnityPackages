using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>This script renders Volumetric Objects as an Image Effect. Each Volumetric Object adds one Pass, so be careful with how many you use!</para>
/// <para>Created with this Tutorial on Raymarching: http://flafla2.github.io/2016/10/01/raymarching.html</para>
///
/// v1.1 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Effects/Volumetric Objects")]
public class VolumetricObjectsRenderer : SceneViewFilter
{
    // ######################## PROPERTIES ######################## //
    /// <summary>
    /// The Material that renders the Effect
    /// </summary>
    private Material EffectMaterial
    {
        get
        {
            // create material if it does not exist yet
            if (!_effectMaterial)
            {
                _effectMaterial = new Material(Shader.Find("Hidden/VolumetricObjectsShader"));
                _effectMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _effectMaterial;
        }
    }

    /// <summary>
    /// The Camera that is rendering the effect
    /// </summary>
    private Camera CurrentCamera
    {
        get
        {
            // get camera if we don't already have it
            if (!_currentCamera)
            {
                _currentCamera = GetComponent<Camera>();
                _currentCamera.tag = "MainCamera";
            }
            return _currentCamera;
        }
    }

    // ######################## PUBLIC VARS ######################## //
    /// <summary>
    /// The distance each raymarching iteration progresses
    /// </summary>
    [Tooltip("The distance each raymarching iteration progresses")]
    public float StepSize = 0.05f;

    /// <summary>
    /// Max Number of steps outside of Volumes
    /// </summary>
    [Tooltip("Max Number of steps outside of Volumes")]
    public int StepCount = 64;

    /// <summary>
    /// Max Number of Steps inside Volumes
    /// </summary>
    [Tooltip("Max Number of Steps inside Volumes")]
    public int StepCountInsideVolume = 100;

    /// <summary>
    /// The maximum draw distance in Unity units
    /// </summary>
    [Space] [Tooltip("The maximum draw distance in Unity units")]
    public int DrawDist = 40;

    // ######################## PRIVATE VARS ######################## //
    /// <summary>
    /// All Volumetric Objects
    /// </summary>
    private static List<VolumetricObject> _objs;

    private Material _effectMaterial;
    private Camera _currentCamera;

    private Matrix4x4 _frustumCorners = Matrix4x4.identity;

    /// <summary>
    /// The Camera FOV at the last Matrix update
    /// </summary>
    private float _prevFov;

    /// <summary>
    /// The Camera Aspect ratio at the last Matrix update
    /// </summary>
    private float _prevAspect;


    // ######################## UNITY EVENT FUNCTIONS ######################## //	
    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // if for some reason there is no material or if there are no Objects, do nothing
        if (!EffectMaterial || _objs == null || _objs.Count == 0)
        {
            Graphics.Blit(src, dest);
            return;
        }

        // sort the Objects by depth
        _objs.Sort((x, y) => (int) Mathf.Clamp(CurrentCamera.transform.InverseTransformPoint(y.transform.position).z - CurrentCamera.transform.InverseTransformPoint(x.transform.position).z, -1, 1));

        // set general shader values dor camera
        EffectMaterial.SetMatrix("_FrustumCornersES", GetFrustumCorners(CurrentCamera));
        EffectMaterial.SetMatrix("_CameraInvViewMatrix", CurrentCamera.cameraToWorldMatrix);
        EffectMaterial.SetVector("_CameraWS", CurrentCamera.transform.position);

        // set general shader values for raymarching
        EffectMaterial.SetInt("_StepCount", StepCount);
        EffectMaterial.SetInt("_StepCountInside", StepCountInsideVolume);
        EffectMaterial.SetFloat("_StepSize", StepSize);
        EffectMaterial.SetInt("_DrawDist", DrawDist);

        // create to temporary Render textures as buffers
        RenderTexture temp0 = RenderTexture.GetTemporary(src.width, src.height, 0, src.format);
        RenderTexture temp1 = RenderTexture.GetTemporary(src.width, src.height, 0, src.format);

        // this is a variable for storing which buffer we last used
        int lastUsed = 0;

        // render all objects
        for (int i = 0; i < _objs.Count; ++i)
        {
            // set shader values that are type dependend
            VolumetricObject.Types t = _objs[i].Type;
            switch (t)
            {
                case VolumetricObject.Types.BOX:
                    EffectMaterial.SetInt("_Type", 0);
                    EffectMaterial.SetVector("_BoxDimensions", _objs[i].Dimensions * 0.5f);
                    break;
                case VolumetricObject.Types.SPHERE:
                    EffectMaterial.SetInt("_Type", 1);
                    EffectMaterial.SetFloat("_SphereRad", _objs[i].Radius);
                    break;
                case VolumetricObject.Types.CAPSULE:
                    EffectMaterial.SetInt("_Type", 2);
                    EffectMaterial.SetFloat("_SphereRad", _objs[i].Radius);
                    EffectMaterial.SetMatrix("_CapsuleBounds", _objs[i].Bounds);
                    break;
            }

            // set non type dependent per Object values
            EffectMaterial.SetColor("_Color", _objs[i].Color);
            EffectMaterial.SetFloat("_Density", _objs[i].Density);
            EffectMaterial.SetMatrix("_Noise_ST", _objs[i].NoiseST);
            EffectMaterial.SetMatrix("_InvModel", _objs[i].transform.localToWorldMatrix.inverse);

            // render the Object by alternating between buffers
            if (i == 0)
            {
                CustomGraphicsBlit(src, temp0, EffectMaterial, 0);
                lastUsed = 0;
            }
            else if (i % 2 == 0)
            {
                CustomGraphicsBlit(temp1, temp0, EffectMaterial, 0);
                lastUsed = 0;
            }
            else
            {
                CustomGraphicsBlit(temp0, temp1, EffectMaterial, 0);
                lastUsed = 1;
            }
        }

        // render final render texture
        Graphics.Blit(lastUsed == 1 ? temp1 : temp0, dest);

        // clean up buffers
        RenderTexture.ReleaseTemporary(temp0);
        RenderTexture.ReleaseTemporary(temp1);
    }

    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Creates a Matrix containing the frustum corner vectors
    /// </summary>
    /// <param name="cam"></param>
    /// <returns></returns>
    private Matrix4x4 GetFrustumCorners(Camera cam)
    {
        // get fov and aspect
        float camFov = cam.fieldOfView;
        float camAspect = cam.aspect;

        // if nothing changed we don't need to recalculate the matrix
        if (camFov == _prevFov && camAspect == _prevAspect)
            return _frustumCorners;

        // save fov and aspect
        _prevFov = camFov;
        _prevAspect = camAspect;

        // create the matrix
        _frustumCorners = Matrix4x4.identity;

        // calculate neede values
        float fovHalf = camFov * 0.5f;
        float tanFov = Mathf.Tan(fovHalf * Mathf.Deg2Rad);

        // calculate Rigth and up vectors in view Space
        Vector3 toRight = Vector3.right * tanFov * camAspect;
        Vector3 toTop = Vector3.up * tanFov;

        // calculate frustum vectors in view space
        Vector3 topLeft = -Vector3.forward - toRight + toTop;
        Vector3 topRight = -Vector3.forward + toRight + toTop;
        Vector3 bottomRight = -Vector3.forward + toRight - toTop;
        Vector3 bottomLeft = -Vector3.forward - toRight - toTop;

        // save vectors in matrix
        _frustumCorners.SetRow(0, topLeft);
        _frustumCorners.SetRow(1, topRight);
        _frustumCorners.SetRow(2, bottomRight);
        _frustumCorners.SetRow(3, bottomLeft);

        return _frustumCorners;
    }

    /// <summary>
    /// Customized version of Graphics.Blit which allows us to pass the frustum information to the shader
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dest"></param>
    /// <param name="fxMaterial"></param>
    /// <param name="passNr"></param>
    private static void CustomGraphicsBlit(RenderTexture src, RenderTexture dest, Material fxMaterial, int passNr)
    {
        RenderTexture.active = dest;

        fxMaterial.SetTexture("_MainTex", src);

        GL.PushMatrix();
        GL.LoadOrtho();

        fxMaterial.SetPass(passNr);

        GL.Begin(GL.QUADS);

        // Here, GL.MultitexCoord2(0, x, y) assigns the value (x, y) to the TEXCOORD0 slot in the shader.
        // GL.Vertex3(x,y,z) queues up a vertex at position (x, y, z) to be drawn.  Note that we are storing
        // our own custom frustum information in the z coordinate.
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f); // BL

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f); // BR

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f); // TR

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f); // TL

        GL.End();
        GL.PopMatrix();
    }

    /// <summary>
    /// Adds the Volumetric Objects to teh Renderer if it is not registered yet
    /// </summary>
    /// <param name="obj"></param>
    public static void AddObj(VolumetricObject obj)
    {
        if (_objs == null)
            _objs = new List<VolumetricObject>();

        if (!_objs.Contains(obj))
            _objs.Add(obj);
    }

    /// <summary>
    /// Returns true if the Volumetric Object is reguistered in the Renderer
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool Contains(VolumetricObject obj)
    {
        return _objs != null && _objs.Contains(obj);
    }

    /// <summary>
    /// Removes the Object from the Renderer
    /// </summary>
    /// <param name="obj"></param>
    public static void Remove(VolumetricObject obj)
    {
        if (_objs != null && _objs.Contains(obj))
            _objs.Remove(obj);
    }
}