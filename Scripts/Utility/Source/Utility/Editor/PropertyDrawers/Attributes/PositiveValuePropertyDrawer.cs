using UnityEngine;
using UnityEditor;
using FK.Utility;

/// <summary>
/// <para>Class Info</para>
///
/// v1.0 mm/20yy
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CustomPropertyDrawer(typeof(PositiveValueAttribute))]
public class PositiveValuePropertyDrawer : PropertyDrawer
{
    // ######################## UNITY EVENT FUNCTIONS ######################## //
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property);

        if(property.propertyType == SerializedPropertyType.Integer)
        {
            if(property.intValue < 0)
            {
                property.intValue = 0;
            }
        }
        else if(property.propertyType == SerializedPropertyType.Float)
        {
            if (property.floatValue < 0.0f)
            {
                property.floatValue = 0.0f;
            }
        }
    }
}
