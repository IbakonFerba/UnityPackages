using UnityEngine;

namespace FK.Language
{
    /// <summary>
    /// <para>This script holds references to Objects that are needed by the Language Manager and need to be included in a Build</para>
    /// <para>An instance of this Script is created in each scene automatically to make sure the assets make it into the Build even though the Language Manager is not present in any scene</para>
    ///
    /// v1.0 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class LanguageManagerReferences : MonoBehaviour
    {
        // ######################## PUBLIC VARS ######################## //
        [HideInInspector] public LanguageManagerConfig Config;
    }
}