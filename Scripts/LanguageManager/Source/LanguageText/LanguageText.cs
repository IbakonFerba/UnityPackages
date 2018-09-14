using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace FK.Language
{
    /// <summary>
    /// <para>An extension of the Legacy Text that gets its text from the Language Manager and changes its text automatically whenever the language changes</para>
    ///
    /// v1.1 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [RequireComponent(typeof(LanguageTextConfig))]
    public class LanguageText : Text
    {
        // ######################## PRIVATE VARS ######################## //
        private LanguageTextConfig _config;

        // ######################## UNITY EVENT FUNCTIONS ######################## //
        protected override void Start()
        {
            base.Start();
            Init();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
#endif
                LanguageManager.OnLanguageChanged -= UpdateText;
        }


        // ######################## INITS ######################## //
        ///<summary>
        /// Does the Init for this Behaviour
        ///</summary>
        private void Init()
        {
            _config = GetComponent<LanguageTextConfig>();
            supportRichText = true;
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
#endif
                // register for language changes and set initial text
                LanguageManager.OnLanguageChanged += UpdateText;
                StartCoroutine(SetInitialText());
#if UNITY_EDITOR
            }
            else // if we are in edit mode, set the name of the object and make visible that the text that is displayed now is not the final text
            {
                if (!string.IsNullOrEmpty(_config.Name))
                    name = _config.Name;

                if (!text.StartsWith("<"))
                    text = $"<{text}>";
            }
#endif
        }

        #region EDITOR

#if UNITY_EDITOR
        /// <summary>
        /// Creates a Language Text via the Create Object Menu
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("GameObject/UI/Language Text")]
        private static void CreateLanguageText(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("LanguageText", typeof(RectTransform), typeof(LanguageTextConfig), typeof(LanguageText));
            LanguageText lt = go.GetComponent<LanguageText>();
            lt.text = "New Language Text";

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
#endif

        #endregion


        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Updates the text to contain the text of the current language
        /// </summary>
        /// <param name="newLanguage"></param>
        private void UpdateText(string newLanguage)
        {
            LanguageManager.SetText(this, _config.Name, _config.Category);
        }

        // ######################## COROUTINES ######################## //
        /// <summary>
        /// Waits until the Language Manager is initialized before updating the text
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetInitialText()
        {
            yield return new WaitUntil(() => LanguageManager.Initialized);
            UpdateText(LanguageManager.CurrentLanguage);
        }
    }
}