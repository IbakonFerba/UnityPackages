using System.Collections;
using FK.Utility;
using UnityEngine;

namespace FK.JLoc
{
    /// <summary>
    /// <para>A component for managing the loading and unloading of a string file based on Unity event functions.</para>
    ///
    /// v1.0 02/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class StringsLoader : MonoBehaviour
    {
        // ######################## ENUMS & DELEGATES ######################## //
        public enum UnityEvents
        {
            NONE,
            AWAKE,
            START,
            ENABLE,
            DISABLE,
            DESTROY
        }


        // ######################## PUBLIC VARS ######################## //
        public UnityEvents LoadOn = UnityEvents.AWAKE;
        public UnityEvents UnloadOn = UnityEvents.DESTROY;

        public bool LoadAsync = true;
        public bool UnloadOtherLoadedStrings = false;
        public string FilePath;


        // ######################## UNITY EVENT FUNCTIONS ######################## //
        private void Awake()
        {
            if (LoadOn == UnityEvents.AWAKE)
                Load();
            if (UnloadOn == UnityEvents.AWAKE)
                Unload();
        }

        private void Start()
        {
            if (LoadOn == UnityEvents.START)
                Load();
            if (UnloadOn == UnityEvents.START)
                Unload();
        }

        private void OnEnable()
        {
            if (LoadOn == UnityEvents.ENABLE)
                Load();
            if (UnloadOn == UnityEvents.ENABLE)
                Unload();
        }

        private void OnDisable()
        {
            if (LoadOn == UnityEvents.DISABLE)
                Load();
            if (UnloadOn == UnityEvents.DISABLE)
                Unload();
        }

        private void OnDestroy()
        {
            if (LoadOn == UnityEvents.DESTROY)
                Load();
            if (UnloadOn == UnityEvents.DESTROY)
                Unload();
        }


        // ######################## FUNCTIONALITY ######################## //
        private void Load()
        {
            // We use a coroutine here so we can make sure the LanguageManager is properly initialized
            CoroutineHost.Instance.StartCoroutine(LoadStrings());
        }

        private void Unload()
        {
            if (LanguageManager.HasStrings)
                LanguageManager.UnloadStringsFile(FilePath);
            else
                Debug.LogWarning($"Trying to unload strings file {FilePath}, but no files are loaded!");
        }


        // ######################## COROUTINES ######################## //
        private IEnumerator LoadStrings()
        {
            yield return new WaitUntil(() => LanguageManager.Initialized);

            LanguageManager.LoadStringsFile(FilePath, LoadAsync, UnloadOtherLoadedStrings);
        }
    }
}