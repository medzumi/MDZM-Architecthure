using System;
using System.Linq;
using ModestTree;
using UnityEditor;
using UnityEngine;

namespace Architecture.TypeProperty.Editor
{
    [CustomPropertyDrawer(typeof(TypeReference))]
    [CustomPropertyDrawer(typeof(InheritsAttribute))]
    public class TypeReferenceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(SerializedPropertyType.Enum, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as InheritsAttribute;
            property = property.FindPropertyRelative("name");
            if (attr.AvailableTypes == null)
            {
                attr.AvailableTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(ass => ass.GetTypes())
                    .Where(t => !t.IsAbstract && (t.IsSubclassOf(attr.Type) || attr.Type.IsAssignableFrom(t)))
                    .ToArray();
            }

            if (attr.DrawNames == null)
            {
                attr.DrawNames = attr.AvailableTypes.Select(t =>
                    $"{(string.IsNullOrWhiteSpace(t.Namespace) ? "DEFAULT" : t.Namespace).Replace('.', '/')}/{t.Name}").ToArray();
            }

            if (attr.TypeNames == null)
            {
                attr.TypeNames = attr.AvailableTypes.Select(t => t.AssemblyQualifiedName).ToArray();
            }

            var index = attr.TypeNames.IndexOf(property.stringValue);

            index = EditorGUI.Popup(position, label.text, index, attr.DrawNames);

            if (index >= 0)
            {
                property.stringValue = attr.TypeNames[index];
            }
        }
    }
}