#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace FK.Editor
{
    /// <summary>
    /// <para>Elements for Multi Object Editing supporting Editors</para>
    ///
    /// v1.0 08/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class MultiObjectEditorGUI
    {
        // ######################## FUNCTIONALITY ######################## //

        #region Bounds

        /// <summary>
        /// Make Center & Extents field for entering a Bounds.
        /// </summary>
        public static Bounds BoundsField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Bounds b = EditorGUILayout.BoundsField(p.boundsValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boundsValue = b;

            return b;
        }

        /// <summary>
        /// Make Center & Extents field for entering a Bounds.
        /// </summary>
        public static Bounds BoundsField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Bounds b = EditorGUILayout.BoundsField(label, p.boundsValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boundsValue = b;

            return b;
        }

        /// <summary>
        /// Make Center & Extents field for entering a Bounds.
        /// </summary>
        public static Bounds BoundsField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Bounds b = EditorGUILayout.BoundsField(label, p.boundsValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boundsValue = b;

            return b;
        }

        #endregion


        #region BoundsInt

        /// <summary>
        /// Make Position & Size field for entering a BoundsInt.
        /// </summary>
        public static BoundsInt BoundsIntField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            BoundsInt b = EditorGUILayout.BoundsIntField(p.boundsIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boundsIntValue = b;

            return b;
        }

        /// <summary>
        /// Make Position & Size field for entering a BoundsInt.
        /// </summary>
        public static BoundsInt BoundsIntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            BoundsInt b = EditorGUILayout.BoundsIntField(label, p.boundsIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boundsIntValue = b;

            return b;
        }

        /// <summary>
        /// Make Position & Size field for entering a BoundsInt.
        /// </summary>
        public static BoundsInt BoundsIntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            BoundsInt b = EditorGUILayout.BoundsIntField(label, p.boundsIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boundsIntValue = b;

            return b;
        }

        #endregion


        #region Color

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Color c = EditorGUILayout.ColorField(p.colorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Color c = EditorGUILayout.ColorField(label, p.colorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Color c = EditorGUILayout.ColorField(label, p.colorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedObject serializedObject, string property, string label, bool showEyedropper, bool showAlpha, bool hdr, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Color c = EditorGUILayout.ColorField(new GUIContent(label), p.colorValue, showEyedropper, showAlpha, hdr, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedObject serializedObject, string property, GUIContent label, bool showEyedropper, bool showAlpha, bool hdr, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Color c = EditorGUILayout.ColorField(label, p.colorValue, showEyedropper, showAlpha, hdr, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.colorValue = c;

            return c;
        }

        #endregion


        #region AnimationCurve

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(p.animationCurveValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.animationCurveValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(label, p.animationCurveValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.animationCurveValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(label, p.animationCurveValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.animationCurveValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, Color color, Rect ranges, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(p.animationCurveValue, color, ranges, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.animationCurveValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, string label, Color color, Rect ranges, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(label, p.animationCurveValue, color, ranges, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.animationCurveValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, GUIContent label, Color color, Rect ranges, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(label, p.animationCurveValue, color, ranges, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.animationCurveValue = c;

            return c;
        }

        #endregion


        #region DelayedDouble

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(p.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(p.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(label, p.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(label, p.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(label, p.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(label, p.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        #endregion


        #region DelayedFloat

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.DelayedFloatField(p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.DelayedFloatField(p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.DelayedFloatField(label, p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.DelayedFloatField(label, p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.DelayedFloatField(label, p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.DelayedFloatField(label, p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        #endregion


        #region DelayedInt

        /// <summary>
        /// Make a delayed text field for entering integers.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.DelayedIntField(p.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a delayed text field for entering integers.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.DelayedIntField(p.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a delayed text field for entering integers.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.DelayedIntField(label, p.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a delayed text field for entering integers.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.DelayedIntField(label, p.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a delayed text field for entering integers.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.DelayedIntField(label, p.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a delayed text field for entering integers.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.DelayedIntField(label, p.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        #endregion


        #region DelayedText

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.DelayedTextField(p.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.DelayedTextField(p.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.DelayedTextField(label, p.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.DelayedTextField(label, p.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.DelayedTextField(label, p.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.DelayedTextField(label, p.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        #endregion


        #region Double

        /// <summary>
        /// Make a text field for entering double values.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(p.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering double values.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(p.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering double values.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(label, p.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering double values.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(label, p.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering double values.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(label, p.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering double values.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(label, p.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.doubleValue = d;

            return d;
        }

        #endregion


        #region Float

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.FloatField(p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.FloatField(p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.FloatField(label, p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.FloatField(label, p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.FloatField(label, p.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.FloatField(label, p.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        #endregion


        #region Int

        /// <summary>
        /// Make a text field for entering integer values.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntField(p.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a text field for entering integer values.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntField(p.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a text field for entering integer values.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntField(label, p.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a text field for entering integer values.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntField(label, p.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a text field for entering integer values.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntField(label, p.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a text field for entering integer values.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntField(label, p.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        #endregion


        #region Long

        /// <summary>
        /// Make a text field for entering long integer values.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            long i = EditorGUILayout.LongField(p.longValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.longValue = i;

            return i;
        }

        /// <summary>
        /// Make a text field for entering long integer values.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            long i = EditorGUILayout.LongField(p.longValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.longValue = i;

            return i;
        }

        /// <summary>
        /// Make a text field for entering long integer values.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            long i = EditorGUILayout.LongField(label, p.longValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.longValue = i;

            return i;
        }

        /// <summary>
        /// Make a text field for entering long integer values.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            long i = EditorGUILayout.LongField(label, p.longValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.longValue = i;

            return i;
        }

        /// <summary>
        /// Make a text field for entering long integer values.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            long i = EditorGUILayout.LongField(label, p.longValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.longValue = i;

            return i;
        }

        /// <summary>
        /// Make a text field for entering long integer values.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            long i = EditorGUILayout.LongField(label, p.longValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.longValue = i;

            return i;
        }

        #endregion


        #region Text

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TextField(p.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TextField(p.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TextField(label, p.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TextField(label, p.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TextField(label, p.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TextField(label, p.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        #endregion


        #region TextArea

        /// <summary>
        /// Make a text area.
        /// </summary>
        public static string TextArea(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TextArea(p.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a text area.
        /// </summary>
        public static string TextArea(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TextArea(p.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        #endregion


        #region IntPopup

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, string[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(p.intValue, displayedOptions, optionValues, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, string[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(p.intValue, displayedOptions, optionValues, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, GUIContent[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(p.intValue, displayedOptions, optionValues, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, GUIContent[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(p.intValue, displayedOptions, optionValues, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, string label, string[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(label, p.intValue, displayedOptions, optionValues, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, string label, string[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(label, p.intValue, displayedOptions, optionValues, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, GUIContent label, GUIContent[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(label, p.intValue, displayedOptions, optionValues, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, GUIContent label, GUIContent[] displayedOptions, int[] optionValues, GUIStyle style,
            params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(label, p.intValue, displayedOptions, optionValues, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        #endregion


        #region IntSlider

        /// <summary>
        /// Make a slider the user can drag to change an integer value between a min and a max.
        /// </summary>
        public static int IntSlider(SerializedObject serializedObject, string property, int leftValue, int rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntSlider(p.intValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a slider the user can drag to change an integer value between a min and a max.
        /// </summary>
        public static int IntSlider(SerializedObject serializedObject, string property, string label, int leftValue, int rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntSlider(label, p.intValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a slider the user can drag to change an integer value between a min and a max.
        /// </summary>
        public static int IntSlider(SerializedObject serializedObject, string property, GUIContent label, int leftValue, int rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntSlider(label, p.intValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        #endregion


        #region Layer

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(p.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(p.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(label, p.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(label, p.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(label, p.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(label, p.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        #endregion


        #region Mask

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, GUIContent label, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(label, p.intValue, displayedOptions, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, string label, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(label, p.intValue, displayedOptions, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, GUIContent label, string[] displayedOptions, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(label, p.intValue, displayedOptions, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, string label, string[] displayedOptions, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(label, p.intValue, displayedOptions, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(p.intValue, displayedOptions, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, string[] displayedOptions, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(p.intValue, displayedOptions, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.intValue = i;

            return i;
        }

        #endregion


        #region MinMaxSlider

        /// <summary>
        /// Make a special slider the user can use to specify a range between a min and a max.
        /// </summary>
        public static void MinMaxSlider(SerializedObject serializedObject, string propertyMin, string propertyMax, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty min = serializedObject.FindProperty(propertyMin);
            SerializedProperty max = serializedObject.FindProperty(propertyMax);
            EditorGUI.showMixedValue = min.hasMultipleDifferentValues || max.hasMultipleDifferentValues;
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
        public static void MinMaxSlider(SerializedObject serializedObject, string propertyMin, string propertyMax, string label, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty min = serializedObject.FindProperty(propertyMin);
            SerializedProperty max = serializedObject.FindProperty(propertyMax);
            EditorGUI.showMixedValue = min.hasMultipleDifferentValues || max.hasMultipleDifferentValues;
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
        public static void MinMaxSlider(SerializedObject serializedObject, string propertyMin, string propertyMax, GUIContent label, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty min = serializedObject.FindProperty(propertyMin);
            SerializedProperty max = serializedObject.FindProperty(propertyMax);
            EditorGUI.showMixedValue = min.hasMultipleDifferentValues || max.hasMultipleDifferentValues;
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


        #region Rect

        /// <summary>
        /// Make an X, Y, W & H field for entering a Rect.
        /// </summary>
        public static Rect RectField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Rect r = EditorGUILayout.RectField(p.rectValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.rectValue = r;

            return r;
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a Rect.
        /// </summary>
        public static Rect RectField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Rect r = EditorGUILayout.RectField(label, p.rectValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.rectValue = r;

            return r;
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a Rect.
        /// </summary>
        public static Rect RectField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Rect r = EditorGUILayout.RectField(label, p.rectValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.rectValue = r;

            return r;
        }

        #endregion


        #region RectInt

        /// <summary>
        /// Make an X, Y, W & H field for entering a RectInt.
        /// </summary>
        public static RectInt RectIntField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            RectInt r = EditorGUILayout.RectIntField(p.rectIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.rectIntValue = r;

            return r;
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a RectInt.
        /// </summary>
        public static RectInt RectIntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            RectInt r = EditorGUILayout.RectIntField(label, p.rectIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.rectIntValue = r;

            return r;
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a RectInt.
        /// </summary>
        public static RectInt RectIntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            RectInt r = EditorGUILayout.RectIntField(label, p.rectIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.rectIntValue = r;

            return r;
        }

        #endregion


        #region Slider

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(SerializedObject serializedObject, string property, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.Slider(p.floatValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(SerializedObject serializedObject, string property, string label, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.Slider(label, p.floatValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(SerializedObject serializedObject, string property, GUIContent label, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            float f = EditorGUILayout.Slider(label, p.floatValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.floatValue = f;

            return f;
        }

        #endregion


        #region Tag

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(p.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(p.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(label, p.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(label, p.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(label, p.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(label, p.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.stringValue = s;

            return s;
        }

        #endregion


        #region Toggle

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(p.boolValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(label, p.boolValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(label, p.boolValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(p.boolValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(label, p.boolValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(label, p.boolValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boolValue = b;

            return b;
        }

        #endregion


        #region ToggleLeft

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            bool b = EditorGUILayout.ToggleLeft(label, p.boolValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            bool b = EditorGUILayout.ToggleLeft(label, p.boolValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedObject serializedObject, string property, string label, GUIStyle labelStyle, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            bool b = EditorGUILayout.ToggleLeft(label, p.boolValue, labelStyle, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedObject serializedObject, string property, GUIContent label, GUIStyle labelStyle, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            bool b = EditorGUILayout.ToggleLeft(label, p.boolValue, labelStyle, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.boolValue = b;

            return b;
        }

        #endregion


        #region Vector2

        /// <summary>
        /// Make an X & Y field for entering a Vector2.
        /// </summary>
        public static Vector2 Vector2Field(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Vector2 v = EditorGUILayout.Vector2Field(label, p.vector2Value, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vector2Value = v;

            return v;
        }

        /// <summary>
        /// Make an X & Y field for entering a Vector2.
        /// </summary>
        public static Vector2 Vector2Field(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Vector2 v = EditorGUILayout.Vector2Field(label, p.vector2Value, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vector2Value = v;

            return v;
        }

        #endregion


        #region Vector2Int

        /// <summary>
        /// Make an X & Y field for entering a Vector2Int.
        /// </summary>
        public static Vector2Int Vector2IntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Vector2Int v = EditorGUILayout.Vector2IntField(label, p.vector2IntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vector2IntValue = v;

            return v;
        }

        /// <summary>
        /// Make an X & Y field for entering a Vector2Int.
        /// </summary>
        public static Vector2Int Vector2IntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Vector2Int v = EditorGUILayout.Vector2IntField(label, p.vector2IntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vector2IntValue = v;

            return v;
        }

        #endregion


        #region Vector3

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3.
        /// </summary>
        public static Vector3 Vector3Field(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Vector3 v = EditorGUILayout.Vector3Field(label, p.vector3Value, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vector3Value = v;

            return v;
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3.
        /// </summary>
        public static Vector3 Vector3Field(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Vector3 v = EditorGUILayout.Vector3Field(label, p.vector3Value, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vector3Value = v;

            return v;
        }

        #endregion


        #region Vector3Int

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3Int.
        /// </summary>
        public static Vector3Int Vector3IntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Vector3Int v = EditorGUILayout.Vector3IntField(label, p.vector3IntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vector3IntValue = v;

            return v;
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3Int.
        /// </summary>
        public static Vector3Int Vector3IntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Vector3Int v = EditorGUILayout.Vector3IntField(label, p.vector3IntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vector3IntValue = v;

            return v;
        }

        #endregion


        #region Vector4

        /// <summary>
        /// Make an X, Y, Z & W field for entering a Vector4.
        /// </summary>
        public static Vector4 Vector4Field(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            Vector4 v = EditorGUILayout.Vector4Field(label, p.vector4Value, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.vector4Value = v;

            return v;
        }

        #endregion


        #region Object

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static UnityEngine.Object ObjectField(SerializedObject serializedObject, string property, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            UnityEngine.Object obj = EditorGUILayout.ObjectField(p.objectReferenceValue, objType, allowSceneObjects, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.objectReferenceValue = obj;

            return obj;
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static UnityEngine.Object ObjectField(SerializedObject serializedObject, string property, string label, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            UnityEngine.Object obj = EditorGUILayout.ObjectField(label, p.objectReferenceValue, objType, allowSceneObjects, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.objectReferenceValue = obj;

            return obj;
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static UnityEngine.Object ObjectField(SerializedObject serializedObject, string property, GUIContent label, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty p = serializedObject.FindProperty(property);
            EditorGUI.showMixedValue = p.hasMultipleDifferentValues;
            UnityEngine.Object obj = EditorGUILayout.ObjectField(label, p.objectReferenceValue, objType, allowSceneObjects, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                p.objectReferenceValue = obj;

            return obj;
        }

        #endregion
    }
}
#endif