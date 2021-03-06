﻿using UnityEngine;

namespace FK.Utility.UI
{
    /// <summary>
    /// <para>Class to make UI face the main camera or a specified target</para>
    /// 
    /// v1.1 08/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class Billboard : MonoBehaviour
    {
        /// <summary>
        /// If this is true, the forward vector will face away from the target, alse it will point at the target
        /// </summary>
        [Tooltip("If this is true, the forward vector will face away from the target, alse it will point at the target")]
        public bool FlipFacing = true;

        /// <summary>
        /// If this is set, the UI should face it instead of the main Camera
        /// </summary>
        [Tooltip("If you set a gameobject here, the UI will face that object, else it will face the main camera")]
        public Transform Target;

        private void Update()
        {
            Transform target = Target ? Target : Camera.main.transform;

            transform.LookAt(target);

            if (FlipFacing)
                transform.forward = -transform.forward;
        }
    }
}