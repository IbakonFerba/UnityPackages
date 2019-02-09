using FK.Paths;
using UnityEngine;

/// <summary>
/// <para>An Agent that can follow a Path</para>
///
/// v1.1 02/2019
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class PathAgent : MonoBehaviour
{
    // ######################## PROPERTIES ######################## //
    public bool FollowPath { get; private set; }

    // ######################## PUBLIC VARS ######################## //
    /// <summary>
    /// The Path to follow
    /// </summary>
    public Path TargetPath;

    /// <summary>
    /// Speed of the movement
    /// </summary>
    [Space] [Tooltip("Speed of the movement")]
    public float Speed = 1f;

    /// <summary>
    /// Should the Agent follow the Path as soon as it becomes active?
    /// </summary>
    [Space] [Tooltip("Should the Agent follow the Path as soon as it becomes active?")]
    public bool FollowOnStart = true;

    /// <summary>
    /// Should the Agent face the movement direction?
    /// </summary>
    [Space] [Tooltip("Should the Agent face the movement direction?")]
    public bool FaceDirection = true;

    /// <summary>
    /// Smoothing of the Facing direction. This might be needed when using a small presampling resolution on a path
    /// </summary>
    [Tooltip("Smoothing of the Facing direction. This might be needed when using a small presampling resolution on a path")]
    public float FacingSmoothingFactor = 0f;

    // ######################## PRIVATE VARS ######################## //
    /// <summary>
    /// The progress along the path of this bit
    /// </summary>
    private float _pathProgress;

    /// <summary>
    /// Total path length
    /// </summary>
    private float _pathLength;

    private Vector3 _smoothingVel;

    // ######################## UNITY EVENT FUNCTIONS ######################## //
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (FollowPath)
            Follow();
    }


    // ######################## INITS ######################## //
    ///<summary>
    /// Does the Init for this Behaviour
    ///</summary>
    private void Init()
    {
        FollowPath = FollowOnStart;
        _pathLength = TargetPath.GetLength();
    }


    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Makes the Agent follow the path
    /// </summary>
    private void Follow()
    {
        // get position along path
        transform.position = TargetPath.GetUniformPosition(_pathProgress % 1);

        // set facing direction
        if (FaceDirection)
            transform.forward = Vector3.SmoothDamp(transform.forward, TargetPath.GetUniformDirection(_pathProgress % 1), ref _smoothingVel, FacingSmoothingFactor);

        // update progress
        _pathProgress += Time.deltaTime * (Speed / _pathLength);
    }
}