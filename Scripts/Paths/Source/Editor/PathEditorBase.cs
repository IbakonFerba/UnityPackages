using UnityEngine;
using UnityEditor;
using FK.Editor;
using FK.Paths;

/// <summary>
/// <para>This is the base a custom editor for Paths</para>
/// <para>This class was created with the help of this tutorial: https://catlikecoding.com/unity/tutorials/curves-and-splines/</para>
/// 
/// v1.0 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class PathEditorBase : Editor
{
    // ######################## PROTECTED VARS ######################## //
    /// <summary>
    /// Transform of the Path Object
    /// </summary>
    protected Transform HandleTransform;

    /// <summary>
    /// Rotation of the Path Object
    /// </summary>
    protected Quaternion HandleRotation;

    /// <summary>
    /// Should we automatically presample after each change?
    /// </summary>
    protected bool AutoPresampleInEditMode = false;

    /// <summary>
    /// The index of the selected point
    /// </summary>
    protected int SelectedIndex = -1;

    // ######################## PRIVATE VARS ######################## //
    private Path _path;

    /// <summary>
    /// The number of steps along each curve for displaying direction
    /// </summary>
    private const int STEPS_PER_CURVE = 10;

    /// <summary>
    /// The scale for displaying direction
    /// </summary>
    private const float DIRECTION_SCALE = 0.5f;

    /// <summary>
    /// Size of the Handle display
    /// </summary>
    private const float HANDLE_SIZE = 0.04f;

    /// <summary>
    /// Size of the Handle Pick
    /// </summary>
    private const float PICK_SIZE = 0.06f;

    // ######################## UNITY EVENT FUNCTIONS ######################## //
    protected virtual void OnSceneGUI()
    {
        // get values
        _path = target as Path;
        HandleTransform = _path.transform;
        HandleRotation = Tools.pivotRotation == PivotRotation.Local ? _path.transform.rotation : Quaternion.identity;
    }

    public override void OnInspectorGUI()
    {
        // update serializedObject and get path
        serializedObject.Update();
        _path = target as Path;

        
        // display presampling options
        EditorGUILayout.LabelField("Presampling", EditorStyles.whiteLabel);
        AutoPresampleInEditMode =
            EditorGUILayout.Toggle(new GUIContent("Auto Presample in Editmode", "If true, the Path gets automatically presampled for uniform interpolation along it whenever something changes"),
                AutoPresampleInEditMode);

        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        bool autoPresample =
            EditorGUILayout.Toggle(new GUIContent("Auto Presample in Playmode", "If true, the Path gets automatically presampled for uniform interpolation along it whenever something changes"),
                _path.AutoPresample);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_path, "Toggle Auto Presample");
            EditorUtility.SetDirty(_path);
            _path.AutoPresample = autoPresample;
        }


        MultiObjectEditorGUI.Toggle(serializedObject, "_presampleOnAwake",
            new GUIContent("Presample On Awake", "If true, the Path gets automatically presampled for uniform interpolation along it on Awake"));

        MultiObjectEditorGUI.FloatField(serializedObject, "_presampleSearchStepSize",
            new GUIContent("Search Step Size", "Interpolation step size relativ to the path length for resampling for uniform interpolation"));
        MultiObjectEditorGUI.IntField(serializedObject, "_presampleResolution", new GUIContent("Presample Resolution", "Resolution of the presampled curve for uniform interpolation"));
        if (GUILayout.Button("Delete Presampeled Points"))
        {
            Undo.RecordObject(_path, "Delete Presample");
            SerializedProperty linearPoints = serializedObject.FindProperty("LinearPoints");
            linearPoints.ClearArray();
            EditorUtility.SetDirty(_path);
        }

        
        
        GUILayout.Space(50);

        
        // looping
        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", _path.IsLoop);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_path, "Toggle Loop");
            EditorUtility.SetDirty(_path);
            _path.IsLoop = loop;
            if (AutoPresampleInEditMode)
                _path.Presample();
        }

        
        // display inspector for the currently selected point
        if (SelectedIndex >= 0 && SelectedIndex < _path.ControlPointCount)
        {
            DrawSelectedPointInspector();
            EditorGUILayout.Space();
        }

        // buttons
        if (GUILayout.Button("Add Segment"))
        {
            Undo.RecordObject(_path, "Add Segment");
            _path.AddSegment();
            EditorUtility.SetDirty(_path);
            if (AutoPresampleInEditMode)
                _path.Presample();
        }

        if (GUILayout.Button("Remove Segment"))
        {
            Undo.RecordObject(_path, "Remove Segment");
            _path.RemoveSegment();
            EditorUtility.SetDirty(_path);
            if (AutoPresampleInEditMode)
                _path.Presample();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button(new GUIContent("Presample", "This calculates a number of points along the path to allow uniform interpolation along the path"), GUILayout.Height(50)))
        {
            Undo.RecordObject(_path, "Presample Path");
            _path.Presample();
            EditorUtility.SetDirty(_path);
        }

        // save serialized Object
        serializedObject.ApplyModifiedProperties();
    }

    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Displays a point of the Path
    /// </summary>
    /// <param name="index"></param>
    /// <param name="handleColor"></param>
    /// <returns></returns>
    protected Vector3 ShowPoint(int index, Color handleColor)
    {
        // get the point in world space
        Vector3 point = HandleTransform.TransformPoint(_path.GetControlPoint(index));

        // calculate the handle size
        float size = HandleUtility.GetHandleSize(point);
        
        // if this is the start of the path, double the handle size
        if (index == 0)
            size *= 2f;
        
        // display a Button handle for the Point
        Handles.color = handleColor;
        if (Handles.Button(point, HandleRotation, size * HANDLE_SIZE, size * PICK_SIZE, Handles.DotHandleCap))
        {
            SelectedIndex = index;
            Repaint();
        }

        // if this point is not selected, return
        if (SelectedIndex != index)
            return point;

        // if this point is selected, display a Position Handle
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, HandleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_path, "Move Point");
            EditorUtility.SetDirty(_path);
            _path.SetControlPoint(index, point, Space.World);
            if (AutoPresampleInEditMode)
                _path.Presample();
        }

        return point;
    }

    /// <summary>
    /// Displays the directions along a path
    /// </summary>
    protected void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = _path.GetPosition(0f);
        Handles.DrawLine(point, point + _path.GetDirection(0f) * DIRECTION_SCALE);
        int steps = STEPS_PER_CURVE * _path.SegmentCount;
        for (int i = 1; i <= steps; i++)
        {
            point = _path.GetPosition(i / (float) steps);
            Handles.DrawLine(point, point + _path.GetDirection(i / (float) steps) * DIRECTION_SCALE);
        }
    }

    /// <summary>
    /// Draws the Properties of the selected point
    /// </summary>
    protected virtual void DrawSelectedPointInspector()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Selected Point", EditorStyles.whiteLabel);
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", _path.GetControlPoint(SelectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_path, "Move Point");
            EditorUtility.SetDirty(_path);
            _path.SetControlPoint(SelectedIndex, point);
            if (AutoPresampleInEditMode)
                _path.Presample();
        }
    }
}