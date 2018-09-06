using UnityEditor;
using UnityEngine;

/// <summary>
/// <para>This is the Controller Class for Objects with the ClipVolumeShader.
/// It defines the volume in which all of its child objects with the Clip Volume Shader are rendered</para>
///
/// v3.2 09/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[ExecuteInEditMode]
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

    /// <summary>
    /// Pivot of the Volume relative to its size. Everytime you assign to this, all child renderers are updated, so be careful when you call this!
    /// </summary>
    public Vector3 Pivot
    {
        get { return _pivot; }
        set
        {
            _pivot = value;
            ClampPivot();
            UpdateShaderValues();
        }
    }

    /// <summary>
    /// This property makes sure a Property Block exists and returns it
    /// </summary>
    private MaterialPropertyBlock PropertyBlock => _propertyBlock ?? (_propertyBlock = new MaterialPropertyBlock());

    /// <summary>
    /// This property makes sure we have the renderers and returns them
    /// </summary>
    private Renderer[] Renderers => _renderers ?? (_renderers = GetComponentsInChildren<Renderer>(true));

    // ######################## PRIVATE VARS ######################## //
    /// <summary>
    /// The size of the Box Volume
    /// </summary>
    [SerializeField] private Vector3 _size = Vector3.one;
    /// <summary>
    /// The pivot of the Box Volume relative to its size
    /// </summary>
    [SerializeField] private Vector3 _pivot = new Vector3(0.5f, 0.5f, 0.5f);

    /// <summary>
    /// All Renderers that we need to take care of (use Renderers Property to make sure this is initialized)
    /// </summary>
    private Renderer[] _renderers;

    /// <summary>
    /// The property block for setting our uniforms (use PropertyBlock Property to make sure this is initialized)
    /// </summary>
    private MaterialPropertyBlock _propertyBlock;

    /// <summary>
    /// The transform matrix of the volume
    /// </summary>
    private Matrix4x4 _volumeMatrix;

    // ######################## UNITY EVENT FUNCTIONS ######################## //
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        // Only update if the matrix changed
        if (_volumeMatrix != transform.worldToLocalMatrix)
            UpdateShaderValues();
    }

    /// <summary>
    /// Make sure everything updates when animating
    /// </summary>
    private void OnDidApplyAnimationProperties()
    {
        ClampPivot();
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

        Gizmos.DrawCube(Vector3.zero+Vector3.Scale(new Vector3(0.5f, 0.5f, 0.5f)- _pivot, _size), _size);
    }

    /// <summary>
    /// Context Menu function for creating a Clip Volume Object
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Effects/Clip Volume")]
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
        _renderers = GetComponentsInChildren<Renderer>(true);
        _propertyBlock = new MaterialPropertyBlock();

        // update once
        UpdateShaderValues();
    }

    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Updates the Uniforms of the ClipVolumeShader
    /// </summary>
    public void UpdateShaderValues()
    {
        // get the transform matrix
        _volumeMatrix = transform.worldToLocalMatrix;

        // set uniforms in property block
        PropertyBlock.SetVector("_ClipVolumeWorldPos", transform.position);
        PropertyBlock.SetMatrix("_ClipVolumeWorldToLocal", _volumeMatrix);
        PropertyBlock.SetVector("_ClipVolumeMin", -Vector3.Scale(_size,_pivot));
        PropertyBlock.SetVector("_ClipVolumeMax", Vector3.Scale(_size, Vector3.one-_pivot));

        bool updateRends = false;
        // set property block in all child renderers
        foreach (Renderer rend in Renderers)
        {
            if (rend == null)
            {
                updateRends = true;
                continue;
            }
            rend.SetPropertyBlock(PropertyBlock);
        }
        
        if(updateRends)
            _renderers = GetComponentsInChildren<Renderer>(true);
    }
    
    // ######################## UTILITY ######################## //
    /// <summary>
    /// Clamps the values of the pivot between 0 and 1
    /// </summary>
    private void ClampPivot()
    {
        _pivot.x = Mathf.Clamp01(_pivot.x);
        _pivot.y = Mathf.Clamp01(_pivot.y);
        _pivot.z = Mathf.Clamp01(_pivot.z);
    }
}