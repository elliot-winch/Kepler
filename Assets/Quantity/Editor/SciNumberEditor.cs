﻿using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(SciNumber))]
public class SciNumberEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        int mantWidth = 100;
        int factorWidth = 35;
        int expWidth = 30;

        var mantissaRects = new Rect(position.x, position.y, mantWidth, position.height);
        var factorRect = new Rect(position.x + mantWidth, position.y, factorWidth, position.height);
        var exponentRect = new Rect(position.x + (mantWidth + factorWidth), position.y, expWidth, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(mantissaRects, property.FindPropertyRelative("mantissa"), GUIContent.none);
        EditorGUI.LabelField(factorRect, new GUIContent("x10^"));
        EditorGUI.PropertyField(exponentRect, property.FindPropertyRelative("exponent"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
