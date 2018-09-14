// remove this line if you don't have TextMeshPro in your Project

#define TEXT_MESH_PRO


#if TEXT_MESH_PRO
using System.Collections;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace FK.Language
{
    /// <summary>
    /// <para>An extension of the TextMeshPro Text that gets its text from the Language Manager and changes its text automatically whenever the language changes</para>
    ///
    /// v1.1 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [RequireComponent(typeof(LanguageTextConfig))]
    public class TextMeshProLanguageText : TextMeshPro
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
        [MenuItem("GameObject/3D Object/TextMeshPro - Language Text")]
        private static void CreateLanguageText(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("TextMeshPro LanguageText", typeof(RectTransform), typeof(LanguageTextConfig), typeof(TextMeshProLanguageText));
            TextMeshProLanguageText lt = go.GetComponent<TextMeshProLanguageText>();
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
#endif