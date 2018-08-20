using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// <para>This class represents a Volumetric Object. It contains all the data and manages registering itself at the VolumetricObjectRenderer</para>
///
/// v2.1 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[ExecuteInEditMode]
public class VolumetricObject : MonoBehaviour
{
    // ######################## ENUMS & DELEGATES ######################## //
    /// <summary>
    /// All Types of Volumetric Objects that are available
    /// </summary>
    public enum Types
    {
        BOX,
        SPHERE,
        CAPSULE
    }

    /// <summary>
    /// All alvailable falloff types
    /// </summary>
    public enum FalloffTypes
    {
        LINEAR = 0,
        EXPONENTIAL = 1,
        SQUARED_EXPONENTIAL = 2
    }

    /// <summary>
    /// All available alpha blend modes
    /// </summary>
    public enum BlendModes
    {
        ALPHA_BLENDED = 0,
        ADDITIVE = 1
    }

    // ######################## PROPERTIES VARS ######################## //
    /// <summary>
    /// Sets scale of the Noise and enables noise
    /// </summary>
    public Vector3 NoiseScale
    {
        get { return new Vector3(NoiseSTO[0, 0], NoiseSTO[0, 1], NoiseSTO[0, 2]); }
        set
        {
            NoiseSTO[0, 0] = value.x;
            NoiseSTO[0, 1] = value.y;
            NoiseSTO[0, 2] = value.z;
            NoiseSTO[2, 0] = 1;
        }
    }

    /// <summary>
    /// Sets transform of the Noise and enables noise
    /// </summary>
    public Vector3 NoiseTransform
    {
        get { return new Vector3(NoiseSTO[1, 0], NoiseSTO[1, 1], NoiseSTO[1, 2]); }
        set
        {
            NoiseSTO[1, 0] = value.x;
            NoiseSTO[1, 1] = value.y;
            NoiseSTO[1, 2] = value.z;
            NoiseSTO[2, 0] = 1;
        }
    }

    /// <summary>
    /// Sets strenght og the noise and enables noise
    /// </summary>
    public float NoiseStrength
    {
        get { return NoiseSTO[2, 1]; }
        set
        {
            NoiseSTO[2, 1] = Mathf.Clamp01(value);
            NoiseSTO[2, 0] = 1;
        }
    }

    /// <summary>
    /// Enables or diables noise
    /// </summary>
    public bool EnableNoise
    {
        get { return NoiseSTO[2, 0] > 0; }
        set { NoiseSTO[2, 0] = value ? 1 : 0; }
    }

    /// <summary>
    /// Sets the noise to be local or global
    /// </summary>
    public bool GlobalNoise
    {
        get { return NoiseSTO[2, 2] > 0; }
        set { NoiseSTO[2, 2] = value ? 1 : 0; }
    }

    // ######################## PUBLIC VARS ######################## //
    public float Density = 1;
    public FalloffTypes Falloff = FalloffTypes.SQUARED_EXPONENTIAL;
    public Color Color = Color.white;

    /// <summary>
    /// If true, the denser the fog is the more its color will become the DenseColor
    /// </summary>
    public bool UseDenseColor;

    /// <summary>
    /// Color for denser areas
    /// </summary>
    public Color DenseColor = Color.white;

    public BlendModes BlendMode = BlendModes.ALPHA_BLENDED;

    /// <summary>
    /// This matrix contains all noise related values like scale, transform and strength
    ///
    /// Row 0: Scale
    /// Row 1: Translation
    /// Row 2: Options
    ///     0: enable noise (true if > 0)
    ///     1: noise strength between 0 and 1
    ///     2: global noise (true if > 0
    /// </summary>
    public Matrix4x4 NoiseSTO = new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(1, 0, 1, 0), new Vector4(1, 0, 0, 0), new Vector4(1, 0, 0, 0));

    /// <summary>
    /// The type of the Object
    /// </summary>
    public Types Type;

    /// <summary>
    /// Dimensions of a Box object
    /// </summary>
    public Vector3 Dimensions = Vector3.one;

    /// <summary>
    /// Radius of a Sphere and Capsule Object
    /// </summary>
    public float Radius = 1;

    /// <summary>
    /// Row 0: Point 1
    /// Row 1: Point 2
    /// [2,0]: Radius
    /// </summary>
    public Matrix4x4 CapsuleParams = new Matrix4x4(new Vector4(0, 0, 0.5f, 0), new Vector4(1, 0, 0, 0), Vector4.zero, Vector4.zero);

    // ######################## UNITY EVENT FUNCTIONS ######################## //
    private void Update()
    {
        // register the object if it isnt in the list already
        if (!VolumetricObjectsRenderer.Contains(this))
            VolumetricObjectsRenderer.AddObj(this);
    }

    private void OnDestroy()
    {
        // unregister the object
        VolumetricObjectsRenderer.Remove(this);
    }

    private void OnDisable()
    {
        // unregister the object
        VolumetricObjectsRenderer.Remove(this);
    }

    // ######################## FUNCTIONALITY ######################## //

    #region EDITOR

#if UNITY_EDITOR
    /// <summary>
    /// Creates a Volumetric Object
    /// </summary>
    /// <param name="menuCommand"></param>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static VolumetricObject CreateVolumetricObject(MenuCommand menuCommand, Types type, string name)
    {
        VolumetricObjectsRenderer rend = Camera.main.GetComponent<VolumetricObjectsRenderer>();
        if (rend == null)
        {
            Camera.main.gameObject.AddComponent<VolumetricObjectsRenderer>();
        }

        // create the Object
        GameObject obj = new GameObject(name, typeof(VolumetricObject));
        VolumetricObject vo = obj.GetComponent<VolumetricObject>();

        // set type
        vo.Type = type;

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
        Selection.activeObject = obj;
        return vo;
    }

    /// <summary>
    /// Context Menu function for creating a VolumetricBox
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Volumetric/Box", false, 1)]
    public static void CreateVolumetricBox(MenuCommand menuCommand)
    {
        CreateVolumetricObject(menuCommand, Types.BOX, "VolumetricBox");
    }

    /// <summary>
    /// Context Menu function for creating a VolumetricSphere
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Volumetric/Sphere", false, 1)]
    public static void CreateVolumetricSphere(MenuCommand menuCommand)
    {
        CreateVolumetricObject(menuCommand, Types.SPHERE, "VolumetricSphere");
    }

    /// <summary>
    /// Context Menu function for creating a VolumetricCapsule
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Volumetric/Capsule", false, 1)]
    public static void CreateVolumetricCapsule(MenuCommand menuCommand)
    {
        VolumetricObject vo = CreateVolumetricObject(menuCommand, Types.CAPSULE, "VolumetricCapsule");
    }
#endif

    #endregion
}