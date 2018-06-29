using UnityEditor;
using UnityEngine;

/// <summary>
/// <para>This is the Controller Class for Objects with the ClipVolumeShader.
/// It defines the volume in which all of its child objects with the Clip Volume Shader are rendered</para>
///
/// v2.1 06/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class ClipVolume : MonoBehaviour
{
    // ######################## PROPERTIES ######################## //
    /// <summary>
    /// Size of the Volume. Everytime you assign to this, all child renderers are updated, so be careful when you call this!
    /// </summary>
    public Vector3 Size
    {
        get { return _size; }
        set
        {
            _size = value;
            UpdateShaderValues();
        }
    }

    // ######################## PRIVATE VARS ######################## //
    /// <summary>
    /// The size of the Box Volume
    /// </summary>
    [SerializeField] private Vector3 _size = Vector3.one;
    
    /// <summary>
    /// All Renderers that we need to take care of
    /// </summary>
    private Renderer[] _renderers;
    
    /// <summary>
    /// The transform matrix of the volume
    /// </summary>
    private Matrix4x4 _volumeMatrix;
    
    // ######################## UNITY EVENT FUNCTIONS ######################## //
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        // Only update if the matrix changed
        if (_volumeMatrix != transform.worldToLocalMatrix)
            UpdateShaderValues();
    }

    #region EDITOR
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Draw the volume as a green Cube
        Gizmos.color = new Color(0, 1, 0, 0.2f);

        Vector3 scale = transform.lossyScale;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);

        Gizmos.DrawCube(Vector3.zero, _size);
    }
    
    /// <summary>
    /// Context Menu function for creating a Clip Volume Object
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Effects/ClipVolume")]
    public static void CreateClipVolume(MenuCommand menuCommand)
    {
        // create game object with a clip volume component
        GameObject volumeObject = new GameObject("ClipVolume", typeof(ClipVolume));
        
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(volumeObject, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(volumeObject, "Create " + volumeObject.name);
        Selection.activeObject = volumeObject;
    }
    #endif
    #endregion


    // ######################## INITS ######################## //
    ///<summary>
    /// Does the Init for this Behaviour
    ///</summary>
    private void Init()
    {
        // get the child renderes
        _renderers = GetComponentsInChildren<Renderer>();
        
        // update once
        UpdateShaderValues();
    }

    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Updates the Uniforms of the ClipVolumeShader
    /// </summary>
    private void UpdateShaderValues()
    {
        // get the transform matrix
        _volumeMatrix = transform.worldToLocalMatrix;
        
        // set uniforms in all materials of all child renderers
        foreach (Renderer rend in _renderers)
        {
            foreach (Material material in rend.materials)
            {
                material.SetVector("_ClipVolumeWorldPos", transform.position);
                material.SetMatrix("_ClipVolumeWorldToLocal", _volumeMatrix);
                material.SetVector("_ClipVolumeMin", -_size / 2);
                material.SetVector("_ClipVolumeMax", _size / 2);
            }
        }
    }
}