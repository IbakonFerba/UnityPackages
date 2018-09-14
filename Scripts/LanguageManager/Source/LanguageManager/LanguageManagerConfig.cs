using FK.Utility;
using UnityEngine;

namespace FK.Language
{
    /// <summary>
    /// <para>Configuration Data for the Language Manager</para>
    ///
    /// v1.0 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class LanguageManagerConfig : SingletonScriptableObject<LanguageManagerConfig>
    {
        // ######################## PUBLIC VARS ######################## //       
        /// <summary>
        /// Should we load the strings asynchronously?
        /// </summary>
        [HideInInspector] public bool LoadStringsAsync = true;
        
        /// <summary>
        /// File name of the strings file without file ending
        /// </summary>
        [HideInInspector] public string StringsFileName = "strings";

        /// <summary>
        /// Should the system language be used as the default language?
        /// </summary>
        [HideInInspector] public bool UseSystemLanguageAsDefault = true;
        
        /// <summary>
        /// Language Code of the default language
        /// </summary>
        [HideInInspector] public string DefaultLang = "en";

        /// <summary>
        /// Should the language when loading the app be taken from the playerprefs?
        /// </summary>
        [HideInInspector] public bool UseSavedLanguage = true;
    }
}