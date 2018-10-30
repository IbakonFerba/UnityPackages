using System;
using System.IO;
using System.Threading;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FK.JSON;
using FK.Utility;

namespace FK.Language
{
    /// <summary>
    /// <para>This Language Manager works without being present in any scene. Everything concerning it is static.</para>
    /// <para>It loads the strings from a json file in the StreamingAssets folder. You can then set text in different languages either manually or use the language texts that manage language changes automatically</para>
    ///
    /// v2.1 10/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class LanguageManager
    {
        // ######################## ENUMS & DELEGATES ######################## //
        /// <summary>
        /// Delegate for the language change
        /// </summary>
        /// <param name="newLanguage"></param>
        public delegate void LanguageChangedCallback(string newLanguage);

        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// If this is false, the strings are not loaded yet. If you want to get strings on the start of the application, you should wait until this is true
        /// </summary>
        public static bool Initialized { get; private set; }

        /// <summary>
        /// The Language Code of the Current Language
        /// </summary>
        public static string CurrentLanguage
        {
            get { return _currentLanguage; }
            private set
            {
                _currentLanguage = value;
                PlayerPrefs.SetString("Lang", CurrentLanguage);
            }
        }

        /// <summary>
        /// All languages that are available in the strings file
        /// </summary>
        public static string[] Languages
        {
            get
            {
                if (_langs == null)
                {
                    if (_strings != null)
                        _langs = _strings[LANGUAGES_KEY].Keys;
                    else
                        Debug.LogWarning("Trying to access Languages with no string file loaded! Either the LanguageManager is not initialized yet or the file does not exist!");
                }

                return _langs;
            }
        }

        /// <summary>
        /// The language the users OS is running in
        /// </summary>
        public static string SystemLanguage
        {
            get
            {
                switch (Application.systemLanguage)
                {
                    case UnityEngine.SystemLanguage.Afrikaans:
                        return "af";
                    case UnityEngine.SystemLanguage.Arabic:
                        return "ar";
                    case UnityEngine.SystemLanguage.Basque:
                        return "eu";
                    case UnityEngine.SystemLanguage.Belarusian:
                        return "be";
                    case UnityEngine.SystemLanguage.Bulgarian:
                        return "bg";
                    case UnityEngine.SystemLanguage.Catalan:
                        return "ca";
                    case UnityEngine.SystemLanguage.Chinese:
                        return "zh";
                    case UnityEngine.SystemLanguage.Czech:
                        return "cs";
                    case UnityEngine.SystemLanguage.Danish:
                        return "da";
                    case UnityEngine.SystemLanguage.Dutch:
                        return "nl";
                    case UnityEngine.SystemLanguage.English:
                        return "en";
                    case UnityEngine.SystemLanguage.Estonian:
                        return "et";
                    case UnityEngine.SystemLanguage.Faroese:
                        return "fo";
                    case UnityEngine.SystemLanguage.Finnish:
                        return "fi";
                    case UnityEngine.SystemLanguage.French:
                        return "fr";
                    case UnityEngine.SystemLanguage.German:
                        return "de";
                    case UnityEngine.SystemLanguage.Greek:
                        return "el";
                    case UnityEngine.SystemLanguage.Hebrew:
                        return "he";
                    case UnityEngine.SystemLanguage.Hungarian:
                        return "hu";
                    case UnityEngine.SystemLanguage.Icelandic:
                        return "is";
                    case UnityEngine.SystemLanguage.Indonesian:
                        return "id";
                    case UnityEngine.SystemLanguage.Italian:
                        return "it";
                    case UnityEngine.SystemLanguage.Japanese:
                        return "ja";
                    case UnityEngine.SystemLanguage.Korean:
                        return "ko";
                    case UnityEngine.SystemLanguage.Latvian:
                        return "lv";
                    case UnityEngine.SystemLanguage.Lithuanian:
                        return "lt";
                    case UnityEngine.SystemLanguage.Norwegian:
                        return "no";
                    case UnityEngine.SystemLanguage.Polish:
                        return "pl";
                    case UnityEngine.SystemLanguage.Portuguese:
                        return "pt";
                    case UnityEngine.SystemLanguage.Romanian:
                        return "ro";
                    case UnityEngine.SystemLanguage.Russian:
                        return "ru";
                    case UnityEngine.SystemLanguage.SerboCroatian:
                        return "sr";
                    case UnityEngine.SystemLanguage.Slovak:
                        return "sk";
                    case UnityEngine.SystemLanguage.Slovenian:
                        return "sl";
                    case UnityEngine.SystemLanguage.Spanish:
                        return "es";
                    case UnityEngine.SystemLanguage.Swedish:
                        return "sv";
                    case UnityEngine.SystemLanguage.Thai:
                        return "th";
                    case UnityEngine.SystemLanguage.Turkish:
                        return "tr";
                    case UnityEngine.SystemLanguage.Ukrainian:
                        return "uk";
                    case UnityEngine.SystemLanguage.Vietnamese:
                        return "vi";
                    case UnityEngine.SystemLanguage.ChineseSimplified:
                        return "zh";
                    case UnityEngine.SystemLanguage.ChineseTraditional:
                        return "zh";
                    case UnityEngine.SystemLanguage.Unknown:
                        return null;
                    default:
                        return null;
                }
            }
        }

        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// This callback is invoked every time the language is changed
        /// </summary>
        public static LanguageChangedCallback OnLanguageChanged;

        /// <summary>
        /// The default category that is used to look up a string when no category is provided
        /// </summary>
        public const string DEFAULT_CATEGORY = "default";

        /// <summary>
        /// Key of the Object that contains all available languages
        /// </summary>
        public const string LANGUAGES_KEY = "Languages";

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// File extension of the strings file
        /// </summary>
        private const string JSON_FILE_EXTENSION = ".json";

        /// <summary>
        /// Escaped Line breaks to replace
        /// </summary>
        private static readonly string[] ESCAPED_LINE_BREAKS = {"\\r\\n", "\\r", "\\n"};

        /// <summary>
        /// The JSON Object containing all strings
        /// </summary>
        private static JSONObject _strings;

        private static string _currentLanguage;

        /// <summary>
        /// All available Languages
        /// </summary>
        private static string[] _langs;


        // ######################## INITS ######################## //

        #region INIT

        /// <summary>
        /// Loads the strings and initializes the Manager on Application Start
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            // if there is no Language Manager Config, we cannot proceed
            if (LanguageManagerConfig.Instance == null)
                throw new NullReferenceException("Could not find a Language Manager Config! This should not happen, open the Language Manager to create one!");

            // get the current language
            // if we have a saved Language in the Player Prefs we load that
            CurrentLanguage = LanguageManagerConfig.Instance.UseSavedLanguage && PlayerPrefs.HasKey("Lang") ? PlayerPrefs.GetString("Lang") : null;


            // calculate the path to the strings file
            string path = Path.Combine(Application.streamingAssetsPath, LanguageManagerConfig.Instance.StringsFileName + JSON_FILE_EXTENSION);

            // now we need to load the file either asynchronously or synchronously
            if (LanguageManagerConfig.Instance.LoadStringsAsync)
            {
                // load the file async
                _strings = new JSONObject();
#if !UNITY_EDITOR
                CoroutineHost.Instance.StartCoroutine(LoadStringsAsync(path));
#else
                CoroutineHost.StartTrackedCoroutine(LoadStringsAsync(path), _strings, "LanguageManager");
#endif
            }
            else
            {
                // load the file synchronously
                try
                {
                    _strings = JSONObject.LoadFromFile(path);
                }
                catch (FileNotFoundException)
                {
                    Debug.LogError($"Could not load strings from {path} because the File does not exist!");
                    throw;
                }

                // if the strings file does not have the Languages Object, we have a problem, abort!
                if (!_strings.HasField(LANGUAGES_KEY))
                {
                    _strings = null;
                    throw new NullReferenceException($"Improper structure of strings file, could not find property \"{LANGUAGES_KEY}\"");
                }

                ParseStrings(_strings);
                FinishInit();
            }
        }

        /// <summary>
        /// Loads the strings asynchronously
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private static IEnumerator LoadStringsAsync(string path)
        {
#if UNITY_EDITOR
            // in the editor keep track of how long the loading process takes
            System.Diagnostics.Stopwatch loadWatch = new System.Diagnostics.Stopwatch();
            loadWatch.Start();
#endif
            // wait for the JSONObject to load
            yield return JSONObject.LoadFromFileAsync(path, _strings, -2);

            // if the strings file does not have the Languages Object, we have a problem, abort!
            if (!_strings.HasField(LANGUAGES_KEY))
            {
                _strings = null;
                throw new NullReferenceException($"Improper structure of strings file, could not find property \"{LANGUAGES_KEY}\"");
            }

            // parse the strings asynchronously
            Thread parseThread = new Thread(() => ParseStrings(_strings));
            parseThread.Start();

            // wait until parsing is done
            yield return new WaitWhile(() => parseThread.ThreadState == ThreadState.Running);

            // finish him!
            FinishInit();

#if UNITY_EDITOR
            // tell the dev how much time loading took
            Debug.Log($"Loaded strings in {System.Math.Round(loadWatch.Elapsed.TotalMilliseconds)} milliseconds!");
            loadWatch.Reset();
#endif
        }

        /// <summary>
        /// The last steps of the initializing process
        /// </summary>
        private static void FinishInit()
        {
            // if the CurrentLanguage is invalid or not contained in the strings file, check our other options
            if (string.IsNullOrEmpty(CurrentLanguage) || !_strings[LANGUAGES_KEY].HasField(CurrentLanguage))
            {
                // if we should use the system language and it is contained in the strings, use that
                if (LanguageManagerConfig.Instance.UseSystemLanguageAsDefault && _strings[LANGUAGES_KEY].HasField(SystemLanguage))
                {
                    CurrentLanguage = SystemLanguage;
                } // if we could not use the system language, use the default language if it is contained in the strings
                else if (_strings[LANGUAGES_KEY].HasField(LanguageManagerConfig.Instance.DefaultLang))
                {
                    CurrentLanguage = LanguageManagerConfig.Instance.DefaultLang;
                }
                else // we are out of options, just use the first language
                {
                    Debug.LogWarning($"Selected language \"{CurrentLanguage}\" does not exist, using \"{Languages[0]}\" instead");
                    CurrentLanguage = Languages[0];
                }
            }

            // show that we are done initializing
            Initialized = true;
        }

        #endregion

        /// <summary>
        /// Parses the strings so linebreaks are actual line breaks
        /// </summary>
        public static void ParseStrings(JSONObject strings)
        {
            foreach (JSONObject category in strings)
            {
                foreach (JSONObject languageString in category)
                {
                    for (int i = 0; i < languageString.Count; ++i)
                    {
                        string s = languageString[i].StringValue;
                        for (int j = 0; j < ESCAPED_LINE_BREAKS.Length; ++j)
                        {
                            s = s.Replace(ESCAPED_LINE_BREAKS[j], "\n");
                        }

                        languageString.SetField(languageString.GetKeyAt(i), s);
                    }
                }
            }
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Set the language to the provided Language if it is valid
        /// </summary>
        /// <param name="language">The Language Code of the desired Language</param>
        public static void SetLanguage(string language)
        {
            // make sure we are lower case
            string lowerCaseLang = language.ToLower();

            // if the language is invalid or not contained in the file, we cannot change
            if (string.IsNullOrEmpty(lowerCaseLang) || !_strings[LANGUAGES_KEY].HasField(lowerCaseLang))
            {
                Debug.LogError($"Cannot change language because language \"{lowerCaseLang}\" does not exist!");
                return;
            }

            // change language and notify everyone
            CurrentLanguage = lowerCaseLang;
            OnLanguageChanged?.Invoke(CurrentLanguage);
        }


        #region SET_TEXT

        /// <summary>
        /// Sets the text of the provided Text object to the correct one in the current language
        /// </summary>
        /// <param name="textField">Text Field to set the Text on</param>
        /// <param name="name">Name of the text in the strings file</param>
        /// <param name="category">Category of the text in the strings file</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void SetText(Text textField, string name, string category = DEFAULT_CATEGORY)
        {
            // if the text field is null, we cannot do anything
            if (textField == null)
                return;

            // if we have no strings, we cannot continue, throw an exeption so the dev knows this won't work!
            if (_strings == null)
                throw new NullReferenceException("Trying to access strings with no string file loaded! Either the LanguageManager is not initialized yet or the file does not exist!");

            // get the string
            string s = _strings[category]?[name]?[CurrentLanguage]?.StringValue;

            // if the string is null, something went wrong, notify the dev with an exeption (I know devs love them)
            if (s == null)
            {
                textField.text = "<MISSING>";
                throw new NullReferenceException($"Could not find string \"{name}\" in language \"{CurrentLanguage}\" in category \"{category}\"");
            }

            // set the text
            textField.text = s;
        }

        /// <summary>
        /// Sets the text of the provided Text object to the correct one in any language
        /// </summary>
        /// <param name="textField">Text Field to set the Text on</param>
        /// <param name="name">Name of the text in the strings file</param>
        /// <param name="category">Category of the text in the strings file</param>
        /// <param name="language">The language to use</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void SetText(Text textField, string name, string category, string language)
        {
            // if the text field is null, we cannot do anything
            if (textField == null)
                return;

            // if we have no strings, we cannot continue, throw an exeption so the dev knows this won't work!
            if (_strings == null)
                throw new NullReferenceException("Trying to access strings with no string file loaded! Either the LanguageManager is not initialized yet or the file does not exist!");

            // make sure the language is lower case
            string lowerCaseLang = language.ToLower();

            // get the string
            string s = _strings[category]?[name]?[lowerCaseLang]?.StringValue;

            // if the string is null, something went wrong, notify the dev with an exeption (I know devs love them)
            if (s == null)
            {
                textField.text = "<MISSING>";
                throw new NullReferenceException($"Could not find string \"{name}\" in language \"{lowerCaseLang}\" in category \"{category}\"");
            }

            // set the text
            textField.text = s;
        }

        /// <summary>
        /// Sets the text of the provided Text object to the correct one in the current language
        /// </summary>
        /// <param name="textField">Text Field to set the Text on</param>
        /// <param name="name">Name of the text in the strings file</param>
        /// <param name="category">Category of the text in the strings file</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void SetText(TMP_Text textField, string name, string category = DEFAULT_CATEGORY)
        {
            // if the text field is null, we cannot do anything
            if (textField == null)
                return;

            // if we have no strings, we cannot continue, throw an exeption so the dev knows this won't work!
            if (_strings == null)
                throw new NullReferenceException("Trying to access strings with no string file loaded! Either the LanguageManager is not initialized yet or the file does not exist!");

            // get the string
            string s = _strings[category]?[name]?[CurrentLanguage]?.StringValue;

            // if the string is null, something went wrong, notify the dev with an exeption (I know devs love them)
            if (s == null)
            {
                textField.text = "<MISSING>";
                throw new NullReferenceException($"Could not find string \"{name}\" in language \"{CurrentLanguage}\" in category \"{category}\"");
            }

            // set the text
            textField.text = s;
        }

        /// <summary>
        /// Sets the text of the provided Text object to the correct one in any language
        /// </summary>
        /// <param name="textField">Text Field to set the Text on</param>
        /// <param name="name">Name of the text in the strings file</param>
        /// <param name="category">Category of the text in the strings file</param>
        /// <param name="language">The language to use</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void SetText(TMP_Text textField, string name, string category, string language)
        {
            // if the text field is null, we cannot do anything
            if (textField == null)
                return;

            // if we have no strings, we cannot continue, throw an exeption so the dev knows this won't work!
            if (_strings == null)
                throw new NullReferenceException("Trying to access strings with no string file loaded! Either the LanguageManager is not initialized yet or the file does not exist!");

            // make sure the language is lower case
            string lowerCaseLang = language.ToLower();

            // get the string
            string s = _strings[category]?[name]?[lowerCaseLang]?.StringValue;

            // if the string is null, something went wrong, notify the dev with an exeption (I know devs love them)
            if (s == null)
            {
                textField.text = "<MISSING>";
                throw new NullReferenceException($"Could not find string \"{name}\" in language \"{lowerCaseLang}\" in category \"{category}\"");
            }

            // set the text
            textField.text = s;
        }

        #endregion

        #region GET_STRING

        /// <summary>
        /// Returns the string in the current language
        /// </summary>
        /// <param name="name">Name of the text in the strings file</param>
        /// <param name="category">Category of the text in the strings file</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static string GetString(string name, string category = DEFAULT_CATEGORY)
        {
            // if we have no strings, we cannot continue, throw an exeption so the dev knows this won't work!
            if (_strings == null)
                throw new NullReferenceException("Trying to access strings with no string file loaded! Either the LanguageManager is not initialized yet or the file does not exist!");

            // get the string
            string s = _strings[category]?[name]?[CurrentLanguage]?.StringValue;

            // if the string is null, something went wrong, notify the dev with an exeption (I know devs love them)
            if (s == null)
                throw new NullReferenceException($"Could not find string \"{name}\" in language \"{CurrentLanguage}\" in category \"{category}\"");

            return s;
        }

        /// <summary>
        /// Returns the string in the provided language
        /// </summary>
        /// <param name="name">Name of the text in the strings file</param>
        /// <param name="category">Category of the text in the strings file</param>
        /// <param name="language">The language to use</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static string GetString(string name, string category, string language)
        {
            // if we have no strings, we cannot continue, throw an exeption so the dev knows this won't work!
            if (_strings == null)
                throw new NullReferenceException("Trying to access strings with no string file loaded! Either the LanguageManager is not initialized yet or the file does not exist!");

            // make sure the language is lower case
            string lowerCaseLang = language.ToLower();

            // get the string
            string s = _strings[category]?[name]?[lowerCaseLang]?.StringValue;

            // if the string is null, something went wrong, notify the dev with an exeption (I know devs love them)
            if (s == null)
                throw new NullReferenceException($"Could not find string \"{name}\" in language \"{lowerCaseLang}\" in category \"{category}\"");

            return s;
        }

        #endregion


        // ######################## UTILITIES ######################## //
        /// <summary>
        /// Returns the Language Name for the provided Language code if it exists in the strings file
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static string GetLanguageDisplayName(string languageCode)
        {
            // if strings is null, we cannot continue
            if (_strings == null)
            {
                Debug.LogWarning("Trying to access Languages with no string file loaded! Either the LanguageManager is not initialized yet or the file does not exist!");
                return null;
            }

            // if the language does not exist, notify the dev
            if (!_strings[LANGUAGES_KEY].HasField(languageCode))
            {
                Debug.LogWarning($"Requested Language \"{languageCode}\" does not exist!");
                return null;
            }

            return _strings[LANGUAGES_KEY][languageCode].StringValue;
        }
    }
}