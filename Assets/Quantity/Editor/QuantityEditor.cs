using System.Collections;
using UnityEditor;
using UnityEngine;

/*
[CustomPropertyDrawer(typeof(Quantity))]
public class QuantityEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        Quantity quantity = fieldInfo.GetValue(property.serializedObject.targetObject) as Quantity ?? new Quantity(SciNumber.Identity, new Units());
        Debug.Log(quantity.Value);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var valueRect = new Rect(position.x, position.y, 30, position.height);
        var unitsRect = new Rect(position.x + 35, position.y, 50, position.height);
        //var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("values"), GUIContent.none);
        EditorGUI.PropertyField(unitsRect, property.FindPropertyRelative("units"), GUIContent.none);
        //EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
*/