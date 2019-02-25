#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FK.JLoc
{
    /// <summary>
    /// <para>A window that displays a rich text formatted string intended for the Language Manager Editor</para>
    ///
    /// v1.0 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class TextPreviewWindow : EditorWindow
    {
        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// Style of the text
        /// </summary>
        private GUIStyle TextPreviewStyle
        {
            get
            {
                if (_textPreviewStyle == null)
                {
                    _textPreviewStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
                    _textPreviewStyle.richText = true;
                    _textPreviewStyle.fontSize = 20;
                }

                return _textPreviewStyle;
            }
        }

        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// The text that should be displayed
        /// </summary>
        public static string Text;

        // ######################## PRIVATE VARS ######################## //
        private Vector2 _scrollPos;
        private GUIStyle _textPreviewStyle;


        // ######################## UNITY EVENT FUNCTIONS ######################## //
        private void OnGUI()
        {
            // display the text in a scroll view
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            GUILayout.Space(20);
            EditorGUILayout.TextArea(Text, TextPreviewStyle);
            EditorGUILayout.EndScrollView();
            Repaint();
        }


        // ######################## INITS ######################## //
        public static void OpenTextPreviewWindow()
        {
            TextPreviewWindow window = (TextPreviewWindow) GetWindow(typeof(TextPreviewWindow));
            window.titleContent = new GUIContent("Text Preview");
            window.Show();
        }
    }
}
#endif