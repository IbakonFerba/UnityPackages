﻿#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using FK.JSON;
using FK.Editor;

namespace FK.JLoc
{
    /// <summary>
    /// <para>The Editor for the Language Manager and strings files. This allows the User to edit settings and strings files</para>
    ///
    /// v3.0 02/2019
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
        /// path to the Config File
        /// </summary>
        private static string ConfigFilePath => System.IO.Path.Combine(Application.streamingAssetsPath, LanguageManager.CONFIG_NAME + ".json");

        /// <summary>
        /// The Config Data
        /// </summary>
        private JSONObject Config
        {
            get
            {
                if (_config == null)
                    LoadConfig();

                return _config;
            }
        }

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
                    _categoryStyle = new GUIStyle(EditorStyles.helpBox);
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

        private GUIStyle FileButtonStyle
        {
            get
            {
                _fileButtonStyle = null;
                if (_fileButtonStyle == null)
                {
                    _fileButtonStyle = new GUIStyle(GUI.skin.button);
                    _fileButtonStyle.alignment = TextAnchor.MiddleLeft;
                }

                return _fileButtonStyle;
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
                    for (int i = 0; i < _strings.Count; ++i)
                    {
                        AnimBool ab = new AnimBool(false);
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
                    _showLangs = new AnimBool(false);
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

        /// <summary>
        /// Initial space in the group of the buttons before the categories and language fields
        /// </summary>
        private static readonly float VERTICAL_BUTTONS_INITIAL_SPACE = 3;

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

        private GUIStyle _fileButtonStyle;


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
        /// Backing for Config
        /// </summary>
        private static JSONObject _config;

        /// <summary>
        /// Language codes of all languages in the strings
        /// </summary>
        private List<string> _langs;


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

        /// <summary>
        /// Absolute path of the currently edited strings file
        /// </summary>
        private string _currentlyEditingFilePath;

        /// <summary>
        /// Path of the currently edited strings file relative to the Streaming Assets folder
        /// </summary>
        private string _currentlyEditingStreamingAssetsFilePath;

        // ######################## UNITY EVENT FUNCTIONS ######################## //
        /// <summary>
        /// Displays the Editor
        /// </summary>
        private void OnGUI()
        {
            // Do nothing if in playmode
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox(new GUIContent("Cannot Edit Strings while in Playmode"));
                GUI.enabled = false;
            }

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
            IsFocused = false;
        }


        // ######################## INITS ######################## //
        /// <summary>
        /// Opens a Language Manager Window
        /// </summary>
        [MenuItem("Window/JLoc Editor", false, 50)]
        public static void OpenLanguageManager()
        {
            LanguageManagerEditor window = (LanguageManagerEditor) GetWindow(typeof(LanguageManagerEditor));
            window.titleContent = new GUIContent("JLoc Editor");
            window.Show();
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

        /// <summary>
        /// Loads the Config
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                _config = JSONObject.LoadFromFile(ConfigFilePath);
            }
            catch
            {
                // if loading failed, create new config
                _config = new JSONObject(JSONObject.Type.OBJECT);

                _config[LanguageManager.CONFIG_USE_SYSTEM_LANG_DEFAULT_KEY].BoolValue = true;
                _config[LanguageManager.CONFIG_DEFAULT_LANG_KEY].StringValue = "en";
                _config[LanguageManager.CONFIG_USE_SAVED_LANG_KEY].BoolValue = true;

                // add the default language
                JSONObject languages = new JSONObject(JSONObject.Type.OBJECT);
                languages.AddField(_config[LanguageManager.CONFIG_DEFAULT_LANG_KEY].StringValue, _config[LanguageManager.CONFIG_DEFAULT_LANG_KEY].StringValue);
                _config.AddField(LanguageManager.LANGUAGES_KEY, languages);

                // make sure streaming assets exists
                if (!AssetDatabase.IsValidFolder("Assets/StreamingAssets"))
                {
                    AssetDatabase.CreateFolder("Assets", "StreamingAssets");

                    AssetDatabase.Refresh();
                }

                _config.SaveToFile(ConfigFilePath);
            }
        }

        /// <summary>
        /// Checks out the config from version control
        /// </summary>
        private void CheckoutConfig()
        {
            // if version Control is active, check out the file automatically if it is not yet checked out
            if (Provider.isActive)
            {
                // Get the version control asset of the file
                Asset configFile = Provider.GetAssetByPath($"Assets/StreamingAssets/{LanguageManager.CONFIG_NAME}.json");

                // checkout the file if it is not checked out yet and wait until it is checked out
                if (!Provider.IsOpenForEdit(configFile))
                    Provider.Checkout(configFile, CheckoutMode.Asset).Wait();
            }
        }

        #endregion

        #region SAVING_LOADING

        /// <summary>
        /// Parses the opened strings and saves them to the file
        /// </summary>
        private void SaveStrings()
        {
            // create a copy of the strings because we have to change them a little before saving and we don't want the user to see these changes
            JSONObject processedStrings = new JSONObject(_strings);

            // first we need to replace any line break with an escaped line break (because json does not allow line breaks, it wouldwork in this case but i want to stay json compatible)
            // for this we need to go through all categories
            foreach (JSONObject category in processedStrings)
            {
                // now we need to go through every language string in this category and replace linebreaks with \\n
                foreach (JSONObject languageString in category)
                {
                    for (int i = 0; i < languageString.Count; ++i)
                    {
                        string s = languageString[i].StringValue;

                        // rescape escape characters and quotation marks
                        s = s.Replace("\\", "\\\\").Replace("\"", "\\\"");

                        for (int j = 0; j < LINE_BREAKS.Length; ++j)
                        {
                            s = s.Replace(LINE_BREAKS[j], "\\n");
                        }


                        languageString.SetField(languageString.GetKeyAt(i), s);
                    }
                }
            }

            // if version Control is active, check out the file automatically if it is not yet checked out
            if (Provider.isActive)
            {
                // Get the version control asset of the file
                Asset stringsFile = Provider.GetAssetByPath($"Assets/StreamingAssets/{_currentlyEditingStreamingAssetsFilePath}");

                // checkout the file if it is not checked out yet and wait until it is checked out
                if (!Provider.IsOpenForEdit(stringsFile))
                    Provider.Checkout(stringsFile, CheckoutMode.Asset).Wait();
            }

            // try to save the file. If the access is denied, display a message to the user
            try
            {
                processedStrings.SaveToFile(_currentlyEditingFilePath);
            }
            catch (UnauthorizedAccessException)
            {
                EditorUtility.DisplayDialog("Access Denied", $"The file {_currentlyEditingFilePath} cannot be accessed, is it write protected?", "Close");
                return;
            }

            // we just saved, so there can't be any unsaved changes
            _unsavedChanges = false;
        }

        /// <summary>
        /// Loads the strings from the file
        /// </summary>
        private void LoadStrings()
        {
            // if we are loading, we cannot have any unsaved changes
            _unsavedChanges = false;

            // try to load the file
            try
            {
                _strings = JSONObject.LoadFromFile(_currentlyEditingFilePath);

                // make sure the file contains the languages that are set and only the ones that are set
                foreach (JSONObject category in _strings)
                {
                    foreach (JSONObject field in category)
                    {
                        for (int i = field.Count - 1; i >= 0; --i)
                        {
                            if (!_config[LanguageManager.LANGUAGES_KEY].HasField(field.GetKeyAt(i)))
                            {
                                field.RemoveAt(i);
                                _unsavedChanges = true;
                            }
                        }

                        if (field.Count == _config[LanguageManager.LANGUAGES_KEY].Count)
                            continue;

                        for (int i = 0; i < _langs.Count; ++i)
                        {
                            if (!field.HasField(_config[LanguageManager.LANGUAGES_KEY].GetKeyAt(i)))
                            {
                                field.AddField(_config[LanguageManager.LANGUAGES_KEY].GetKeyAt(i), PLACEHOLDER_STRING);
                                _unsavedChanges = true;
                            }
                        }
                    }
                }
            }
            catch
            {
                // if loading failed, create new strings
                _strings = new JSONObject(JSONObject.Type.OBJECT);

                // add the default category
                AddCategory(LanguageManager.DEFAULT_CATEGORY);
            }

            // parse the strings into ther nice, formatted form
            LanguageManager.ParseStrings(_strings);

            // reset the editor
            ResetValues();

            if (_unsavedChanges)
                SaveStrings();
        }

        #endregion

        #region GENERATE_STRINGS

        /// <summary>
        /// Generates new strings by looking at all language texts in all scenes or just the open scene. This is additive, if there is a string file already, it won't override it but add to it
        /// </summary>
        private void GenerateStrings(bool onlyOpenScene)
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

            // look at the open scene or at all scenes to generate the file
            if (onlyOpenScene)
            {
                GenerateStringsForOpenScene();
            }
            else
            {
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
                  GenerateStringsForOpenScene();
                }

                // now re open the scene we came from
                EditorSceneManager.OpenScene(startScenePath);
            }

            // reset the editor
            ResetValues();
        }

        /// <summary>
        /// Generates the fields for the currently open scene
        /// </summary>
        private void GenerateStringsForOpenScene()
        {
            LanguageTextConfig[] ltcs = FindObjectsOfType<LanguageTextConfig>();

            // iterate through all language text configs and check if they already have a representation in the strings file
            foreach (LanguageTextConfig ltc in ltcs)
            {
                CheckField(ltc.Category, ltc.Field);
            }
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
                for (int i = 0; i < Config[LanguageManager.LANGUAGES_KEY].Count; ++i)
                {
                    newField.AddField(Config[LanguageManager.LANGUAGES_KEY].GetKeyAt(i), PLACEHOLDER_STRING);
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
            EditorGUI.FocusTextInControl(null);
            // reset our local representation of the laanguages
            _langs = null;

            // add the language in the languaes lookup
            Config[LanguageManager.LANGUAGES_KEY].AddField(_newLangCode, _newLangDisplay);

            if (_strings == null)
                return;

            // go through all categories and add the new language to every field
            for (int c = 0; c < _strings.Count; ++c)
            {
                JSONObject category = _strings[c];
                foreach (JSONObject languageString in category)
                {
                    languageString.AddField(_newLangCode, PLACEHOLDER_STRING);
                }
            }

            // mark file as dirty
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
            for (int i = 0; i < _langs.Count; ++i)
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

            // check if the new index is valid. We cant move further up than 1 because 0 is the default category that should always stay at the top.
            // Of course we cant move farther down than the max number of categories
            if (newIndex > 0 && newIndex < _strings.Count)
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
        /// Moves a category to the top or bottom
        /// </summary>
        /// <param name="categoryIndex">Current index of the category</param>
        /// <param name="top">Move to the top? Else it moves to the bottom</param>
        private void MoveCategory(int categoryIndex, bool top)
        {
            // Move the category. Top is index 2 because 0 is the default category
            int newIndex = top ? 1 : _strings.Count - 1;
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

        /// <summary>
        /// Moves a language field up or down
        /// </summary>
        /// <param name="category">Category containing the filed</param>
        /// <param name="fieldIndex">Current index of the field</param>
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
        /// Moves a language field to the top or bottom
        /// </summary>
        /// <param name="category">Category containing the filed</param>
        /// <param name="fieldIndex">Current index of the field</param>
        /// <param name="top">Move to the top? Else it moves to the bottom</param>
        private void MoveLanguageField(JSONObject category, int fieldIndex, bool top)
        {
            category.MoveField(fieldIndex, top ? 0 : category.Count - 1);
            _unsavedChanges = true;
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

            Config[LanguageManager.LANGUAGES_KEY].RenameField(oldCode, newCode);

            if (_strings == null)
                return;

            // go through all categories
            for (int c = 0; c < _strings.Count; ++c)
            {
                JSONObject category = _strings[c];

                // rename the language field in the strings
                foreach (JSONObject languageString in category)
                {
                    languageString.RenameField(oldCode, newCode);
                }
            }

            // mark file as dirty
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

            Config[LanguageManager.LANGUAGES_KEY].RemoveField(lang);

            if (_strings == null)
                return;

            // go through all categories
            for (int c = 0; c < _strings.Count; ++c)
            {
                JSONObject category = _strings[c];

                // remove the language field from the strings
                foreach (JSONObject languageString in category)
                {
                    languageString.RemoveField(lang);
                }
            }

            // mark file as dirty
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
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // draw settings
            EditorGUI.BeginChangeCheck();


            bool useSavedLanguage =
                EditorGUILayout.ToggleLeft(
                    new GUIContent("Use saved language on Application start",
                        "If set, the Language Manager tries to load the language that was last used from the Player Prefs. If there is none or if the strings file does not contain the language, it will go through the other options after this"),
                    Config[LanguageManager.CONFIG_USE_SAVED_LANG_KEY].BoolValue);


            bool useSystemLanguage =
                EditorGUILayout.ToggleLeft(
                    new GUIContent("Use system language as default",
                        "If set, the Language Manager uses the Language of the System of the user as the default language, if the strings file contains this language"),
                    Config[LanguageManager.CONFIG_USE_SYSTEM_LANG_DEFAULT_KEY].BoolValue);

            string defaultLanguage =
                EditorGUILayout.DelayedTextField(new GUIContent("Default Language", "Language Code of the Language to use as a default or as a fallback if the system language is not supported"),
                    Config[LanguageManager.CONFIG_DEFAULT_LANG_KEY].StringValue);

            EditorGUILayout.EndVertical();
            GUILayout.Space(20);

            ShowLanguages();
            if (_langs == null || _langs.Count == 0)
                _langs = new List<string>(Config[LanguageManager.LANGUAGES_KEY].Keys);


            // set settings values
            if (EditorGUI.EndChangeCheck())
            {
                CheckoutConfig();

                Config[LanguageManager.CONFIG_USE_SYSTEM_LANG_DEFAULT_KEY].BoolValue = useSystemLanguage;
                Config[LanguageManager.CONFIG_DEFAULT_LANG_KEY].StringValue = defaultLanguage;
                Config[LanguageManager.CONFIG_USE_SAVED_LANG_KEY].BoolValue = useSavedLanguage;

                // delete language key in player prefs so testing is easier
                PlayerPrefs.DeleteKey("Lang");

                Config.SaveToFile(ConfigFilePath);
            }
        }

        /// <summary>
        /// Displays an editor for editing the strings
        /// </summary>
        private void ShowStringsEditor()
        {
            EditorGUILayout.LabelField("Strings", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            ShowActionButtons();

            // don't continue if no strings are loaded
            if (_strings == null)
            {
                EditorGUILayout.EndVertical();
                return;
            }

            GUILayout.Space(20);


            // display all the categories in a scroll view
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            ShowCategories();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Shows Buttons for all the actions concerning the strings editor
        /// </summary>
        private void ShowActionButtons()
        {
            // button for opening a string file
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent(string.IsNullOrEmpty(_currentlyEditingFilePath) ? "Select a file to edit" : _currentlyEditingFilePath, "The currently edited strings file"),
                FileButtonStyle))
            {
                string path = EditorUtility.OpenFilePanel("Select a strings file to edit", Application.streamingAssetsPath, "json");

                if (!string.IsNullOrEmpty(path))
                {
                    if (CheckPath(path))
                    {
                        _currentlyEditingFilePath = path;
                        _currentlyEditingStreamingAssetsFilePath = _currentlyEditingFilePath.Remove(0, Application.streamingAssetsPath.Length);
                        LoadStrings();
                    }
                }
            }

            // button for creating a new string file
            if (GUILayout.Button(new GUIContent("New Strings File", "Create a new Strings file"), BUTTON_WIDTH))
            {
                string path = EditorUtility.SaveFilePanel("Create a new Strings file", Application.streamingAssetsPath, "strings", "json");

                if (!string.IsNullOrEmpty(path))
                {
                    if (CheckPath(path))
                    {
                        _currentlyEditingFilePath = path;
                        _currentlyEditingStreamingAssetsFilePath = _currentlyEditingFilePath.Remove(0, Application.streamingAssetsPath.Length);
                        LoadStrings();
                        SaveStrings();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            if(string.IsNullOrEmpty(_currentlyEditingFilePath))
                return;
            
            EditorGUILayout.BeginHorizontal();

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
            if (_strings != null)
            {
                // if strings are loaded, display a save button that is grayed out if there are no unsaved changes
                if (GUILayout.Button("Save Strings", !_unsavedChanges ? GrayedOutButtonStyle : GUI.skin.button, BUTTON_WIDTH))
                    SaveStrings();
            }


            GUILayout.Space(40);

            // generate strings         
            if (GUILayout.Button(
                new GUIContent("Generate Strings",
                    "Looks at all Language Texts in ALL SCENES of this Project and generates fields for them in the strings file. If no file exists, it creates a new one"), BUTTON_WIDTH))
                GenerateStrings(false);
            
            if (GUILayout.Button(
                new GUIContent("Generate for Current",
                    "Looks at all Language Texts in the CURRENT SCENE and generates fields for them in the strings file. If no file exists, it creates a new one"), BUTTON_WIDTH))
                GenerateStrings(true);

            GUILayout.Space(40);
            
            // If pressed, this button opens a Text Preview window that displays formatted version of the text currently edited
            if (GUILayout.Button(new GUIContent("Text Preview", "Opens a window that shows a rich text formatted version of the string currently edited as it will appear in the language texts"),
                BUTTON_WIDTH))
                TextPreviewWindow.OpenTextPreviewWindow();


            EditorGUILayout.EndHorizontal();
        }

        #region LANGUAGES

        /// <summary>
        /// Show the edit area for the supported Languages
        /// </summary>
        private void ShowLanguages()
        {
            // headline
            EditorGUILayout.LabelField("Languages", EditorStyles.boldLabel, CELL_WIDTH);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // display all languages
            if (EditorGUILayout.BeginFadeGroup(ShowLangs.faded))
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                // add an indent
                GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal);

                // header
                EditorGUILayout.LabelField("Code", BUTTON_WIDTH);
                EditorGUILayout.LabelField("Display Name", BUTTON_WIDTH);
                EditorGUILayout.EndHorizontal();

                // get the languages from the config
                JSONObject languages = Config[LanguageManager.LANGUAGES_KEY];

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
                    string langCode = EditorGUILayout.DelayedTextField(languages.GetKeyAt(i), BUTTON_WIDTH);
                    if (EditorGUI.EndChangeCheck())
                        RenameLanguage(languages.GetKeyAt(i), langCode);

                    // display name
                    EditorGUI.BeginChangeCheck();
                    string langName = EditorGUILayout.DelayedTextField(languages[i].StringValue, BUTTON_WIDTH);
                    if (EditorGUI.EndChangeCheck())
                    {
                        languages.SetField(langCode, langName);
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
                _newLangCode = EditorGUILayout.DelayedTextField(_newLangCode, BUTTON_WIDTH);
                _newLangDisplay = EditorGUILayout.DelayedTextField(_newLangDisplay, BUTTON_WIDTH);

                // a button to add the new language
                if (GUILayout.Button("Add Language", BUTTON_WIDTH))
                    AddLanguage();

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndFadeGroup();
            // display a button for folding the languages in or out
            if (GUILayout.Button(ShowLangs.target ? "Retract" : "Expand"))
                ShowLangs.target = !ShowLangs.target;
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region CATEGORY

        /// <summary>
        /// Displays all the categories
        /// </summary>
        private void ShowCategories()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // create aa list to store which categories to remove
            List<int> categoriesToRemove = new List<int>();

            // go though all categories and display them with all their fields
            for (int c = 0; c < _strings.Count; ++c)
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
                if (EditorGUILayout.BeginFadeGroup(ShowCategory[c].faded))
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

                        EditorGUILayout.Space();
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
            EditorGUILayout.EndVertical();
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
            if (categoryIndex == 0)
            {
                GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal);
                GUILayout.Space(SQUARE_BUTTON_WIDTH + GUI.skin.button.margin.horizontal);
            }
            else
            {
                // display buttons to move the category to the top or bottom
                EditorGUILayout.BeginVertical(GUILayout.Width(SQUARE_BUTTON_WIDTH));
                GUILayout.Space(VERTICAL_BUTTONS_INITIAL_SPACE);
                if (GUILayout.Button(new GUIContent("⇑", "Move to top"), GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                    MoveCategory(categoryIndex, true);
                if (GUILayout.Button(new GUIContent("⇓", "Move to bottom"), GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                    MoveCategory(categoryIndex, false);
                EditorGUILayout.EndHorizontal();

                // display buttons to move the category up or down
                EditorGUILayout.BeginVertical(GUILayout.Width(SQUARE_BUTTON_WIDTH));
                GUILayout.Space(VERTICAL_BUTTONS_INITIAL_SPACE);
                if (GUILayout.Button(new GUIContent("↑", "Move up one"), GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                    MoveCategory(categoryIndex, -1);
                if (GUILayout.Button(new GUIContent("↓", "Move down one"), GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                    MoveCategory(categoryIndex, 1);
                EditorGUILayout.EndVertical();
            }

            // show a button to extand and retract the category
            if (!ShowCategory[categoryIndex].target)
                ShowCategory[categoryIndex].target = GUILayout.Button(">", GUILayout.Width(SQUARE_BUTTON_WIDTH));
            else
                ShowCategory[categoryIndex].target = !GUILayout.Button("v", GUILayout.Width(SQUARE_BUTTON_WIDTH));

            // display the name of the category. If we are in the first category, we are in the default category. This category cannot be renamed, so we just display a label
            if (categoryIndex == 0)
            {
                EditorGUILayout.LabelField(_strings.GetKeyAt(categoryIndex), EditorStyles.whiteLargeLabel, CELL_WIDTH, GUILayout.Height(20));
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                string category = EditorGUILayout.DelayedTextField(_strings.GetKeyAt(categoryIndex), CategoryNameStyle, CELL_WIDTH, GUILayout.Height(20));
                if (EditorGUI.EndChangeCheck())
                {
                    _strings.RenameField(categoryIndex, category);
                    _unsavedChanges = true;
                }
            }

            // duplicate category
            if (GUILayout.Button("Duplicate", BUTTON_WIDTH))
                DuplicateCategory(_strings[categoryIndex]);

            // if we are not in the default category, show a button to delete the category
            if (categoryIndex > 0)
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
            for (int i = 0; i < _langs.Count; ++i)
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

            // display buttons to move the field to the top or bottom
            EditorGUILayout.BeginVertical(GUILayout.Width(SQUARE_BUTTON_WIDTH));
            GUILayout.Space(VERTICAL_BUTTONS_INITIAL_SPACE);
            if (GUILayout.Button(new GUIContent("⇑", "Move to top in Category"), GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                MoveLanguageField(categoryObj, fieldIndex, true);
            if (GUILayout.Button(new GUIContent("⇓", "Move to bottom in Category"), GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                MoveLanguageField(categoryObj, fieldIndex, false);
            EditorGUILayout.EndHorizontal();

            // display buttons to move the field up or down
            EditorGUILayout.BeginVertical(GUILayout.Width(SQUARE_BUTTON_WIDTH));
            GUILayout.Space(VERTICAL_BUTTONS_INITIAL_SPACE);
            if (GUILayout.Button(new GUIContent("↑", "Move up one"), GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                MoveLanguageField(categoryObj, fieldIndex, -1);
            if (GUILayout.Button(new GUIContent("↓", "Move down one"), GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                MoveLanguageField(categoryObj, fieldIndex, 1);
            EditorGUILayout.EndVertical();


            // delete the field
            if (GUILayout.Button("X", GUILayout.Width(SQUARE_BUTTON_WIDTH)))
                ret = false;

            // display field name
            EditorGUI.BeginChangeCheck();
            string fieldName = EditorGUILayout.DelayedTextField(categoryObj.GetKeyAt(fieldIndex), CELL_WIDTH);
            if (EditorGUI.EndChangeCheck())
            {
                categoryObj.RenameField(fieldIndex, fieldName);
                _unsavedChanges = true;
            }

            // show strings for all languages
            for (int j = 0; j < _langs.Count; ++j)
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

        /// <summary>
        /// Checks if a provided path is inside the Streaming Assets and displays a dialog if it is not
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CheckPath(string path)
        {
            if (!path.StartsWith(Application.streamingAssetsPath))
            {
                EditorUtility.DisplayDialog("Invalid Path", $"String files must reside inside the StreamingAssets folder", "Close");
                return false;
            }

            return true;
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