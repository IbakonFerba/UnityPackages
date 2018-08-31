using UnityEngine;
using UnityEditor;
using FK.Paths;

/// <summary>
/// <para>Custom Editor for a Linear Path</para>
///
/// v1.0 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CustomEditor(typeof(LinearPath))]
public class LinearPathEditor : PathEditorBase 
{
	// ######################## PRIVATE VARS ######################## //
	private LinearPath _linearPath;
	
	// ######################## UNITY EVENT FUNCTIONS ######################## //
	protected override void OnSceneGUI()
	{
		base.OnSceneGUI();
		
		// draw the path
		_linearPath = target as LinearPath;

		Vector3 p0 = ShowPoint(0, Color.white);
		for (int i = 1; i < _linearPath.ControlPointCount; ++i)
		{
			Vector3 p1 = ShowPoint(i, Color.white);
			
			Handles.color = Color.white;
			Handles.DrawLine(p0, p1);
			p0 = p1;
		}
	}
}