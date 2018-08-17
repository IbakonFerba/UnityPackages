using UnityEditor;

/// <summary>
/// <para>Material Editor for Transparent Clip Volume Shader</para>
///
/// v1.0 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class ClipVolumeTransparentEditor : ClipVolumeStandardEditor 
{
	// ######################## UNITY EVENT FUNCTIONS ######################## //	
	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		ShowGeneralProperties1(materialEditor);
		ShowSurfaceShaderProperties(materialEditor);
		ShowGeneralProperties2(materialEditor);  
		ShowGeneralProperties3(materialEditor);
	}
}