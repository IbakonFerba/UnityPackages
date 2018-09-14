#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using FK.JSON;
using FK.Editor;

namespace FK.Language
{
    /// <summary>
    /// <para>The Editor for the Language Manager. This allows the User to edit settings and strings files</para>
    ///
    /// v1.0 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class LanguageManagerEditor : EditorWindow
    {
        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// Is the Editor window focused?
        /// </summary>
        public static bool IsFocused { get; private set; }

        /// <summary>
        /// Style for strings text areas with rich text disabled
        /// </summary>
        private GUIStyle TextAreaStyle
        {
            get
            {
                if (_textAreaStyle == null)
                {
                    _textAreaStyle = new GUIStyle(EditorStyles.textArea);
                    _textAreaStyle.richText = false;
                }

                return _textAreaStyle;
            }
        }

        /// <summary>
        /// Style for category name text fields
        /// </summary>
        private GUIStyle CategoryNameStyle
        {
            get
            {
                if (_categoryNameStyle == null)
                {
                    _categoryNameStyle = new GUIStyle(EditorStyles.textField);
                    _categoryNameStyle.fontSize = 12;
                    _categoryNameStyle.normal.textColor = Color.white;
                    _categoryNameStyle.focused.textColor = Color.white;
                }

                return _categoryNameStyle;
            }
        }

        /// <summary>
        /// Style for a whole category
        /// </summary>
        private GUIStyle CategoryStyle
        {
            get
            {
                if (_categoryStyle == null)
                {
                    _categoryStyle = new GUIStyle();
                    _categoryStyle.normal.background = _categoryBackground;
                }

                return _categoryStyle;
            }
        }

        /// <summary>
        /// Style for a Button with grayed out text (used for the save button)
        /// </summary>
        private GUIStyle GrayedOutButtonStyle
        {
            get
            {
                if (_grayedOutButtonStyle == null)
                {
                    _grayedOutButtonStyle = new GUIStyle(GUI.skin.button);
                    _grayedOutButtonStyle.normal.textColor = Color.gray;
                }

                return _grayedOutButtonStyle;
            }
        }

        /// <summary>
        /// Anim Bools for Categories
        /// </summary>
        private List<AnimBool> ShowCategory
        {
            get
            {
                // setup anim bools for all categories
                if (_showCategory == null)
                {
                    _showCategory = new List<AnimBool>();
                    // we start at 1 because 0 is not a category but the language lookup
                    for (int i = 1; i < _strings.Count; ++i)
                    {
                        AnimBool ab = new AnimBool(true);
                        ab.valueChanged.AddListener(Repaint);
                        _showCategory.Add(ab);
                    }
                }

                return _showCategory;
            }
        }

        /// <summary>
        /// Anim Bool for Languages
        /// </summary>
        private AnimBool ShowLangs
        {
            get
            {
                // set up the anim bool
                if (_showLangs == null)
                {
                    _showLangs = new AnimBool(true);
                    _showLangs.valueChanged.AddListener(Repaint);
                }

                return _showLangs;
            }
        }


        // ######################## PRIVATE VARS ######################## //

        #region CONSTANTS

        /// <summary>
        /// Placeholder for newly created language strings
        /// </summary>
        private const string PLACEHOLDER_STRING = "<Empty String>";

        /// <summary>
        /// Width of square buttons
        /// </summary>
        private const float SQUARE_BUTTON_WIDTH = 20;

        /// <summary>
        /// All line break symbols
        /// </summary>
        private static readonly string[] LINE_BREAKS = {"\r\n", "\r", "\n"};

        /// <summary>
        /// Width of one cell in the display of the strings file
        /// </summary>
        private static readonly GUILayoutOption CELL_WIDTH = GUILayout.Width(300);

        /// <summary>
        /// Width for Buttons
        /// </summary>
        private static readonly GUILayoutOption BUTTON_WIDTH = GUILayout.Width(150);

        #endregion

        #region GUI

        /// <summary>
        /// The background for displaying a category
        /// </summary>
        private Texture2D _categoryBackground;


        /// <summary>
        /// Backing for TextAreaStyle Property
        /// </summary>
        private GUIStyle _textAreaStyle;

        /// <summary>
        /// Backing for CategoryNameStyle Property
        /// </summary>
        private GUIStyle _categoryNameStyle;

        /// <summary>
        /// Backing for CategoryStyle Property
        /// </summary>
        private GUIStyle _categoryStyle;

        /// <summary>
        /// Backing for GrayedOutButtonStyle Property
        /// </summary>
        private GUIStyle _grayedOutButtonStyle;


        /// <summary>
        /// AnimBools for all categorie fade groups
        /// </summary>
        private List<AnimBool> _showCategory;

        /// <summary>
        /// AnimBool for languages fade group
        /// </summary>
        private AnimBool _showLangs;


        /// <summary>
        /// Position of the scroll view
        /// </summary>
        private Vector2 _scrollPos;

        #endregion

        /// <summary>
        /// All the strings
        /// </summary>
        private JSONObject _strings;

        /// <summary>
        /// Language codes of all languages in the strings
        /// </summary>
        private string[] _langs;


        /// <summary>
        /// Language code for new language
        /// </summary>
        private string _newLangCode;

        /// <summary>
        /// Language name for new language
        /// </summary>
        private string _newLangDisplay;

        /// <summary>
        /// Are there any changes to the strings that are not saved yet?
        /// </summary>
        private bool _unsavedChanges;

        // ######################## UNITY EVENT FUNCTIONS ######################## //
        /// <summary>
        /// Displays the Editor
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Space(20);
            ShowSettings();
            GUILayout.Space(20);
            ShowStringsEditor();
        }

        private void OnFocus()
        {
            IsFocused = true;
        }

        private void OnLostFocus()
        {
            // if there are unsaved changes, display a dialog to ask the user if he wants to save
            if (_unsavedChanges)
                if (DisplaySaveDialog())
                    SaveStrings();

            IsFocused = false;
        }


        // ######################## INITS ######################## //
        /// <summary>
        /// This method is called when the Editor loads and initializes things that are not tied to the window
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Init()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        /// <summary>
        /// Opens a Language Manager Window
        /// </summary>
        [MenuItem("Window/Language Manager", false, 50)]
        public static void OpenLanguageManager()
        {
            LanguageManagerEditor window = (LanguageManagerEditor) GetWindow(typeof(LanguageManagerEditor));
            window.titleContent = new GUIContent("Language Manager");
            window.Show();
            CheckConfig();
            window.ResetValues();
        }

        /// <summary>
        /// Resets some values for the Editor window
        /// </summary>
        private void ResetValues()
        {
            _textAreaStyle = null;
            _categoryNameStyle = null;
            _categoryStyle = null;
            _grayedOutButtonStyle = null;

            _langs = null;
            _showCategory = null;
            _showLangs = null;

            _categoryBackground = EditorUtils.GetBackgroundColor(new Color(0, 0, 0, 0.2f));
            LanguageFileAutoSaver.OnSave = SaveStrings;
        }

        // ######################## FUNCTIONALITY ######################## //

        #region CONFIG_MANAGEMENT

        private static void OnSceneOpened(Scene s, OpenSceneMode mode)
        {
            SaveReference(s);
        }

        /// <summary>
        /// Saves a reference to the Language Manager config in the provided scene so the config is guaranted to make it into a build
        /// </summary>
        /// <param name="s"></param>
        private static void SaveReference(Scene s)
        {
            // look for a language manager reference instance in the scene
            LanguageManagerReferences lmr = FindObjectOfType<LanguageManagerReferences>();

            // if there is no instance, create one at the top of the hierarchy
            if (lmr == null)
            {
                lmr = new GameObject("<Language_Manager_References>", typeof(LanguageManagerReferences)).GetComponent<LanguageManagerReferences>();
                lmr.transform.SetAsFirstSibling();
                Debug.Log("Created Reference Container for Language Manager");
            }

            // if the instance we found or just created does not have a config reference set yet, try to add it
            if (lmr.Config == null)
            {
                // if there is no config, warn the user
                if (LanguageManagerConfig.Instance == null)
                {
                    Debug.LogWarning("Could not find a Language Manager Config! This should not happen, open the Language Manager to create one!");
                    return;
                }

                // set the refernce and mark the scene as dirty (we shouldn't have touched it with our dirty hands :P)
                lmr.Config = LanguageManagerConfig.Instance;
                EditorSceneManager.MarkSceneDirty(s);
            }
        }

        /// <summary>
        /// Checks if a config exists and crates one if there is none
        /// </summary>
        private static void CheckConfig()
        {
            if (LanguageManagerConfig.Instance == null)
            {
                // create a new config instance and make it an asset
                LanguageManagerConfig config = ScriptableObject.CreateInstance<LanguageManagerConfig>();
                AssetDatabase.CreateAsset(config, "Assets/LanguageManager/LanguageManagerConfig.asset");
                Debug.Log("Created Language Manager Config");

                // save a reference to the config
                SaveReference(EditorSceneManager.GetActiveScene());
            }
        }

        #endregion

        #region SAVING_LOADING

        /// <summary>
        /// Parses the opened strings and saves them to the file
        /// </summary>
        private void SaveStrings()
        {
            // first we need to replace any line break with an escaped line break (because json does not allow line breaks, it wouldwork in this case but i want to stay json compatible)
            // for this we need to go through all categories
            foreach (JSONObject category in _strings)
            {
                // now we need to go through every language string in this category and replace linebreaks with \\n
                foreach (JSONObject languageString in category)
                {
                    for (int i = 0; i < languageString.Count; ++i)
                    {
                        string s = languageString[i].StringValue;
                        for (int j = 0; j < LINE_BREAKS.Length; ++j)
                        {
                            s = s.Replace(LINE_BREAKS[j], "\\n");
                        }

                        languageString.SetField(languageString.GetKeyAt(i), s);
                    }
                }
            }

            // the strings are parsed now, save them into a file
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, LanguageManagerConfig.Instance.StringsFileName + ".json");
            _strings.SaveToFile(path);

            // load the file to get the strings back in a form that we can show the user
            LoadStrings();
        }

        /// <summary>
        /// Loads the strings from the file
        /// </summary>
        private void LoadStrings()
        {
            // if we are loading, we cannot have any unsaved changes
            _unsavedChanges = false;

            // try to load the file
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, LanguageManagerConfig.Instance.StringsFileName + ".json");
            try
            {
                _strings = JSONObject.LoadFromFile(path);
            }
            catch
            {
                // if loading failed, create new strings
                _strings = new JSONObject(JSONObject.Type.OBJECT);

                // add the default language
                JSONObject languages = new JSONObject(JSONObject.Type.OBJECT);
                languages.AddField(LanguageManagerConfig.Instance.DefaultLang, LanguageManagerConfig.Instance.DefaultLang);
                _strings.AddField(LanguageManager.LANGUAGES_KEY, languages);

                // add the default category
                AddCategory(LanguageManager.DEFAULT_CATEGORY);
            }

            // parse the strings into ther nice, formatted form
            LanguageManager.ParseStrings(_strings);

            // reset the editor
            ResetValues();
        }

        #endregion

        #region GENERATE_STRINGS

        /// <summary>
        /// Generates new strings by looking at all language texts in all scenes. This is additive, if there is a string file already, it won't override it but add to it
        /// </summary>
        private void GenerateStrings()
        {
            // if there are unsaved changes, ask the user if he wants to save them, then load the existing strings (this will create new strings if there are now existing strings)
            if (_unsavedChanges)
            {
                if (DisplaySaveDialog())
                    SaveStrings();
                else
                    LoadStrings();
            }
            else
            {
                LoadStrings();
            }

            // get all asset paths
            string[] paths = AssetDatabase.GetAllAssetPaths();

            // save the path of the current scene and save the current scene
            string startScenePath = EditorSceneManager.GetActiveScene().path;
            EditorSceneManager.SaveOpenScenes();

            // go through all paths and find the scenes
            foreach (string path in paths)
            {
                // if the path does not end with .unity, this is not a scene, look at the next path
                if (!path.EndsWith(".unity"))
                    continue;

                // we found a scene! now we need to open it and find all language text configs in it
                EditorSceneManager.OpenScene(path);
                LanguageTextConfig[] ltcs = FindObjectsOfType<LanguageTextConfig>();

                // iterate through all language text configs and check if they already have a representation in the strings file
                foreach (LanguageTextConfig ltc in ltcs)
                {
                    CheckField(ltc.Category, ltc.Name);
                }
            }

            // now re open the scene we came from
            EditorSceneManager.OpenScene(startScenePath);

            // reset the editor
            ResetValues();
        }

        /// <summary>
        /// Checks if the strings contain a field with the provided name in the provided category and adds it if it does not exist
        /// </summary>
        /// <param name="category"></param>
        /// <param name="fieldName"></param>
        private void CheckField(string category, string fieldName)
        {
            // does the category already exist? If not, create it!
            if (!_strings.HasField(category))
            {
                // create a new category and mark the file as dirty
                _strings.AddField(category, new JSONObject(JSONObject.Type.OBJECT));
                _unsavedChanges = true;
            }

            // does the filed exist im the category? If not, create it!
            if (!_strings[category].HasField(fieldName))
            {
                // create the new field and add strings for each language
                JSONObject newField = new JSONObject(JSONObject.Type.OBJECT);
                for (int i = 0; i < _strings[LanguageManager.LANGUAGES_KEY].Count; ++i)
                {
                    newField.AddField(_strings[LanguageManager.LANGUAGES_KEY].GetKeyAt(i), PLACEHOLDER_STRING);
                }

                // add the new field and mark the file as dirty
                _strings[category].AddField(fieldName, newField);
                _unsavedChanges = true;
            }
        }

        #endregion

        #region STRINGS_EDITING

        /// <summary>
        /// Adds a new language to all strings
        /// </summary>
        private void AddLanguage()
        {
            // reset our local represnetation of the laanguages
            _langs = null;

            // add the language in the languaes lookup
            _strings[LanguageManager.LANGUAGES_KEY].AddField(_newLangCode, _newLangDisplay);

            // go through all categories (0 is the languages lookup, so start at 1) and add the new language to every field
            for (int c = 1; c < _strings.Count; ++c)
            {
                JSONObject category = _strings[c];
                foreach (JSONObject languageString in category)
                {
                    languageString.AddField(_newLangCode, PLACEHOLDER_STRING);
                }
            }

            // mark as dirty
            _unsavedChanges = true;
        }

        /// <summary>
        /// Adds a new field to the given category
        /// </summary>
        /// <param name="category"></param>
        private void AddLanguageField(JSONObject category)
        {
            // create the field and add all languages
            JSONObject field = new JSONObject(JSONObject.Type.OBJECT);
            for (int i = 0; i < _langs.Length; ++i)
            {
                field.AddField(_langs[i], PLACEHOLDER_STRING);
            }

            // add the field and mark the file as dirty
            category.AddField($"Text_Field_{category.Count}", field);
            _unsavedChanges = true;
        }

        /// <summary>
        /// Adds a new Category to the strings
        /// </summary>
        /// <param name="categoryName">Optional name for the Category, if not set it will create a default name</param>
        private void AddCategory(string categoryName = null)
        {
            // create the category and add it to the strings
            JSONObject category = new JSONObject(JSONObject.Type.OBJECT);
            _strings.AddField(categoryName ?? $"Category_{_strings.Count - 1}", category);

            // add a new anim bool for the category
            AnimBool ab = new AnimBool(true);
            ab.valueChanged.AddListener(Repaint);
            ShowCategory.Add(ab);

            // mark file as dirty
            _unsavedChanges = true;
        }

        /// <summary>
        /// Duplicates an existing category
        /// </summary>
        /// <param name="category">Category to duplicate</param>
        private void DuplicateCategory(JSONObject category)
        {
            // create the category and add it to the strings
            JSONObject newCategory = new JSONObject(category);
            _strings.AddField($"Category_{_strings.Count - 1}", newCategory);

            // add a new anim bool for the category
            AnimBool ab = new AnimBool(true);
            ab.valueChanged.AddListener(Repaint);
            ShowCategory.Add(ab);

            // mark file as dirty
            _unsavedChanges = true;
        }

        /// <summary>
        /// Moves a category up or down
        /// </summary>
        /// <param name="categoryIndex">Current index of the category</param>
        /// <param name="direction">Amount to move (positive for down, negative for up)</param>
        private void MoveCategory(int categoryIndex, int direction)
        {
            // calculate the new index
            int newIndex = categoryIndex + direction;

            // check if the new index is valid. We cant move further up than 2 because 1 is the default category that should always stay at the top and 0 is the Language Lookup.
            // Of course we cant move farther down than the max number of categories
            if (newIndex > 1 && newIndex < _strings.Count)
            {
                // move the category
                _strings.MoveField(categoryIndex, newIndex);

                // move the anim bool
                if (newIndex < categoryIndex)
                {
                    ShowCategory.Insert(newIndex - 1, ShowCategory[categoryIndex - 1]);
                    ShowCategory.RemoveAt(categoryIndex);
                }
                else
                {
                    ShowCategory.Insert(newIndex, ShowCategory[categoryIndex - 1]);
                    ShowCategory.RemoveAt(categoryIndex - 1);
                }

                // mark file as dirty
                _unsavedChanges = true;
            }
        }

        /// <summary>
        /// Moves a language field up or down
        /// </summary>
        /// <param name="category">Category containing the filed</param>
        /// <param name="fieldIndex">Current index of the category</param>
        /// <param name="direction">Amount to move (positive for down, negative for up)</param>
        private void MoveLanguageField(JSONObject category, int fieldIndex, int direction)
        {
            // calculate the new index
            int newIndex = fieldIndex + direction;

            // check if the new index is valid
            if (newIndex >= 0 && newIndex < category.Count)
            {
                // move field and mark file as dirty
                category.MoveField(fieldIndex, newIndex);
                _unsavedChanges = true;
            }
        }

        /// <summary>
        /// Renames a language code in all strings
        /// </summary>
        /// <param name="oldCode"></param>
        /// <param name="newCode"></param>
        private void RenameLanguage(string oldCode, string newCode)
        {
            // reset our local representation of the languages
            _langs = null;

            // go through all categories
            for (int c = 1; c < _strings.Count; ++c)
            {
                JSONObject category = _strings[c];

                // rename the language field in the strings
                foreach (JSONObject languageString in category)
                {
                    languageString.RenameField(oldCode, newCode);
                }
            }

            // rename the language in the lookup and mark file as dirty
            _strings[LanguageManager.LANGUAGES_KEY].RenameField(oldCode, newCode);
            _unsavedChanges = true;
        }

        /// <summary>
        /// Removes a language from all strings
        /// </summary>
        /// <param name="lang"></param>
        private void RemoveLanguage(string lang)
        {
            // reset our local representation of the languages
            _langs = null;

            // go through all categories
            for (int c = 1; c < _strings.Count; ++c)
            {
                JSONObject category = _strings[c];

                // remove the language field from the strings
                foreach (JSONObject languageString in category)
                {
                    languageString.RemoveField(lang);
                }
            }

            // remove the language from the lookup and mark file as dirty
            _strings[LanguageManager.LANGUAGES_KEY].RemoveField(lang);
            _unsavedChanges = true;
        }

        #endregion


        // ######################## GUI ######################## //

        #region GUI

        /// <summary>
        /// Shows the settings for the Language Manager
        /// </summary>
        private void ShowSettings()
        {
            // Headline
            EditorGUILayout.LabelField("Settings", EditorStyles.whiteLargeLabel, GUILayout.Height(20));

            // draw settings
            EditorGUI.BeginChangeCheck();
            bool loadAsync =
                EditorGUILayout.ToggleLeft(
                    new GUIContent("Load strings asynchronously",
                        "If set, the Language Manager will load the strings asynchronously without blocking the Main Thread"),
                    LanguageManagerConfig.Instance.LoadStringsAsync);

            string stringsFileName = EditorGUILayout.TextField(new GUIContent("Strings File Name", "Name of the JSON file containing all strings in the Streaming Assets  (without file ending)"),
                LanguageManagerConfig.Instance.StringsFileName);

            EditorGUILayout.Space();

            bool useSystemLanguage =
                EditorGUILayout.ToggleLeft(
                    new GUIContent("Use saved language on Application start",
                        "If set, the Language Manager tries to load the language that was last used from the Player Prefs. If there is none or if the strings file does not contain the language, it will go through the other options after this"),
                    LanguageManagerConfig.Instance.UseSystemLanguageAsDefault);

            bool useSavedLanguage =
                EditorGUILayout.ToggleLeft(
                    new GUIContent("Use system language as default",
                        "If set, the Language Manager uses the Language of the System of the user as the default language, if the strings file contains this language"),
                    LanguageManagerConfig.Instance.UseSystemLanguageAsDefault);

            string defaultLanguage =
                EditorGUILayout.TextField(new GUIContent("Default Language", "Language Code of the Language to use as a default or as a fallback if the system language is not supported"),
                    LanguageManagerConfig.Instance.DefaultLang);


            // set settings values
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(LanguageManagerConfig.Instance, "Changed Language Manager settings");
                LanguageManagerConfig.Instance.LoadStringsAsync = loadAsync;
                LanguageManagerConfig.Instance.StringsFileName = stringsFileName;
                LanguageManagerConfig.Instance.UseSystemLanguageAsDefault = useSystemLanguage;
                LanguageManagerConfig.Instance.UseSavedLanguage = useSavedLanguage;
                LanguageManagerConfig.Instance.DefaultLang = defaultLanguage;

                // delete language key in player prefs so testing is easier
                PlayerPrefs.DeleteKey("Lang");

                // set config dirty
                EditorUtility.SetDirty(LanguageManagerConfig.Instance);
            }
        }

        /// <summary>
        /// Displays an editor for editing the strings
        /// </summary>
        private void ShowStringsEditor()
        {
            ShowActionButtons();

            // don't continue if no strings are loaded
            if (_strings == null)
                return;

            GUILayout.Space(20);

            ShowLanguages();
            if (_langs == null || _langs.Length == 0)
                _langs = _strings[LanguageManager.LANGUAGES_KEY].Keys;

            GUILayout.Space(20);

            // display all the categories in a scroll view
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            ShowCategories();
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Shows Buttons for all the actions concerning the strings editor
        /// </summary>
        private void ShowActionButtons()
        {
            // generate strings
            if (GUILayout.Button(
                new GUIContent("Generate Strings",
                    "Looks at all Language Texts in all scenes of this Project and generates fields for them in the strings file. If no file exists, it creates a new one"), BUTTON_WIDTH))
                GenerateStrings();


            EditorGUILayout.Space();


            EditorGUILayout.BeginHorizontal();
            // If pressed, this button opens a Text Preview window that displays formatted version of the text currently edited
            if (GUILayout.Button(new GUIContent("Text Preview", "Opens a window that shows a rich text formatted version of the string currently edited as it will appear in the language texts"),
                BUTTON_WIDTH))
                TextPreviewWindow.OpenTextPreviewWindow();


            // load strings
            if (GUILayout.Button("Load Strings", BUTTON_WIDTH))
            {
                // if there are unsaved changes, ask the user if he wants to save them
                if (_unsavedChanges)
                {
                    if (DisplaySaveDialog())
                        SaveStrings();
                    else
                        LoadStrings();
                }
                else
                {
                    LoadStrings();
                }
            }

            // if no strings are loaded, stop here
            if (_strings == null)
            {
                EditorGUILayout.EndHorizontal();
                return;
            }

            // if strings are loaded, display a save button that is grayed out if there are no unsaved changes
            if (GUILayout.Button("Save Strings", !_unsavedChanges ? GrayedOutButtonStyle : GUI.skin.button, BUTTON_WIDTH))
                SaveStrings();
            EditorGUILayout.EndHorizontal();
        }

        #region LANGUAGES

        /// <summary>
        /// Show the edit area for the supported Languages
        /// </summary>
        private void ShowLanguages()
        {
            EditorGUILayout.BeginHorizontal();

            // display a button for folding the languages in or out
            if (!ShowLangs.target)
                ShowLangs.target = GUILayout.Button(">", GUILayout.Width(SQUARE_BUTTON_WIDTH));
            else
                ShowLangs.target = !GUILayout.Button("v", GUILayout.Width(SQUARE_BUTTON_WIDTH));

            // headline
            EditorGUILayout.LabelField("Languages", EditorStyles.whiteLargeLabel, CELL_WIDTH, GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();


            // display all languages
            if (EditorGUILayout.BeginFadeGroup(ShowLangs.faded))
            {
                EditorGUILayout.BeginHorizontal();
                // add an indent
                GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal);

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                // add an indent
                GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal);

                // header
                EditorGUILayout.LabelField("Code", BUTTON_WIDTH);
                EditorGUILayout.LabelField("Display Name", BUTTON_WIDTH);
                EditorGUILayout.EndHorizontal();

                // get the languages from the strings
                JSONObject languages = _strings[LanguageManager.LANGUAGES_KEY];

                // create a list for storing which languages should be removed after this loop
                List<string> toRemove = new List<string>();

                // go through all languages and display them
                for (int i = 0; i < languages.Count; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    // button for removing the language
                    if (GUILayout.Button("X", GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                        toRemove.Add(languages.GetKeyAt(i));

                    // language code
                    EditorGUI.BeginChangeCheck();
                    string langCode = EditorGUILayout.TextField(languages.GetKeyAt(i), BUTTON_WIDTH);
                    if (EditorGUI.EndChangeCheck())
                        RenameLanguage(languages.GetKeyAt(i), langCode);

                    // display name
                    EditorGUI.BeginChangeCheck();
                    string langName = EditorGUILayout.TextField(languages[i].StringValue, BUTTON_WIDTH);
                    if (EditorGUI.EndChangeCheck())
                    {
                        languages.SetField(langCode, langName);
                        _unsavedChanges = true;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                // remove requested languages
                foreach (string lang in toRemove)
                {
                    RemoveLanguage(lang);
                }

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                // add indent
                GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal);

                // display fields for a new language
                _newLangCode = EditorGUILayout.TextField(_newLangCode, BUTTON_WIDTH);
                _newLangDisplay = EditorGUILayout.TextField(_newLangDisplay, BUTTON_WIDTH);

                // a button to add the new language
                if (GUILayout.Button("Add Language", BUTTON_WIDTH))
                    AddLanguage();

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndFadeGroup();
        }

        #endregion

        #region CATEGORY

        /// <summary>
        /// Displays all the categories
        /// </summary>
        private void ShowCategories()
        {
            // create aa list to store which categories to remove
            List<int> categoriesToRemove = new List<int>();

            // go though all categories and display them with all their fields. We start at 1 because 0 is not a category but the language lookup
            for (int c = 1; c < _strings.Count; ++c)
            {
                // create a coloured area for the category
                EditorGUILayout.BeginVertical(CategoryStyle);

                // get the category from the strings
                JSONObject categoryObj = _strings[c];

                // show all actions for the categories. If this function returns false, it means the category should be deleted and we don't need to show the rest of it in this frame
                if (!ShowCategoryActions(c))
                {
                    categoriesToRemove.Add(c);
                    continue;
                }

                // display the contents of the category inside a fade group so it can be retracted
                if (EditorGUILayout.BeginFadeGroup(ShowCategory[c - 1].faded))
                {
                    EditorGUILayout.BeginHorizontal();
                    // add a tripple indent
                    GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal / 2f);
                    GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal / 2f);
                    GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal / 2f);

                    EditorGUILayout.BeginVertical();
                    // display a header
                    ShowHeader();

                    // create a list to store which fields to remove from the category
                    List<int> fieldsToRemove = new List<int>();

                    // display all fields
                    for (int i = 0; i < categoryObj.Count; ++i)
                    {
                        // if the display function for the field returns false, it means it should be deleted
                        if (!ShowLanguageField(categoryObj, i))
                        {
                            fieldsToRemove.Add(i);
                        }
                    }

                    // remove requested fields and marke file as dirty
                    foreach (int i in fieldsToRemove)
                    {
                        categoryObj.RemoveAt(i);
                        _unsavedChanges = true;
                    }

                    // show a footer for the category
                    ShowFooter(categoryObj);

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.EndVertical();

                GUILayout.Space(50);
            }

            // remove the requested categories and mark the file as dirty
            foreach (int i in categoriesToRemove)
            {
                _strings.RemoveAt(i);
                _unsavedChanges = true;
            }

            // a button to add a new category
            if (GUILayout.Button("Add Category", BUTTON_WIDTH))
                AddCategory();

            GUILayout.Space(50);
        }

        /// <summary>
        /// Displays actions for a category. Returns false if the category should be deleted
        /// </summary>
        /// <param name="categoryIndex"></param>
        /// <returns></returns>
        private bool ShowCategoryActions(int categoryIndex)
        {
            // initialize the return value as true
            bool ret = true;

            EditorGUILayout.BeginHorizontal();
            // Display buttons to move the categroy up or down. If we are in the first category, we are in the default category. This category cannot be moved, so just display spaces so the intent is the same
            if (categoryIndex == 1)
            {
                GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal);
                GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal);
            }
            else
            {
                if (GUILayout.Button("↓", GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                    MoveCategory(categoryIndex, 1);

                if (GUILayout.Button("↑", GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                    MoveCategory(categoryIndex, -1);
            }

            // show a button to extand and retract the category
            if (!ShowCategory[categoryIndex - 1].target)
                ShowCategory[categoryIndex - 1].target = GUILayout.Button(">", GUILayout.Width(SQUARE_BUTTON_WIDTH));
            else
                ShowCategory[categoryIndex - 1].target = !GUILayout.Button("v", GUILayout.Width(SQUARE_BUTTON_WIDTH));

            // display the name of the category. If we are in the first category, we are in the default category. This category cannot be renamed, so we just display a label
            if (categoryIndex == 1)
            {
                EditorGUILayout.LabelField(_strings.GetKeyAt(categoryIndex), EditorStyles.whiteLargeLabel, CELL_WIDTH, GUILayout.Height(20));
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                string category = EditorGUILayout.TextField(_strings.GetKeyAt(categoryIndex), CategoryNameStyle, CELL_WIDTH, GUILayout.Height(20));
                if (EditorGUI.EndChangeCheck())
                {
                    _strings.RenameField(_strings.GetKeyAt(categoryIndex), category);
                    _unsavedChanges = true;
                }
            }

            // duplicate category
            if (GUILayout.Button("Duplicate", BUTTON_WIDTH))
                DuplicateCategory(_strings[categoryIndex]);

            // if we are not in the default category, show a button to delete the category
            if (categoryIndex > 1)
            {
                if (GUILayout.Button("Remove", BUTTON_WIDTH))
                    ret = false;
            }

            EditorGUILayout.EndHorizontal();
            return ret;
        }

        /// <summary>
        /// Displays a header for the category contents
        /// </summary>
        private void ShowHeader()
        {
            EditorGUILayout.BeginHorizontal();
            // Add tripple indent
            GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal / 2f);
            GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal / 2f);
            GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal / 2f);

            // display labels for string name and all languages
            EditorGUILayout.LabelField("String Name", EditorStyles.whiteLargeLabel, GUILayout.Height(20), CELL_WIDTH);
            for (int i = 0; i < _langs.Length; ++i)
            {
                EditorGUILayout.LabelField(_langs[i], EditorStyles.whiteLargeLabel, GUILayout.Height(20), CELL_WIDTH);
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Displays a footer for the category
        /// </summary>
        /// <param name="categoryObj"></param>
        private void ShowFooter(JSONObject categoryObj)
        {
            EditorGUILayout.BeginHorizontal();
            // add tripple indent
            GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal / 2f);
            GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal / 2f);
            GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal / 2f);

            // add one field
            if (GUILayout.Button("+", BUTTON_WIDTH))
                AddLanguageField(categoryObj);

            // add 5 fields
            if (GUILayout.Button("+5", BUTTON_WIDTH))
            {
                for (int i = 0; i < 5; ++i)
                    AddLanguageField(categoryObj);
            }

            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region LANGUAGE_FIELD

        /// <summary>
        /// Dsiplays a language field with its name and all strings. Returns false if the Field should be deleted
        /// </summary>
        /// <param name="categoryObj">The category the field belongs to</param>
        /// <param name="fieldIndex">Index of the field</param>
        /// <returns></returns>
        private bool ShowLanguageField(JSONObject categoryObj, int fieldIndex)
        {
            // init return value
            bool ret = true;

            EditorGUILayout.BeginHorizontal();

            // display buttons to move the field up or down
            if (GUILayout.Button("↓", GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                MoveLanguageField(categoryObj, fieldIndex, 1);
            if (GUILayout.Button("↑", GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                MoveLanguageField(categoryObj, fieldIndex, -1);

            // delete the field
            if (GUILayout.Button("X", GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                ret = false;

            // display field name
            EditorGUI.BeginChangeCheck();
            string fieldName = EditorGUILayout.TextField(categoryObj.GetKeyAt(fieldIndex), CELL_WIDTH);
            if (EditorGUI.EndChangeCheck())
            {
                categoryObj.RenameField(categoryObj.GetKeyAt(fieldIndex), fieldName);
                _unsavedChanges = true;
            }

            // show strings for all languages
            for (int j = 0; j < _langs.Length; ++j)
            {
                ShowLanguageString(categoryObj, fieldIndex, j);
            }

            EditorGUILayout.EndHorizontal();
            return ret;
        }

        /// <summary>
        /// Shows one language string of a language field
        /// </summary>
        /// <param name="categoryObj">The category the field belongs to</param>
        /// <param name="fieldIndex">Index of the field</param>
        /// <param name="stringIndex">Index of the string</param>
        private void ShowLanguageString(JSONObject categoryObj, int fieldIndex, int stringIndex)
        {
            EditorGUI.BeginChangeCheck();

            // create a unique name for the field so we can find out whether it is focused
            string fieldName = $"{fieldIndex}{stringIndex}string{GUIUtility.GetControlID(FocusType.Keyboard)}";

            // draw a field for the string and give it the name
            GUI.SetNextControlName(fieldName);
            string text = EditorGUILayout.TextArea(categoryObj[fieldIndex][_langs[stringIndex]].StringValue, TextAreaStyle, CELL_WIDTH);

            // if the field we just drew is focused, set the text of the text preview window to this string
            if (GUI.GetNameOfFocusedControl() == fieldName)
                TextPreviewWindow.Text = text;

            // save the value and mark file as dirty
            if (EditorGUI.EndChangeCheck())
            {
                categoryObj[fieldIndex].SetField(_langs[stringIndex], text);
                _unsavedChanges = true;
            }
        }

        #endregion

        /// <summary>
        /// Opens a dialog that asks the user if he wants to save. Returns true if he clicks on yes and false if he clicks on no
        /// </summary>
        /// <returns></returns>
        private bool DisplaySaveDialog()
        {
            return EditorUtility.DisplayDialog("Unsaved Changes", "There are unsaved changes in the strings, do you want to save them to the file?", "Save", "Don't Save");
        }

        #endregion
    }

    /// <summary>
    /// <para>This is an asset modification processor that manages saving the strings whenever the unity editor saves.</para>
    /// <para>This allows the user to press ctrl+s to save the strings and saves them when the project is saved or the editor is closed while he is working on the strings</para>
    ///
    /// v1.0 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class LanguageFileAutoSaver : UnityEditor.AssetModificationProcessor
    {
        public delegate void OnSaveCallback();

        /// <summary>
        /// This callback is invoked whenever the unity editor saves
        /// </summary>
        public static OnSaveCallback OnSave;

        private static string[] OnWillSaveAssets(string[] paths)
        {
            // if the user is working in the language manager editor, invoke the callback
            if (LanguageManagerEditor.IsFocused)
            {
                OnSave?.Invoke();
            }

            return paths;
        }
    }
}
#endif