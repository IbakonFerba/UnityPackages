using UnityEngine;
using UnityEditor;
using FK.Paths;

/// <summary>
/// <para>Custom Editor for a Bezier Spline</para>
///
/// v1.0 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CustomEditor(typeof(BezierSpline))]
public class BezierSplineEditor : PathEditorBase 
{
	// ######################## PRIVATE VARS ######################## //
	private BezierSpline _spline;
	
	/// <summary>
	/// Colors for the Control point modes
	/// </summary>
	private static readonly Color[] MODE_COLORS = {
		Color.white,
		Color.yellow,
		Color.cyan
	};

	// ######################## UNITY EVENT FUNCTIONS ######################## //
	protected override void OnSceneGUI()
	{
		base.OnSceneGUI();
		_spline = target as BezierSpline;

		// draw the spline
		Vector3 p0 = ShowPoint(0, MODE_COLORS[(int)_spline.GetControlPointMode(0)]);
		for (int i = 1; i < _spline.ControlPointCount; i += 3)
		{
			Vector3 p1 = ShowPoint(i, MODE_COLORS[(int)_spline.GetControlPointMode(i)]);
			Vector3 p2 = ShowPoint(i+1, MODE_COLORS[(int)_spline.GetControlPointMode(i+1)]);
			Vector3 p3 = ShowPoint(i+2, MODE_COLORS[(int)_spline.GetControlPointMode(i+2)]);
			
			Handles.color = Color.gray;
			Handles.DrawLine(p0, p1);
			Handles.DrawLine(p2, p3);
			
			Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
			p0 = p3;
		}

		ShowDirections();
	}


	// ######################## FUNCTIONALITY ######################## //
	/// <summary>
	/// Draws the Properties of the selected point
	/// </summary>
	protected override void DrawSelectedPointInspector()
	{
		base.DrawSelectedPointInspector();
		
		EditorGUI.BeginChangeCheck();
		Bezier.BezierControlPointMode mode = (Bezier.BezierControlPointMode)
			EditorGUILayout.EnumPopup("Mode", _spline.GetControlPointMode(SelectedIndex));
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(_spline, "Change Point Mode");
			_spline.SetControlPointMode(SelectedIndex, mode);
			EditorUtility.SetDirty(_spline);
			if(AutoPresampleInEditMode)
				_spline.Presample();
		}
	}
}