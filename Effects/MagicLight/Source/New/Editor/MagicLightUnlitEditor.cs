using UnityEditor;

/// <summary>
/// <para>Material Editor for Unlit Magic Light Shader</para>
///
/// v1.0 09/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class MagicLightUnlitEditor : MagicLightStandardEditor 
{
	// ######################## UNITY EVENT FUNCTIONS ######################## //	
	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		ShowColAnMainTex(materialEditor);
		ShowTilingAndEffectsHeadline(materialEditor);
		ShowMagicLightParams(materialEditor, false);
		ShowAdvancedOptions(materialEditor);
	}
}