using UnityEditor;
using UnityEngine;
using FK.Utility;

#if UNITY_EDITOR
/// <summary>
/// <para>Custom Property Drawer for Enum Flags</para>
///
/// v1.0 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagPropertyDrawer : PropertyDrawer 
{
	// ######################## UNITY EVENT FUNCTIONS ######################## //
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (property.propertyType == SerializedPropertyType.Enum)
		{
			property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
		}
		else
		{
			EditorGUI.LabelField(position, label.text, "Use EnumFlags with enum");
		}
	}
}
#endif