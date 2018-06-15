using UnityEngine;
using System.Collections.Generic;
using FK.Utility;

/// <summary>
/// This class casts rays from touches and the mouse into the world. Use this so you don't have to cast one (or multiple) ray(s) for each Interactable each frame
/// </summary>
[RequireComponent(typeof(Camera))]
public class InteractablesRaycaster : Singleton<InteractablesRaycaster>
{
    // ######################## STRUCTS & CLASSES ######################## //
    /// <summary>
    /// A struct that can store a RaycastHit object and an accociated touchId
    /// </summary>
    public struct TouchRaycastHit
    {
        public RaycastHit Hit;
        /// <summary>
        /// fingerId of the touch that casted the ray
        /// </summary>
        public int TouchId;

        public TouchRaycastHit(RaycastHit hit, int touchId)
        {
            Hit = hit;
            TouchId = touchId;
        }
    }


    // ######################## PRIVATE VARS ######################## //
    /// <summary>
    /// The camera to cast rays from
    /// </summary>
    private Camera _cam;

    /// <summary>
    /// All RaycastHits from touches
    /// </summary>
    private List<TouchRaycastHit> _raycastHits = new List<TouchRaycastHit>();
    /// <summary>
    /// RaycastHit from the Mouse
    /// </summary>
    private RaycastHit _mouseHit;



    // ######################## UNITY START & UPDATE ######################## //

    void Start() { Init(); }

    void Update()
    {
        //remove all old Raycasts
        _raycastHits.Clear();

        // cast a ray for each touch
        foreach(Touch touch in Input.touches)
        {
            RaycastHit hit;
            Ray camRay = _cam.ScreenPointToRay(touch.position); //get the ray

            // cast the ray
            if(Physics.Raycast(camRay, out hit, _cam.farClipPlane, ~Physics.IgnoreRaycastLayer))
            {
                // save the RaycastHit
                _raycastHits.Add(new TouchRaycastHit(hit, touch.fingerId));


                Debug.DrawLine(camRay.origin, hit.point, Color.yellow);
            }
        }

        Ray mouseCamRay = _cam.ScreenPointToRay(Input.mousePosition); // get the ray for the mouse

        // cast the ray for the mouse
        if (Physics.Raycast(mouseCamRay, out _mouseHit, _cam.farClipPlane, ~Physics.IgnoreRaycastLayer))
        {
            Debug.DrawLine(mouseCamRay.origin, _mouseHit.point, Color.yellow);
        }
    }



    // ######################## INITS ######################## //

    /// <summary>Does the init for this behaviour</summary>
    private void Init()
    {
        _cam = GetComponent<Camera>();
    }


    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Returns true if a ray casted from any touch if the provided ID is -1 or the one with the provided id hit the provided Game Object or one of its children (this does not cast a ray, Rays are only casted once in each frame in Update)
    /// </summary>
    /// <param name="obj">The GameObject you want to test</param>
    /// <param name="touchId">The ID of the touch to test. Use -1 to testv all touches</param>
    /// <param name="useMouseInput">If true this method will also check the ray casted from the mouse</param>
    /// <returns></returns>
    private bool HitGameObjectOrChild(GameObject obj, int touchId, bool useMouseInput)
    {
        // go through all hits
        foreach(TouchRaycastHit touchRaycast in _raycastHits)
        {
            // if the touchID is not the default value and the one of the hit is not the one we are looking for, check the next Hit
            if (touchId != -1 && touchRaycast.TouchId != touchId)
                continue;

            RaycastHit hit = touchRaycast.Hit;

            // if nothing was hit, don't test for this object
            if (hit.collider == null)
                continue;

            // test if this object or one of its children were hit
            if(hit.collider.gameObject == obj || hit.collider.gameObject.transform.IsChildOf(obj.transform))
            {
                return true;
            }
        }

        // test for the mouse ray
        if(useMouseInput && touchId == -1)
        {
            if (_mouseHit.collider)
            {
                if (_mouseHit.collider.gameObject == obj || _mouseHit.collider.gameObject.transform.IsChildOf(obj.transform))
                {
                    return true;
                }
            }
        }

        // the obejct was not hit, return false
        return false;
    }

    /// <summary>
    /// Returns true if a ray casted from any touch hit the provided Game Object or one of its children (this does not cast a ray, Rays are only casted once in each frame in Update)
    /// </summary>
    /// <param name="obj">The GameObject you want to test</param>
    /// <param name="useMouseInput">If true this method will also check the ray casted from the mouse</param>
    /// <returns></returns>
    public bool HitGameObjectOrChild(GameObject obj, bool useMouseInput = false)
    {
        return HitGameObjectOrChild(obj, -1, useMouseInput);
    }

    /// <summary>
    /// Returns true if a ray casted from the touch with the provided id hit the provided Game Object or one of its children (this does not cast a ray, Rays are only casted once in each frame in Update)
    /// </summary>
    /// <param name="obj">The GameObject you want to test</param>
    /// <param name="touchId">The fingerID of the touch you want to test</param>
    /// <returns></returns>
    public bool HitGameObjectOrChild(GameObject obj, int touchId)
    {
        return HitGameObjectOrChild(obj, touchId, false);
    }

    /// <summary>
    /// Returns true if a ray casted from any touch if the provided ID is -1 or the one with the provided id hit a Game Object with the provided tag (this does not cast a ray, Rays are only casted once in each frame in Update)
    /// </summary>
    /// <param name="tag">The tag to test for</param>
    /// <param name="touchId">The fingerID of the touch you want to test, use -1 to check all touches</param>
    /// <param name="useMouseInput">If true this method will also check the ray casted from the mouse</param>
    /// <returns></returns>
    private bool HitObjectWithTag(string tag, int touchId, bool useMouseInput)
    {
        // go through all hits
        foreach (TouchRaycastHit touchRaycast in _raycastHits)
        {
            // if the touchID is not the default value and the one of the hit is not the one we are looking for, check the next Hit
            if (touchId != -1 && touchRaycast.TouchId != touchId)
                continue;

            RaycastHit hit = touchRaycast.Hit;

            // if nothing was hit, don't test for this object
            if (hit.collider == null)
                continue;

            // test if this object or one of its children were hit
            if (hit.collider.gameObject.tag == tag)
            {
                return true;
            }
        }

        // test for the mouse ray
        if (useMouseInput && touchId != -1)
        {
            if (_mouseHit.collider)
            {
                if (_mouseHit.collider.gameObject.tag == tag)
                {
                    return true;
                }
            }
        }

        // the obejct was not hit, return false
        return false;
    }

    /// <summary>
    /// Returns true if a ray casted from any touch hit a Game Object with the provided tag (this does not cast a ray, Rays are only casted once in each frame in Update)
    /// </summary>
    /// <param name="tag">The tag to test for</param>
    /// <param name="useMouseInput">If true this method will also check the ray casted from the mouse</param>
    /// <returns></returns>
    public bool HitObjectWithTag(string tag, bool useMouseInput = false)
    {
        return HitObjectWithTag(tag, -1, useMouseInput);
    }

    /// <summary>
    /// Returns true if a ray casted from the touch with the provided fingerId hit a Game Object with the provided tag (this does not cast a ray, Rays are only casted once in each frame in Update)
    /// </summary>
    /// <param name="tag">The tag to test for</param>
    /// <param name="touchId">The fingerID of the touch you want to test</param>
    /// <returns></returns>
    public bool HitObjectWithTag(string tag, int touchId)
    {
        return HitObjectWithTag(tag, touchId, false);
    }
}
