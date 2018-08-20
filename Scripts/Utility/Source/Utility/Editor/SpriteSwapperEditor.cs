#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using FK.Utility.UI;
using FK.Editor;

/// <summary>
/// <para>Custom Editor for the Sprite Swapper Component</para>
/// 
/// v2.0 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(SpriteSwapper))]
public class SpriteSwapperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update ();
        SpriteSwapper spriteSwapper = (SpriteSwapper)target;

        MultiObjectEditorGUI.ObjectField(serializedObject, "HighlightedSprite", "Highlighted Sprite", typeof(Sprite), true);
        MultiObjectEditorGUI.ObjectField(serializedObject, "PressedSprite", "Pressed Sprite", typeof(Sprite), true);
        MultiObjectEditorGUI.ObjectField(serializedObject, "DisabledSprite", "Disabled Sprite", typeof(Sprite), true);
        serializedObject.ApplyModifiedProperties ();
    }
}
#endif