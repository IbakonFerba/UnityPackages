using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>A simple class for an easy Turntable Animation</para>
/// 
/// v1.0 06/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class Turntable : MonoBehaviour
{
    // ######################## ENUMS & DELEGATES ######################## //
    public enum TurnAxis
    {
        X,
        Y,
        Z,
    }

    // ######################## PUBLIC VARS ######################## //
    /// <summary>
    /// Axis to rotate around
    /// </summary>
    [Tooltip("Axis to rotate around")]
    public TurnAxis Axis = TurnAxis.Y;
    /// <summary>
    /// Space to use for the Rotation
    /// </summary>
    [Tooltip("Space to use for the Rotation")]
    public Space Space = Space.World;

    [Space]
    public float Speed = 20.0f;

    /// <summary>
    /// Deactivate only the parent Rigdbody or all Rigidbodies below this object?
    /// </summary>
    [Space]
    [Tooltip("Deactivate only the parent Rigdbody or all Rigidbodies below this object?")]
    public bool DeactivateOnlyParentRigidbody = false;

    // ######################## PRIVATE VARS ######################## //
    private readonly Dictionary<TurnAxis, Vector3> _axes = new Dictionary<TurnAxis, Vector3>();

    // ######################## UNITY EVENT FUNCTIONS ######################## //
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Rotate();
    }

    // ######################## INITS ######################## //
    ///<summary>
    /// Does the Init for this Behaviour
    ///</summary>
    private void Init()
    {
        // set axes
        _axes.Add(TurnAxis.X, Vector3.right);
        _axes.Add(TurnAxis.Y, Vector3.up);
        _axes.Add(TurnAxis.Z, Vector3.forward);

        // deactivate Rigidbodies
        if (DeactivateOnlyParentRigidbody)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            if (body != null)
                body.isKinematic = true;
        }
        else
        {
            Rigidbody[] bodys = transform.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody body in bodys)
            {
                body.isKinematic = true;
            }
        }
    }

    // ######################## FUNCTIONALITY ######################## //
    private void Rotate()
    {
        float step = Speed * Time.deltaTime;
        transform.Rotate(_axes[Axis], step, Space);
    }
}
