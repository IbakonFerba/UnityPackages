using UnityEngine;

namespace FK.Language
{
    /// <summary>
    /// <para>The Data for a Language Dependent Text</para>
    ///
    /// v1.0 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
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
    }
}