using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using FK.Utility;
#endif

namespace FK.Utility
{
    /// <summary>
    /// <para>A Class for Hosting Coroutines. Whenever you need an extra MonoBehaviour to run a Coroutine on, you can use the Instance Property of this Host.</para>
    /// <para>If you use its static functions, you can track corutines to stop them manually. This is mostly intedned for use with the other Functions in this Package</para>
    ///
    /// v2.3 02/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class CoroutineHost : MonoBehaviour
    {
        // ######################## STRUCTS & CLASSES ######################## //
        /// <summary>
        /// A Corutine Paired to an Object. This is the non generic interface
        /// </summary>
        public class CoroutineObjectPair
        {
            public Coroutine Routine;
#if UNITY_EDITOR
            /// <summary>
            /// The name of the Object used for debug purposes
            /// </summary>
            public string ObjName;
#endif
        }

        /// <summary>
        /// A Corutine Paired to an Object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class CoroutineObjectPair<T> : CoroutineObjectPair
        {
            public T Obj;

            public CoroutineObjectPair(Coroutine routine, T obj)
            {
                Routine = routine;
                Obj = obj;
#if UNITY_EDITOR
                ObjName = obj.ToString();
#endif
            }
        }

        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// The Instance used to run Coroutines. Initializes itself if no Instance exists yet
        /// </summary>
        public static CoroutineHost Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("<Master_of_Coroutines>", typeof(CoroutineHost)).GetComponent<CoroutineHost>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Public access for the Coroutines for Debug purposes
        /// </summary>
        public static Dictionary<string, List<CoroutineObjectPair>> Coroutines => _coroutines;
#endif

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// Backing for the Instance Property
        /// </summary>
        private static CoroutineHost _instance;

        /// <summary>
        /// This structure contains all the Tracked coroutines
        /// </summary>
        private static Dictionary<string, List<CoroutineObjectPair>> _coroutines = new Dictionary<string, List<CoroutineObjectPair>>();

        // ######################## UNITY EVENT FUNCTIONS ######################## //
        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this);

                Debug.LogWarning("Tried to Instantiate second instance of Coroutine Host. Additional Instance was destroyed.");
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Starts a Coroutine on the provided Object and remembers it so it can be stopped later
        /// </summary>
        /// <param name="routine">The Coroutine to start</param>
        /// <param name="obj">The Object that is affected by the Coroutine</param>
        /// <param name="tag">Tag of the Coroutine</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Coroutine StartTrackedCoroutine<T>(IEnumerator routine, T obj, string tag = "Default")
        {
            if(_coroutines == null)
                _coroutines = new Dictionary<string, List<CoroutineObjectPair>>();
            // if this tag does not exist yet, create an entry for it
            if (!_coroutines.ContainsKey(tag))
                _coroutines.Add(tag, new List<CoroutineObjectPair>());


            // create the pair and add it to the list
            CoroutineObjectPair<T> cop = new CoroutineObjectPair<T>(null, obj);
            _coroutines[tag].Add(cop);

            // start the coroutine
            Coroutine co = Instance.StartCoroutine(Instance.TrackRoutine<T>(routine, cop, tag));

            // return the tracked routine
            return co;
        }

        /// <summary>
        /// If there is a tracked coroutine with the provided Tag on the provided object, it will be stopped
        /// </summary>
        /// <param name="obj">The Object that is affected by the Coroutine</param>
        /// <param name="tag">Tag of the Coroutine</param>
        /// <typeparam name="T"></typeparam>
        public static void StopTrackedCoroutine<T>(T obj, string tag)
        {
            // if there are no coroutines with the provided tag, do nothing
            if (!_coroutines.ContainsKey(tag) || _coroutines[tag].Count == 0)
                return;

            // go through all tracked coroutines with this tag and find the proper one
            foreach (CoroutineObjectPair pair in _coroutines[tag])
            {
                // try to cast the pair
                CoroutineObjectPair<T> p = pair as CoroutineObjectPair<T>;

                // if the cast was successfull and the Object in this pair is the provided object, we need to stop the coroutine and remove it from our list
                if (p != null && p.Obj.Equals(obj))
                {
                    Instance.StopCoroutine(p.Routine);
                    _coroutines[tag].Remove(pair);
                    return;
                }
            }
        }

        /// <summary>
        /// Stops the provided Coroutine if it is tracked
        /// </summary>
        /// <param name="routine">The Coroutine to stop</param>
        /// <param name="tag">The tag of the Coroutine</param>
        public static void StopTrackedCoroutine(Coroutine routine, string tag)
        {
            // if there are no coroutines with the provided tag, do nothing
            if (!_coroutines.ContainsKey(tag) || _coroutines[tag].Count == 0)
                return;

            // go through all tracked coroutines with this tag and find the proper one
            foreach (CoroutineObjectPair pair in _coroutines[tag])
            {
                // if the contained routine is the one we alre looking for, we need to stop it and remove it from our list
                if (pair.Routine == routine)
                {
                    Instance.StopCoroutine(routine);
                    _coroutines[tag].Remove(pair);
                    return;
                }
            }
        }

        /// <summary>
        /// Stops all Tracked Coroutines with the provided tag on the provided Object
        /// </summary>
        /// <param name="obj">he Object that is affected by the Coroutine</param>
        /// <param name="tag">Tag of the Coroutine</param>
        /// <typeparam name="T"></typeparam>
        public static void StopAllTrackedCoroutines<T>(T obj, string tag)
        {
            // if there are no coroutines with the provided tag, do nothing
            if (!_coroutines.ContainsKey(tag) || _coroutines[tag].Count == 0)
                return;

            // in this list we save which routines need to be removed from our list
            List<CoroutineObjectPair> toRemove = new List<CoroutineObjectPair>();

            // go through all tracked coroutines with this tag and find the proper one
            foreach (CoroutineObjectPair pair in _coroutines[tag])
            {
                // try to cast the pair
                CoroutineObjectPair<T> p = pair as CoroutineObjectPair<T>;

                // if the cast was successfull and the Object in this pair is the provided object, we need to stop the coroutine and remember it to remove it from our list
                if (p != null && p.Obj.Equals(obj))
                {
                    Instance.StopCoroutine(p.Routine);
                    toRemove.Add(pair);
                }
            }

            // remove everything from the list
            foreach (CoroutineObjectPair pair in toRemove)
            {
                _coroutines[tag].Remove(pair);
            }

            toRemove = null;
        }

        /// <summary>
        /// Stops all tracked coroutines with the provided tag on all objects
        /// </summary>
        /// <param name="tag"></param>
        public static void StopAllTrackedCoroutines(string tag)
        {
            // if there are no coroutines with the provided tag, do nothing
            if (!_coroutines.ContainsKey(tag) || _coroutines[tag].Count == 0)
                return;
            
            // go through all tracked coroutines with this tag and find the proper one
            foreach (CoroutineObjectPair pair in _coroutines[tag])
            {
                Instance.StopCoroutine(pair.Routine);
            }

            _coroutines.Remove(tag);
        }

        // ######################## COROUTINES ######################## //
        /// <summary>
        /// This is a wrapper coroutine for a tracked coroutine. It removes the routine from the list as soon as it finished
        /// </summary>
        /// <param name="routine">The Coroutine to run</param>
        /// <param name="cop">The CoroutineObjectPair that is associated to this Coroutine</param>
        /// <param name="t">The Tag of the Coroutine</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IEnumerator TrackRoutine<T>(IEnumerator routine, CoroutineObjectPair cop, string t)
        {
            Coroutine co = StartCoroutine(routine);
            cop.Routine = co;

            yield return co;

            _coroutines[t].Remove(cop);
        }
    }
}

#if UNITY_EDITOR
/// <summary>
/// <para>A Custom Editor for the Coroutine Host that displays Information about the currently tracked coroutines</para>
///
/// v1.0 09/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CustomEditor(typeof(CoroutineHost))]
public class CoroutineHostEditor : Editor
{
    public override void OnInspectorGUI()
    {
        float totalCoroutines = 0;
        foreach (KeyValuePair<string, List<CoroutineHost.CoroutineObjectPair>> pair in CoroutineHost.Coroutines)
        {
            EditorGUILayout.LabelField(pair.Key, EditorStyles.boldLabel);

            ++EditorGUI.indentLevel;
            foreach (CoroutineHost.CoroutineObjectPair coroutineObjectPair in pair.Value)
            {
                EditorGUILayout.LabelField(coroutineObjectPair.ObjName);
                ++totalCoroutines;
            }

            --EditorGUI.indentLevel;
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField($"{totalCoroutines} Coroutines Tracked", EditorStyles.boldLabel);

        Repaint();
    }
}
#endif