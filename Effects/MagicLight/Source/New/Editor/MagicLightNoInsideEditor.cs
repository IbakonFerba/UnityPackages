using UnityEditor;

/// <summary>
/// <para>Material Editor for Magic Light Shaders with no Inside Color</para>
///
/// v1.0 09/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class MagicLightNoInsideEditor : MagicLightStandardEditor 
{
	// ######################## UNITY EVENT FUNCTIONS ######################## //	
	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		ShowColAnMainTex(materialEditor);
		ShowSecondaryColor(materialEditor);
		ShowSurfaceShaderProperties(materialEditor);
		ShowTilingAndEffectsHeadline(materialEditor);
		ShowMagicLightParams(materialEditor, true);
		ShowAdvancedOptions(materialEditor);
	}
}