using UnityEditor;
using UnityEngine;
using FK.Utility.UI;

/// <summary>
/// <para>Custom Editor for the Sprite Swapper Component</para>
/// 
/// v1.0 06/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CustomEditor(typeof(SpriteSwapper))]
public class SpriteSwapperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SpriteSwapper spriteSwapper = (SpriteSwapper)target;

        spriteSwapper.HighlightedSprite = (Sprite)EditorGUILayout.ObjectField("Highlighted Sprite", spriteSwapper.HighlightedSprite, typeof(Sprite), true);
        spriteSwapper.PressedSprite = (Sprite)EditorGUILayout.ObjectField("Pressed Sprite", spriteSwapper.PressedSprite, typeof(Sprite), true);
        spriteSwapper.DisabledSprite = (Sprite)EditorGUILayout.ObjectField("Disabled Sprite", spriteSwapper.DisabledSprite, typeof(Sprite), true);
    }
}
