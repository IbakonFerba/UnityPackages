#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace FK.Editor
{
    /// <summary>
    /// <para>Elements for Multi Object Editing supporting Editors</para>
    ///
    /// v2.0 08/2018
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
        public static Bounds BoundsField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Bounds b = EditorGUILayout.BoundsField(property.boundsValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boundsValue = b;

            return b;
        }

        /// <summary>
        /// Make Center & Extents field for entering a Bounds.
        /// </summary>
        public static Bounds BoundsField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Bounds b = EditorGUILayout.BoundsField(label, property.boundsValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boundsValue = b;

            return b;
        }

        /// <summary>
        /// Make Center & Extents field for entering a Bounds.
        /// </summary>
        public static Bounds BoundsField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Bounds b = EditorGUILayout.BoundsField(label, property.boundsValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boundsValue = b;

            return b;
        }

        /// <summary>
        /// Make Center & Extents field for entering a Bounds.
        /// </summary>
        public static Bounds BoundsField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return BoundsField(p, options);
        }

        /// <summary>
        /// Make Center & Extents field for entering a Bounds.
        /// </summary>
        public static Bounds BoundsField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return BoundsField(p, label, options);
        }

        /// <summary>
        /// Make Center & Extents field for entering a Bounds.
        /// </summary>
        public static Bounds BoundsField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return BoundsField(p, label, options);
        }

        #endregion


        #region BoundsInt

        /// <summary>
        /// Make Center & Extents field for entering a BoundsInt.
        /// </summary>
        public static BoundsInt BoundsIntField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            BoundsInt b = EditorGUILayout.BoundsIntField(property.boundsIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boundsIntValue = b;

            return b;
        }

        /// <summary>
        /// Make Center & Extents field for entering a BoundsInt.
        /// </summary>
        public static BoundsInt BoundsIntField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            BoundsInt b = EditorGUILayout.BoundsIntField(label, property.boundsIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boundsIntValue = b;

            return b;
        }

        /// <summary>
        /// Make Center & Extents field for entering a BoundsInt.
        /// </summary>
        public static BoundsInt BoundsIntField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            BoundsInt b = EditorGUILayout.BoundsIntField(label, property.boundsIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boundsIntValue = b;

            return b;
        }

        /// <summary>
        /// Make Center & Extents field for entering a BoundsInt.
        /// </summary>
        public static BoundsInt BoundsIntField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return BoundsIntField(p, options);
        }

        /// <summary>
        /// Make Center & Extents field for entering a BoundsInt.
        /// </summary>
        public static BoundsInt BoundsIntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return BoundsIntField(p, label, options);
        }

        /// <summary>
        /// Make Center & Extents field for entering a BoundsInt.
        /// </summary>
        public static BoundsInt BoundsIntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return BoundsIntField(p, label, options);
        }

        #endregion


        #region Color

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Color c = EditorGUILayout.ColorField(property.colorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Color c = EditorGUILayout.ColorField(label, property.colorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Color c = EditorGUILayout.ColorField(label, property.colorValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedProperty property, string label, bool showEyedropper, bool showAlpha, bool hdr, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Color c = EditorGUILayout.ColorField(new GUIContent(label), property.colorValue, showEyedropper, showAlpha, hdr, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.colorValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedProperty property, GUIContent label, bool showEyedropper, bool showAlpha, bool hdr, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Color c = EditorGUILayout.ColorField(label, property.colorValue, showEyedropper, showAlpha, hdr, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.colorValue = c;

            return c;
        }


        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ColorField(p, options);
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ColorField(p, label, options);
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ColorField(p, label, options);
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedObject serializedObject, string property, string label, bool showEyedropper, bool showAlpha, bool hdr, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ColorField(p, label, showEyedropper, showAlpha, hdr, options);
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// </summary>
        public static Color ColorField(SerializedObject serializedObject, string property, GUIContent label, bool showEyedropper, bool showAlpha, bool hdr, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ColorField(p, label, showEyedropper, showAlpha, hdr, options);
        }

        #endregion


        #region AnimationCurve

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(property.animationCurveValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.animationCurveValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(label, property.animationCurveValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.animationCurveValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(label, property.animationCurveValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.animationCurveValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedProperty property, Color color, Rect ranges, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(property.animationCurveValue, color, ranges, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.animationCurveValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedProperty property, string label, Color color, Rect ranges, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(label, property.animationCurveValue, color, ranges, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.animationCurveValue = c;

            return c;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedProperty property, GUIContent label, Color color, Rect ranges, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            AnimationCurve c = EditorGUILayout.CurveField(label, property.animationCurveValue, color, ranges, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.animationCurveValue = c;

            return c;
        }


        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return CurveField(p, options);
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return CurveField(p, label, options);
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return CurveField(p, label, options);
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, Color color, Rect ranges, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return CurveField(p, color, ranges, options);
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, string label, Color color, Rect ranges, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return CurveField(p, label, color, ranges, options);
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// </summary>
        public static AnimationCurve CurveField(SerializedObject serializedObject, string property, GUIContent label, Color color, Rect ranges, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return CurveField(p, label, color, ranges, options);
        }

        #endregion


        #region DelayedDouble

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(property.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(property.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(label, property.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(label, property.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(label, property.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DelayedDoubleField(label, property.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }


        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedDoubleField(p, options);
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedDoubleField(p, style, options);
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedDoubleField(p, label, options);
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedDoubleField(p, label, style, options);
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedDoubleField(p, label, options);
        }

        /// <summary>
        /// Make a delayed text field for entering doubles.
        /// </summary>
        public static double DelayedDoubleField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedDoubleField(p, label, style, options);
        }

        #endregion


        #region DelayedFloat

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.DelayedFloatField(property.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.DelayedFloatField(property.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.DelayedFloatField(label, property.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.DelayedFloatField(label, property.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.DelayedFloatField(label, property.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.DelayedFloatField(label, property.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }


        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedFloatField(p, options);
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedFloatField(p, style, options);
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedFloatField(p, label, options);
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedFloatField(p, label, style, options);
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedFloatField(p, label, options);
        }

        /// <summary>
        /// Make a delayed text field for entering floats.
        /// </summary>
        public static float DelayedFloatField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedFloatField(p, label, style, options);
        }

        #endregion


        #region DelayedInt

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.DelayedIntField(property.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.DelayedIntField(property.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.DelayedIntField(label, property.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.DelayedIntField(label, property.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.DelayedIntField(label, property.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.DelayedIntField(label, property.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }


        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedIntField(p, options);
        }

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedIntField(p, style, options);
        }

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedIntField(p, label, options);
        }

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedIntField(p, label, style, options);
        }

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedIntField(p, label, options);
        }

        /// <summary>
        /// Make a delayed text field for entering ints.
        /// </summary>
        public static int DelayedIntField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedIntField(p, label, style, options);
        }

        #endregion


        #region DelayedText

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.DelayedTextField(property.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.DelayedTextField(property.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.DelayedTextField(label, property.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.DelayedTextField(label, property.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.DelayedTextField(label, property.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.DelayedTextField(label, property.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }


        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedTextField(p, options);
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedTextField(p, style, options);
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedTextField(p, label, options);
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedTextField(p, label, style, options);
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedTextField(p, label, options);
        }

        /// <summary>
        /// Make a delayed text field.
        /// </summary>
        public static string DelayedTextField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DelayedTextField(p, label, style, options);
        }

        #endregion


        #region Double

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(property.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(property.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(label, property.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(label, property.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(label, property.doubleValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            double d = EditorGUILayout.DoubleField(label, property.doubleValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = d;

            return d;
        }


        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DoubleField(p, options);
        }

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DoubleField(p, style, options);
        }

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DoubleField(p, label, options);
        }

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DoubleField(p, label, style, options);
        }

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DoubleField(p, label, options);
        }

        /// <summary>
        /// Make a text field for entering doubles.
        /// </summary>
        public static double DoubleField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return DoubleField(p, label, style, options);
        }

        #endregion


        #region Float

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.FloatField(property.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.FloatField(property.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.FloatField(label, property.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.FloatField(label, property.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.FloatField(label, property.floatValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float d = EditorGUILayout.FloatField(label, property.floatValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = d;

            return d;
        }


        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return FloatField(p, options);
        }

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return FloatField(p, style, options);
        }

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return FloatField(p, label, options);
        }

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return FloatField(p, label, style, options);
        }

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return FloatField(p, label, options);
        }

        /// <summary>
        /// Make a text field for entering floats.
        /// </summary>
        public static float FloatField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return FloatField(p, label, style, options);
        }

        #endregion


        #region Int

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.IntField(property.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.IntField(property.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.IntField(label, property.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.IntField(label, property.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.IntField(label, property.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int d = EditorGUILayout.IntField(label, property.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = d;

            return d;
        }


        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntField(p, options);
        }

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntField(p, style, options);
        }

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntField(p, label, options);
        }

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntField(p, label, style, options);
        }

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntField(p, label, options);
        }

        /// <summary>
        /// Make a text field for entering ints.
        /// </summary>
        public static int IntField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntField(p, label, style, options);
        }

        #endregion


        #region Long

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            long d = EditorGUILayout.LongField(property.longValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.longValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            long d = EditorGUILayout.LongField(property.longValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.longValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            long d = EditorGUILayout.LongField(label, property.longValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.longValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            long d = EditorGUILayout.LongField(label, property.longValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.longValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            long d = EditorGUILayout.LongField(label, property.longValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.longValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            long d = EditorGUILayout.LongField(label, property.longValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.longValue = d;

            return d;
        }


        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LongField(p, options);
        }

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LongField(p, style, options);
        }

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LongField(p, label, options);
        }

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LongField(p, label, style, options);
        }

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LongField(p, label, options);
        }

        /// <summary>
        /// Make a text field for entering longs.
        /// </summary>
        public static long LongField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LongField(p, label, style, options);
        }

        #endregion


        #region Text

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.TextField(property.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.TextField(property.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.TextField(label, property.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.TextField(label, property.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.TextField(label, property.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string d = EditorGUILayout.TextField(label, property.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = d;

            return d;
        }


        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TextField(p, options);
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TextField(p, style, options);
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TextField(p, label, options);
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TextField(p, label, style, options);
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TextField(p, label, options);
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        public static string TextField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TextField(p, label, style, options);
        }

        #endregion


        #region TextArea

        /// <summary>
        /// Make a text area.
        /// </summary>
        public static string TextArea(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string s = EditorGUILayout.TextArea(property.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a text area.
        /// </summary>
        public static string TextArea(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string s = EditorGUILayout.TextArea(property.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = s;

            return s;
        }


        /// <summary>
        /// Make a text area.
        /// </summary>
        public static string TextArea(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TextArea(p, options);
        }

        /// <summary>
        /// Make a text area.
        /// </summary>
        public static string TextArea(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TextArea(p, style, options);
        }

        #endregion


        #region IntPopup

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedProperty property, string[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(property.intValue, displayedOptions, optionValues, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedProperty property, string[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(property.intValue, displayedOptions, optionValues, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedProperty property, GUIContent[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(property.intValue, displayedOptions, optionValues, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedProperty property, GUIContent[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(property.intValue, displayedOptions, optionValues, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedProperty property, string label, string[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(label, property.intValue, displayedOptions, optionValues, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedProperty property, string label, string[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(label, property.intValue, displayedOptions, optionValues, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedProperty property, GUIContent label, GUIContent[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(label, property.intValue, displayedOptions, optionValues, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedProperty property, GUIContent label, GUIContent[] displayedOptions, int[] optionValues, GUIStyle style,
            params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntPopup(label, property.intValue, displayedOptions, optionValues, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }


        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, string[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntPopup(p, displayedOptions, optionValues, options);
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, string[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntPopup(p, displayedOptions, optionValues, style, options);
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, GUIContent[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntPopup(p, displayedOptions, optionValues, options);
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, GUIContent[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntPopup(p, displayedOptions, optionValues, style, options);
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, string label, string[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntPopup(p, label, displayedOptions, optionValues, options);
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, string label, string[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntPopup(p, label, displayedOptions, optionValues, style, options);
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, GUIContent label, GUIContent[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntPopup(p, label, displayedOptions, optionValues, options);
        }

        /// <summary>
        /// Make an integer popup selection field.
        /// </summary>
        public static int IntPopup(SerializedObject serializedObject, string property, GUIContent label, GUIContent[] displayedOptions, int[] optionValues, GUIStyle style,
            params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntPopup(p, label, displayedOptions, optionValues, style, options);
        }

        #endregion


        #region IntSlider

        /// <summary>
        /// Make a slider the user can drag to change an integer value between a min and a max.
        /// </summary>
        public static int IntSlider(SerializedProperty property, int leftValue, int rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntSlider(property.intValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a slider the user can drag to change an integer value between a min and a max.
        /// </summary>
        public static int IntSlider(SerializedProperty property, string label, int leftValue, int rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntSlider(label, property.intValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a slider the user can drag to change an integer value between a min and a max.
        /// </summary>
        public static int IntSlider(SerializedProperty property, GUIContent label, int leftValue, int rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.IntSlider(label, property.intValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }


        /// <summary>
        /// Make a slider the user can drag to change an integer value between a min and a max.
        /// </summary>
        public static int IntSlider(SerializedObject serializedObject, string property, int leftValue, int rightValue, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntSlider(p, leftValue, rightValue, options);
        }

        /// <summary>
        /// Make a slider the user can drag to change an integer value between a min and a max.
        /// </summary>
        public static int IntSlider(SerializedObject serializedObject, string property, string label, int leftValue, int rightValue, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntSlider(p, label, leftValue, rightValue, options);
        }

        /// <summary>
        /// Make a slider the user can drag to change an integer value between a min and a max.
        /// </summary>
        public static int IntSlider(SerializedObject serializedObject, string property, GUIContent label, int leftValue, int rightValue, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return IntSlider(p, label, leftValue, rightValue, options);
        }

        #endregion


        #region Layer

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(property.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(property.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(label, property.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(label, property.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(label, property.intValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.LayerField(label, property.intValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }


        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LayerField(p, options);
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LayerField(p, style, options);
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LayerField(p, label, options);
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LayerField(p, label, style, options);
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LayerField(p, label, options);
        }

        /// <summary>
        /// Make a layer selection field.
        /// </summary>
        public static int LayerField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return LayerField(p, label, style, options);
        }

        #endregion


        #region Mask

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedProperty property, GUIContent label, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(label, property.intValue, displayedOptions, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedProperty property, string label, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(label, property.intValue, displayedOptions, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedProperty property, GUIContent label, string[] displayedOptions, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(label, property.intValue, displayedOptions, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedProperty property, string label, string[] displayedOptions, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(label, property.intValue, displayedOptions, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedProperty property, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(property.intValue, displayedOptions, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedProperty property, string[] displayedOptions, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int i = EditorGUILayout.MaskField(property.intValue, displayedOptions, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = i;

            return i;
        }


        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, GUIContent label, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return MaskField(p, label, displayedOptions, style, options);
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, string label, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return MaskField(p, label, displayedOptions, style, options);
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, GUIContent label, string[] displayedOptions, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return MaskField(p, label, displayedOptions, options);
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, string label, string[] displayedOptions, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return MaskField(p, label, displayedOptions, options);
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return MaskField(p, displayedOptions, style, options);
        }

        /// <summary>
        /// Make a field for masks.
        /// </summary>
        public static int MaskField(SerializedObject serializedObject, string property, string[] displayedOptions, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return MaskField(p, displayedOptions, options);
        }

        #endregion


        #region MinMaxSlider

        /// <summary>
        /// Make a special slider the user can use to specify a range between a propertyMin and a propertyMax.
        /// </summary>
        public static void MinMaxSlider(SerializedProperty propertyMin, SerializedProperty propertyMax, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = propertyMin.hasMultipleDifferentValues || propertyMax.hasMultipleDifferentValues;
            float m = propertyMin.floatValue;
            float ma = propertyMax.floatValue;
            EditorGUILayout.MinMaxSlider(ref m, ref ma, minLimit, maxLimit, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                propertyMin.floatValue = m;
                propertyMax.floatValue = ma;
            }
        }

        /// <summary>
        /// Make a special slider the user can use to specify a range between a propertyMin and a propertyMax.
        /// </summary>
        public static void MinMaxSlider(SerializedProperty propertyMin, SerializedProperty propertyMax, string label, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = propertyMin.hasMultipleDifferentValues || propertyMax.hasMultipleDifferentValues;
            float m = propertyMin.floatValue;
            float ma = propertyMax.floatValue;
            EditorGUILayout.MinMaxSlider(label, ref m, ref ma, minLimit, maxLimit, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                propertyMin.floatValue = m;
                propertyMax.floatValue = ma;
            }
        }

        /// <summary>
        /// Make a special slider the user can use to specify a range between a propertyMin and a propertyMax.
        /// </summary>
        public static void MinMaxSlider(SerializedProperty propertyMin, SerializedProperty propertyMax, GUIContent label, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = propertyMin.hasMultipleDifferentValues || propertyMax.hasMultipleDifferentValues;
            float m = propertyMin.floatValue;
            float ma = propertyMax.floatValue;
            EditorGUILayout.MinMaxSlider(label, ref m, ref ma, minLimit, maxLimit, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                propertyMin.floatValue = m;
                propertyMax.floatValue = ma;
            }
        }


        /// <summary>
        /// Make a special slider the user can use to specify a range between a min and a max.
        /// </summary>
        public static void MinMaxSlider(SerializedObject serializedObject, string propertyMin, string propertyMax, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            SerializedProperty min = serializedObject.FindProperty(propertyMin);
            SerializedProperty max = serializedObject.FindProperty(propertyMax);
            MinMaxSlider(min, max, minLimit, maxLimit, options);
        }

        /// <summary>
        /// Make a special slider the user can use to specify a range between a min and a max.
        /// </summary>
        public static void MinMaxSlider(SerializedObject serializedObject, string propertyMin, string propertyMax, string label, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            SerializedProperty min = serializedObject.FindProperty(propertyMin);
            SerializedProperty max = serializedObject.FindProperty(propertyMax);
            MinMaxSlider(min, max, label, minLimit, maxLimit, options);
        }

        /// <summary>
        /// Make a special slider the user can use to specify a range between a min and a max.
        /// </summary>
        public static void MinMaxSlider(SerializedObject serializedObject, string propertyMin, string propertyMax, GUIContent label, float minLimit, float maxLimit, params GUILayoutOption[] options)
        {
            SerializedProperty min = serializedObject.FindProperty(propertyMin);
            SerializedProperty max = serializedObject.FindProperty(propertyMax);
            MinMaxSlider(min, max, label, minLimit, maxLimit, options);
        }

        #endregion


        #region Rect

        /// <summary>
        /// Make an X, Y, W & H field for entering a Rect.
        /// </summary>
        public static Rect RectField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Rect r = EditorGUILayout.RectField(property.rectValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.rectValue = r;

            return r;
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a Rect.
        /// </summary>
        public static Rect RectField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Rect r = EditorGUILayout.RectField(label, property.rectValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.rectValue = r;

            return r;
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a Rect.
        /// </summary>
        public static Rect RectField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Rect r = EditorGUILayout.RectField(label, property.rectValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.rectValue = r;

            return r;
        }


        /// <summary>
        /// Make an X, Y, W & H field for entering a Rect.
        /// </summary>
        public static Rect RectField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return RectField(p, options);
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a Rect.
        /// </summary>
        public static Rect RectField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return RectField(p, label, options);
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a Rect.
        /// </summary>
        public static Rect RectField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return RectField(p, label, options);
        }

        #endregion


        #region RectInt

        /// <summary>
        /// Make an X, Y, W & H field for entering a RectInt.
        /// </summary>
        public static RectInt RectIntField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            RectInt r = EditorGUILayout.RectIntField(property.rectIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.rectIntValue = r;

            return r;
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a RectInt.
        /// </summary>
        public static RectInt RectIntField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            RectInt r = EditorGUILayout.RectIntField(label, property.rectIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.rectIntValue = r;

            return r;
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a RectInt.
        /// </summary>
        public static RectInt RectIntField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            RectInt r = EditorGUILayout.RectIntField(label, property.rectIntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.rectIntValue = r;

            return r;
        }


        /// <summary>
        /// Make an X, Y, W & H field for entering a RectInt.
        /// </summary>
        public static RectInt RectIntField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return RectIntField(p, options);
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a RectInt.
        /// </summary>
        public static RectInt RectIntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return RectIntField(p, label, options);
        }

        /// <summary>
        /// Make an X, Y, W & H field for entering a RectInt.
        /// </summary>
        public static RectInt RectIntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return RectIntField(p, label, options);
        }

        #endregion


        #region Slider

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(SerializedProperty property, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float f = EditorGUILayout.Slider(property.floatValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(SerializedProperty property, string label, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float f = EditorGUILayout.Slider(label, property.floatValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = f;

            return f;
        }

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(SerializedProperty property, GUIContent label, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            float f = EditorGUILayout.Slider(label, property.floatValue, leftValue, rightValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.floatValue = f;

            return f;
        }


        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(SerializedObject serializedObject, string property, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Slider(p, leftValue, rightValue, options);
        }

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(SerializedObject serializedObject, string property, string label, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Slider(p, label, leftValue, rightValue, options);
        }

        /// <summary>
        /// Make a slider the user can drag to change a value between a min and a max.
        /// </summary>
        public static float Slider(SerializedObject serializedObject, string property, GUIContent label, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Slider(p, label, leftValue, rightValue, options);
        }

        #endregion


        #region Tag

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(property.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(property.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(label, property.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(label, property.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(label, property.stringValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = s;

            return s;
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string s = EditorGUILayout.TagField(label, property.stringValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.stringValue = s;

            return s;
        }


        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TagField(p, options);
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TagField(p, style, options);
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TagField(p, label, options);
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TagField(p, label, style, options);
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TagField(p, label, options);
        }

        /// <summary>
        /// Make a tag selection field.
        /// </summary>
        public static string TagField(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return TagField(p, label, style, options);
        }

        #endregion


        #region Toggle

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedProperty property, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(property.boolValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(label, property.boolValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(label, property.boolValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedProperty property, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(property.boolValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedProperty property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(label, property.boolValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedProperty property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            bool b = EditorGUILayout.Toggle(label, property.boolValue, style, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boolValue = b;

            return b;
        }


        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Toggle(p, options);
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Toggle(p, label, options);
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Toggle(p, label, options);
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Toggle(p, style, options);
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Toggle(p, label, style, options);
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        public static bool Toggle(SerializedObject serializedObject, string property, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Toggle(p, label, style, options);
        }

        #endregion


        #region ToggleLeft

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            bool b = EditorGUILayout.ToggleLeft(label, property.boolValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            bool b = EditorGUILayout.ToggleLeft(label, property.boolValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedProperty property, string label, GUIStyle labelStyle, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            bool b = EditorGUILayout.ToggleLeft(label, property.boolValue, labelStyle, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boolValue = b;

            return b;
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedProperty property, GUIContent label, GUIStyle labelStyle, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            bool b = EditorGUILayout.ToggleLeft(label, property.boolValue, labelStyle, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.boolValue = b;

            return b;
        }


        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ToggleLeft(p, label, options);
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ToggleLeft(p, label, options);
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedObject serializedObject, string property, string label, GUIStyle labelStyle, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ToggleLeft(p, label, labelStyle, options);
        }

        /// <summary>
        /// Make a toggle field where the toggle is to the left and the label immediately to the right of it.
        /// </summary>
        public static bool ToggleLeft(SerializedObject serializedObject, string property, GUIContent label, GUIStyle labelStyle, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ToggleLeft(p, label, labelStyle, options);
        }

        #endregion


        #region Vector2

        /// <summary>
        /// Make an X & Y field for entering a Vector2.
        /// </summary>
        public static Vector2 Vector2Field(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Vector2 v = EditorGUILayout.Vector2Field(label, property.vector2Value, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.vector2Value = v;

            return v;
        }

        /// <summary>
        /// Make an X & Y field for entering a Vector2.
        /// </summary>
        public static Vector2 Vector2Field(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Vector2 v = EditorGUILayout.Vector2Field(label, property.vector2Value, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.vector2Value = v;

            return v;
        }


        /// <summary>
        /// Make an X & Y field for entering a Vector2.
        /// </summary>
        public static Vector2 Vector2Field(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Vector2Field(p, label, options);
        }

        /// <summary>
        /// Make an X & Y field for entering a Vector2.
        /// </summary>
        public static Vector2 Vector2Field(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Vector2Field(p, label, options);
        }

        #endregion


        #region Vector2Int

        /// <summary>
        /// Make an X & Y field for entering a Vector2Int.
        /// </summary>
        public static Vector2Int Vector2IntField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Vector2Int v = EditorGUILayout.Vector2IntField(label, property.vector2IntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.vector2IntValue = v;

            return v;
        }

        /// <summary>
        /// Make an X & Y field for entering a Vector2Int.
        /// </summary>
        public static Vector2Int Vector2IntField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Vector2Int v = EditorGUILayout.Vector2IntField(label, property.vector2IntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.vector2IntValue = v;

            return v;
        }


        /// <summary>
        /// Make an X & Y field for entering a Vector2Int.
        /// </summary>
        public static Vector2Int Vector2IntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Vector2IntField(p, label, options);
        }

        /// <summary>
        /// Make an X & Y field for entering a Vector2Int.
        /// </summary>
        public static Vector2Int Vector2IntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Vector2IntField(p, label, options);
        }

        #endregion


        #region Vector3

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3.
        /// </summary>
        public static Vector3 Vector3Field(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Vector3 v = EditorGUILayout.Vector3Field(label, property.vector3Value, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.vector3Value = v;

            return v;
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3.
        /// </summary>
        public static Vector3 Vector3Field(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Vector3 v = EditorGUILayout.Vector3Field(label, property.vector3Value, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.vector3Value = v;

            return v;
        }


        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3.
        /// </summary>
        public static Vector3 Vector3Field(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Vector3Field(p, label, options);
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3.
        /// </summary>
        public static Vector3 Vector3Field(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Vector3Field(p, label, options);
        }

        #endregion


        #region Vector3Int

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3Int.
        /// </summary>
        public static Vector3Int Vector3IntField(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Vector3Int v = EditorGUILayout.Vector3IntField(label, property.vector3IntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.vector3IntValue = v;

            return v;
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3Int.
        /// </summary>
        public static Vector3Int Vector3IntField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Vector3Int v = EditorGUILayout.Vector3IntField(label, property.vector3IntValue, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.vector3IntValue = v;

            return v;
        }


        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3Int.
        /// </summary>
        public static Vector3Int Vector3IntField(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Vector3IntField(p, label, options);
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3Int.
        /// </summary>
        public static Vector3Int Vector3IntField(SerializedObject serializedObject, string property, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Vector3IntField(p, label, options);
        }

        #endregion


        #region Vector4

        /// <summary>
        /// Make an X, Y, Z & W field for entering a Vector4.
        /// </summary>
        public static Vector4 Vector4Field(SerializedProperty property, string label, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            Vector4 v = EditorGUILayout.Vector4Field(label, property.vector4Value, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.vector4Value = v;

            return v;
        }


        /// <summary>
        /// Make an X, Y, Z & W field for entering a Vector4.
        /// </summary>
        public static Vector4 Vector4Field(SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return Vector4Field(p, label, options);
        }

        #endregion


        #region Object

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static UnityEngine.Object ObjectField(SerializedProperty property, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            UnityEngine.Object obj = EditorGUILayout.ObjectField(property.objectReferenceValue, objType, allowSceneObjects, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.objectReferenceValue = obj;

            return obj;
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static UnityEngine.Object ObjectField(SerializedProperty property, string label, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            UnityEngine.Object obj = EditorGUILayout.ObjectField(label, property.objectReferenceValue, objType, allowSceneObjects, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.objectReferenceValue = obj;

            return obj;
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static UnityEngine.Object ObjectField(SerializedProperty property, GUIContent label, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            UnityEngine.Object obj = EditorGUILayout.ObjectField(label, property.objectReferenceValue, objType, allowSceneObjects, options);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.objectReferenceValue = obj;

            return obj;
        }


        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static UnityEngine.Object ObjectField(SerializedObject serializedObject, string property, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ObjectField(p, objType, allowSceneObjects, options);
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static UnityEngine.Object ObjectField(SerializedObject serializedObject, string property, string label, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ObjectField(p, label, objType, allowSceneObjects, options);
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        public static UnityEngine.Object ObjectField(SerializedObject serializedObject, string property, GUIContent label, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            SerializedProperty p = serializedObject.FindProperty(property);
            return ObjectField(p, label, objType, allowSceneObjects, options);
        }

        #endregion
    }
}
#endif