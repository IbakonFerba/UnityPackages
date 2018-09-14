// remove this line if you don't have TextMeshPro in your Project

#define TEXT_MESH_PRO


#if UNITY_EDITOR
#if TEXT_MESH_PRO
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using FK.Editor;

namespace FK.Language
{
    /// <summary>
    /// <para>Custom Editor for the Text Mesh Pro Language Text. If this does not exists, unity does not show the default editor for text mesh pro texts</para>
    ///
    /// v2.0 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [CustomEditor(typeof(TextMeshProLanguageText))]
    [CanEditMultipleObjects]
    public class TextMeshProLanguageTextEditor : TMP_EditorPanel
    {
    }
}
#endif
#endif