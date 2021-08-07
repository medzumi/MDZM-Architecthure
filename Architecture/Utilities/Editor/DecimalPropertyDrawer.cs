using System;
using UnityEditor;
using UnityEngine;

namespace Architecture.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(decimal))]
    [Obsolete("Doesnt work", true)]
    public class DecimalPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(SerializedPropertyType.Float, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.floatValue = EditorGUI.FloatField(position, label, property.floatValue);
        }
    }
}