// comment this line out if you don't have TextMeshPro in your Project
#define TEXT_MESH_PRO


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if TEXT_MESH_PRO
using TMPro;
#endif

#if UNITY_EDITOR
using UnityEditor;

#endif

namespace FK.Language
{
    /// <summary>
    /// <para>This Component makes a Legacy or TMP Text a Language Dependent Text that automatically loads the correct string and updates if the Language is changed</para>
    ///
    /// v2.0 01/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [ExecuteInEditMode]
    public class LanguageTextConfig : MonoBehaviour
    {
        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// Name of the text in the strings file
        /// </summary>
        public string Name = "default";

        /// <summary>
        /// Category of the text in the strings file
        /// </summary>
        public string Category = LanguageManager.DEFAULT_CATEGORY;

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// Is there any text attached to this game object?
        /// </summary>
        private bool _hasText;

        /// <summary>
        /// Are we using the legacy text?
        /// </summary>
        private bool _useLegacy;

        /// <summary>
        /// The Legacy Text
        /// </summary>
        private Text _text;

#if TEXT_MESH_PRO
        /// <summary>
        /// The TMP Text
        /// </summary>
        private TMP_Text _tmpText;
#endif

        // ######################## UNITY EVENT FUNCTIONS ######################## //
        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
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
            // try to get the legacy text
            _text = GetComponent<Text>();

            // if there is a legacy text, use it
            if (_text)
            {
                _hasText = true;
                _useLegacy = true;
                _text.supportRichText = true;
            }
#if TEXT_MESH_PRO
            // if there is no legacy text, try to get a TMP Text
            else
            {
                _tmpText = GetComponent<TMP_Text>();

                _hasText = _tmpText != null;
            }
#endif

            // if there is no text at all, don't continue
            if (!_hasText)
                return;

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
                if (!string.IsNullOrEmpty(Name))
                    gameObject.name = Name;

                if (_useLegacy)
                {
                    if (!_text.text.StartsWith("<"))
                        _text.text = $"<{_text.text}>";
                }
#if TEXT_MESH_PRO
                else
                {
                    if (!_tmpText.text.StartsWith("<"))
                        _tmpText.text = $"<{_tmpText.text}>";
                }
#endif
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
            GameObject go = new GameObject("LanguageText", typeof(RectTransform), typeof(LanguageTextConfig), typeof(Text));
            Text text = go.GetComponent<Text>();
            text.text = "New Language Text";

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

#if TEXT_MESH_PRO
        /// <summary>
        /// Creates a TMPUGUI Language Text via the Create Object Menu
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("GameObject/UI/TextMeshPro - Language Text")]
        private static void CreateLanguageTextTMPUGUI(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("TextMeshPro LanguageText", typeof(RectTransform), typeof(LanguageTextConfig), typeof(TextMeshProUGUI));
            TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
            text.text = "New Language Text";

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        /// <summary>
        /// Creates a TMP Language Text via the Create Object Menu
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("GameObject/3D Object/TextMeshPro - Language Text")]
        private static void CreateLanguageTextTMP(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("TextMeshPro LanguageText", typeof(RectTransform), typeof(LanguageTextConfig), typeof(TextMeshPro));
            TextMeshPro text = go.GetComponent<TextMeshPro>();
            text.text = "New Language Text";

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
#endif
#endif

        #endregion


        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Updates the text to contain the text of the current language
        /// </summary>
        /// <param name="newLanguage"></param>
        private void UpdateText(string newLanguage)
        {
            if (_useLegacy)
                LanguageManager.SetText(_text, Name, Category);
#if TEXT_MESH_PRO
            else
                LanguageManager.SetText(_tmpText, Name, Category);
#endif
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