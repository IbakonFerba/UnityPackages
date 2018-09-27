#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace FK.Editor
{
    /// <summary>
    /// <para>Elements for Multi Material Editing supporting Material Editors</para>
    ///
    /// v1.1 08/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class MultiMaterialEditorGUI
    {
        // ######################## FUNCTIONALITY ######################## //

        #region Color

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(MaterialEditor materialEditor, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Color c = EditorGUILayout.ColorField(p.colorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(MaterialEditor materialEditor, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Color c = EditorGUILayout.ColorField(label, p.colorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(MaterialEditor materialEditor, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Color c = EditorGUILayout.ColorField(label, p.colorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(MaterialEditor materialEditor, string property, string label, bool showEyedropper, bool showAlpha, bool hdr, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Color c = EditorGUILayout.ColorField(new GUIContent(label), p.colorValue, showEyedropper, showAlpha, hdr, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(MaterialEditor materialEditor, string property, GUIContent label, bool showEyedropper, bool showAlpha, bool hdr, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Color c = EditorGUILayout.ColorField(label, p.colorValue, showEyedropper, showAlpha, hdr, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.colorValue = c;

            return c;
        }

        #endregion


        #region DelayedFloat

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(MaterialEditor materialEditor, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.DelayedFloatField(p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(MaterialEditor materialEditor, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.DelayedFloatField(p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(MaterialEditor materialEditor, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.DelayedFloatField(label, p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(MaterialEditor materialEditor, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.DelayedFloatField(label, p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(MaterialEditor materialEditor, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.DelayedFloatField(label, p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(MaterialEditor materialEditor, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.DelayedFloatField(label, p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        #endregion


        #region Float

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(MaterialEditor materialEditor, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.FloatField(p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(MaterialEditor materialEditor, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.FloatField(p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(MaterialEditor materialEditor, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.FloatField(label, p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(MaterialEditor materialEditor, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.FloatField(label, p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(MaterialEditor materialEditor, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.FloatField(label, p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(MaterialEditor materialEditor, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.FloatField(label, p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        #endregion


        #region MinMaxSlider

        /// <summary>
        /// Make a special slider the user can use to specify a range between a min and a max.
        /// </summary>
        public static void MinMaxSlider(MaterialEditor materialEditor, string propertyMin, string propertyMax, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty min = MaterialEditor.GetMaterialProperty(materialEditor.targets, propertyMin);
            MaterialProperty max = MaterialEditor.GetMaterialProperty(materialEditor.targets, propertyMax);
            EditorGUI.showMixedValue = min.hasMixedValue || max.hasMixedValue;
            float m = min.floatValue;
            float ma = max.floatValue;
            EditorGUILayout.MinMaxSlider(ref m, ref ma, minLimit, maxLimit, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                min.floatValue = m;
                max.floatValue = ma;
            }
        }

        /// <summary>
        /// Make a special slider the user can use to specify a range between a min and a max.
        /// </summary>
        public static void MinMaxSlider(MaterialEditor materialEditor, string propertyMin, string propertyMax, string label, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty min = MaterialEditor.GetMaterialProperty(materialEditor.targets, propertyMin);
            MaterialProperty max = MaterialEditor.GetMaterialProperty(materialEditor.targets, propertyMax);
            EditorGUI.showMixedValue = min.hasMixedValue || max.hasMixedValue;
            float m = min.floatValue;
            float ma = max.floatValue;
            EditorGUILayout.MinMaxSlider(label, ref m, ref ma, minLimit, maxLimit, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                min.floatValue = m;
                max.floatValue = ma;
            }
        }

        /// <summary>
        /// Make a special slider the user can use to specify a range between a min and a max.
        /// </summary>
        public static void MinMaxSlider(MaterialEditor materialEditor, string propertyMin, string propertyMax, GUIContent label, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty min = MaterialEditor.GetMaterialProperty(materialEditor.targets, propertyMin);
            MaterialProperty max = MaterialEditor.GetMaterialProperty(materialEditor.targets, propertyMax);
            EditorGUI.showMixedValue = min.hasMixedValue || max.hasMixedValue;
            float m = min.floatValue;
            float ma = max.floatValue;
            EditorGUILayout.MinMaxSlider(label, ref m, ref ma, minLimit, maxLimit, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                min.floatValue = m;
                max.floatValue = ma;
            }
        }

        #endregion


        #region Slider

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(MaterialEditor materialEditor, string property, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.Slider(p.floatValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(MaterialEditor materialEditor, string property, string label, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.Slider(label, p.floatValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(MaterialEditor materialEditor, string property, GUIContent label, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            float f = EditorGUILayout.Slider(label, p.floatValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        #endregion


        #region Toggle

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(MaterialEditor materialEditor, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool b = EditorGUILayout.Toggle(p.floatValue > 0, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = b ? 1 : 0;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(MaterialEditor materialEditor, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool b = EditorGUILayout.Toggle(label, p.floatValue > 0, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = b ? 1 : 0;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(MaterialEditor materialEditor, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool b = EditorGUILayout.Toggle(label, p.floatValue > 0, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = b ? 1 : 0;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(MaterialEditor materialEditor, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool b = EditorGUILayout.Toggle(p.floatValue > 0, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = b ? 1 : 0;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(MaterialEditor materialEditor, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool b = EditorGUILayout.Toggle(label, p.floatValue > 0, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = b ? 1 : 0;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(MaterialEditor materialEditor, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool b = EditorGUILayout.Toggle(label, p.floatValue > 0, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = b ? 1 : 0;

            return b;
        }

        #endregion


        #region ToggleLeft

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(MaterialEditor materialEditor, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool b = EditorGUILayout.ToggleLeft(label, p.floatValue > 0, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = b ? 1 : 0;

            return b;
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(MaterialEditor materialEditor, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool b = EditorGUILayout.ToggleLeft(label, p.floatValue > 0, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = b ? 1 : 0;

            return b;
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(MaterialEditor materialEditor, string property, string label, GUIStyle labelStyle, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool b = EditorGUILayout.ToggleLeft(label, p.floatValue > 0, labelStyle, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = b ? 1 : 0;

            return b;
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(MaterialEditor materialEditor, string property, GUIContent label, GUIStyle labelStyle, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool b = EditorGUILayout.ToggleLeft(label, p.floatValue > 0, labelStyle, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = b ? 1 : 0;

            return b;
        }

        #endregion


        #region Vector2

        /// <summary>
        /// Make an X & Y field for entering a Vector2.
        /// </summary>
        public static Vector2 Vector2Field(MaterialEditor materialEditor, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Vector2 v = EditorGUILayout.Vector2Field(label, p.vectorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vectorValue = v;

            return v;
        }

        /// <summary>
        /// Make an X & Y field for entering a Vector2.
        /// </summary>
        public static Vector2 Vector2Field(MaterialEditor materialEditor, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Vector2 v = EditorGUILayout.Vector2Field(label, p.vectorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vectorValue = v;

            return v;
        }

        #endregion


        #region Vector3

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3.
        /// </summary>
        public static Vector3 Vector3Field(MaterialEditor materialEditor, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Vector3 v = EditorGUILayout.Vector3Field(label, p.vectorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vectorValue = v;

            return v;
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3.
        /// </summary>
        public static Vector3 Vector3Field(MaterialEditor materialEditor, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Vector3 v = EditorGUILayout.Vector3Field(label, p.vectorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vectorValue = v;

            return v;
        }

        #endregion


        #region Vector4

        /// <summary>
        /// Make an X, Y, Z & W field for entering a Vector4.
        /// </summary>
        public static Vector4 Vector4Field(MaterialEditor materialEditor, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Vector4 v = EditorGUILayout.Vector4Field(label, p.vectorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vectorValue = v;

            return v;
        }

        #endregion


        #region Texture

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static Texture TextureField(MaterialEditor materialEditor, string property, bool scaleOffset, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Texture t = (Texture) EditorGUILayout.ObjectField(p.textureValue, typeof(Texture), false, options);
            Vector2 textureTiling = Vector2.one;
            Vector2 textureOffset = Vector2.one;
            if (scaleOffset)
            {
                EditorGUI.indentLevel++;
                textureTiling = EditorGUILayout.Vector2Field("Tiling", new Vector2(p.textureScaleAndOffset.x, p.textureScaleAndOffset.y));
                textureOffset = EditorGUILayout.Vector2Field("Offset", new Vector2(p.textureScaleAndOffset.z, p.textureScaleAndOffset.w));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                p.textureValue = t;

                if (scaleOffset)
                {
                    p.textureScaleAndOffset = new Vector4(textureTiling.x, textureTiling.y, textureOffset.x, textureOffset.y);
                    ;
                }
            }

            return t;
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static Texture TextureField(MaterialEditor materialEditor, string property, string label, bool scaleOffset, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Texture t = (Texture) EditorGUILayout.ObjectField(label, p.textureValue, typeof(Texture), false, options);
            Vector2 textureTiling = Vector2.one;
            Vector2 textureOffset = Vector2.one;
            if (scaleOffset)
            {
                EditorGUI.indentLevel++;
                textureTiling = EditorGUILayout.Vector2Field("Tiling", new Vector2(p.textureScaleAndOffset.x, p.textureScaleAndOffset.y));
                textureOffset = EditorGUILayout.Vector2Field("Offset", new Vector2(p.textureScaleAndOffset.z, p.textureScaleAndOffset.w));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                p.textureValue = t;

                if (scaleOffset)
                {
                    p.textureScaleAndOffset = new Vector4(textureTiling.x, textureTiling.y, textureOffset.x, textureOffset.y);
                    ;
                }
            }

            return t;
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static Texture TextureField(MaterialEditor materialEditor, string property, GUIContent label, bool scaleOffset, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Texture t = (Texture) EditorGUILayout.ObjectField(label, p.textureValue, typeof(Texture), false, options);
            Vector2 textureTiling = Vector2.one;
            Vector2 textureOffset = Vector2.one;
            if (scaleOffset)
            {
                EditorGUI.indentLevel++;
                textureTiling = EditorGUILayout.Vector2Field("Tiling", new Vector2(p.textureScaleAndOffset.x, p.textureScaleAndOffset.y));
                textureOffset = EditorGUILayout.Vector2Field("Offset", new Vector2(p.textureScaleAndOffset.z, p.textureScaleAndOffset.w));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                p.textureValue = t;

                if (scaleOffset)
                {
                    p.textureScaleAndOffset = new Vector4(textureTiling.x, textureTiling.y, textureOffset.x, textureOffset.y);
                    ;
                }
            }

            return t;
        }

        /// <summary>
        /// Make a field to receive texture scale and offset
        /// </summary>
        public static Vector4 TextureScaleOffsetField(MaterialEditor materialEditor, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            Vector2 textureTiling = EditorGUILayout.Vector2Field("Tiling", new Vector2(p.textureScaleAndOffset.x, p.textureScaleAndOffset.y));
            Vector2 textureOffset = EditorGUILayout.Vector2Field("Offset", new Vector2(p.textureScaleAndOffset.z, p.textureScaleAndOffset.w));
            EditorGUI.showMixedValue = false;

            Vector4 result = new Vector4(textureTiling.x, textureTiling.y, textureOffset.x, textureOffset.y);
            if (EditorGUI.EndChangeCheck())
                p.textureScaleAndOffset = result;

            return result;
        }

        #endregion


        #region ShaderProperty

        public static void ShaderPropertyField(MaterialEditor materialEditor, string property, string label)
        {
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            materialEditor.ShaderProperty(p, label);
            EditorGUI.showMixedValue = false;
        }

        public static void ShaderPropertyField(MaterialEditor materialEditor, string property, GUIContent label)
        {
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            materialEditor.ShaderProperty(p, label);
            EditorGUI.showMixedValue = false;
        }

        public static void ShaderPropertyField(MaterialEditor materialEditor, string property, string label, Rect position)
        {
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            materialEditor.ShaderProperty(position, p, label);
            EditorGUI.showMixedValue = false;
        }

        public static void ShaderPropertyField(MaterialEditor materialEditor, string property, GUIContent label, Rect position)
        {
            MaterialProperty p = MaterialEditor.GetMaterialProperty(materialEditor.targets, property);
            EditorGUI.showMixedValue = p.hasMixedValue;
            materialEditor.ShaderProperty(position, p, label);
            EditorGUI.showMixedValue = false;
        }

        #endregion
    }
}
#endif