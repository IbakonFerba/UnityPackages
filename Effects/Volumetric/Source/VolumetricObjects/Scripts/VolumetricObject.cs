using UnityEditor;
using UnityEngine;

/// <summary>
/// <para>This class represents a Volumetric Object. It contains all the data and manages registering itself at the VolumetricObjectRenderer</para>
///
/// v1.1 08/2018
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

    // ######################## PUBLIC VARS ######################## //
    public Types Type;
    public Color Color = Color.white;
    public float Density = 1;
    public bool EnableNoise = false;

    /// <summary>
    /// This matrix contains all noise related values like scale, transform and strength
    /// </summary>
    public Matrix4x4 NoiseST = new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(1, 0, 0, 0), new Vector4(1, 0, 0, 0), new Vector4(1, 0, 0, 0));

    /// <summary>
    /// Dimensions of a Box object
    /// </summary>
    public Vector3 Dimensions = Vector3.one;

    /// <summary>
    /// Radius of a Sphere and Capsule Object
    /// </summary>
    public float Radius = 1;

    /// <summary>
    /// Row 1 and 2 contain the two Points for a Capsule
    /// </summary>
    public Matrix4x4 Bounds = Matrix4x4.identity;

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

        // set bounds
        vo.Bounds[0, 0] = 0;
        vo.Bounds[0, 1] = 1;
        vo.Bounds[0, 2] = 0;
        vo.Bounds[1, 0] = 0;
        vo.Bounds[1, 1] = 0;
        vo.Bounds[1, 2] = 0;
    }
}