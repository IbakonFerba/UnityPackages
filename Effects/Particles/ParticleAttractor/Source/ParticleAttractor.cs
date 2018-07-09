using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FK.Effects.Particles
{
    /// <summary>
    /// <para>An Attractor for Particles.
    /// It attracts the Particles of the defined Particle Systems with a specified force, destroys them if they enter a specific radius around it
    /// and limits their Velocity if a terminal Velocity is set in the Custom Data of the Particle System.</para>
    ///
    /// v1.0 07/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [ExecuteInEditMode]
    public class ParticleAttractor : MonoBehaviour
    {
        // ######################## STRUCTS & ENUMS ######################## //
        /// <summary>
        /// A Particle System accociated with a Buffer of Particles
        /// </summary>
        [System.Serializable]
        public class ParticleSystemData
        {
            /// <summary>
            /// The actual Particle System
            /// </summary>
            public ParticleSystem System;

            /// <summary>
            /// The Particle Buffer
            /// </summary>
            [System.NonSerialized] public ParticleSystem.Particle[] Particles;
        }

        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// The Force with which the Particles are attracted
        /// </summary>
        public float Force = 1f;

        /// <summary>
        /// Particles closer to the attractor than this Distance will be destroyed
        /// </summary>
        [Tooltip("Particles closer to the attractor than this Distance will be destroyed")]
        public float DeathZone = 1f;

        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// All the affected Particle Systems
        /// </summary>
        public List<ParticleSystemData> SystemData = new List<ParticleSystemData>();

        // ######################## UNITY EVENT FUNCTIONS ######################## //
        private void LateUpdate()
        {
            // go through all particle systems
            for (int i = SystemData.Count - 1; i >= 0; --i)
            {
                // get the current system and go to the next one if it is null
                ParticleSystem system = SystemData[i].System;
                if (system == null)
                    continue;

                // initialize Particle Array if necessary
                if (SystemData[i].Particles == null || SystemData[i].Particles.Length != system.main.maxParticles)
                    SystemData[i].Particles = new ParticleSystem.Particle[system.main.maxParticles];

                // get the particles and the data we need from the particle system
                int numParticlesAlive = system.GetParticles(SystemData[i].Particles);
                float terminalVelocity = system.customData.GetVector(ParticleSystemCustomData.Custom1, 0).constant;

                // create variables that are going to be needed in the Particles Loop
                Vector3 dir;
                Vector3 vel;
                float dist;

                // Change only the particles that are alive
                for (int j = 0; j < numParticlesAlive; j++)
                {
                    // get the vector between the particle and the Attractor
                    dir = transform.position - SystemData[i].Particles[j].position;

                    // the length of that vector is the distance between the particle and the Attractor
                    dist = dir.magnitude;

                    // if the particle is inside the death zone, set its remaing lifetime to 0 so it is destroyed
                    if (dist < DeathZone)
                        SystemData[i].Particles[j].remainingLifetime = 0;

                    // calculate the new velocity
                    vel = SystemData[i].Particles[j].velocity + dir.normalized * Force * Time.deltaTime;

                    // limit the velocity to the terminal velocity if custom data is enabled
                    if (system.customData.enabled && vel.magnitude > terminalVelocity)
                        vel = vel.normalized * terminalVelocity;

                    // set the velocity
                    SystemData[i].Particles[j].velocity = vel;
                }

                // Apply the particle changes to the particle system
                system.SetParticles(SystemData[i].Particles, numParticlesAlive);
            }
        }

        #region EDITOR

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            // Draw the death radius as a red sphere
            Gizmos.color = new Color(1, 0, 0, 0.4f);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawSphere(Vector3.zero, DeathZone);
        }

        /// <summary>
        /// Context Menu function for creating a Particle Attractor Object
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("GameObject/Effects/Particle Attractor")]
        public static void CreateParticleAttractor(MenuCommand menuCommand)
        {
            // create game object with a particle attractor component
            GameObject attractorObject = new GameObject("ParticleAttractor", typeof(ParticleAttractor));

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(attractorObject, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(attractorObject, "Create " + attractorObject.name);
            Selection.activeObject = attractorObject;
        }
#endif

        #endregion
    }
}