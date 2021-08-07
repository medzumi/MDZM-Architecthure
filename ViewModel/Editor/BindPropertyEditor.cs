using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Architecture.ViewModel.Editor
{
    public abstract class BindPropertyEditor : PropertyDrawer
    {
        private readonly List<string> _list = new List<string>();
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUI.GetPropertyHeight(SerializedPropertyType.ObjectReference, label);
            height *= 2f;
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var goproperty = property.FindPropertyRelative("_gameObject");
            var keyProperty = property.FindPropertyRelative("_key");
            goproperty.objectReferenceValue = EditorGUI.ObjectField(
                new Rect(position.x, position.y, position.width, position.height / 2f), goproperty.objectReferenceValue,
                typeof(ViewModel));
            if (!goproperty.objectReferenceValue)
            {
                keyProperty.stringValue = string.Empty;
                EditorGUI.Popup(
                    new Rect(position.x, position.y + position.height / 2f, position.width, position.height / 2f), "BindId",
                    0, new string[0]);
            }
            else
            {
                string[] keys;
                var index = 0;
                keys = GetKeys(GetPropertyName(goproperty.objectReferenceValue as ViewModel, _list), keyProperty.stringValue, out index);
                keyProperty.stringValue = keys[EditorGUI.Popup(
                    new Rect(position.x, position.y + position.height / 2f, position.width, position.height / 2f), "BindId", index,
                    keys)];
            }
        }

        protected string[] GetKeys(List<string> keyList, string current, out int index)
        {
            string[] keys;
            if (keyList.Count > 0)
            {
                keys = new string[keyList.Count];
                index = 0;
                for(int i = 0; i < keyList.Count; i++)
                {
                    keys[i] = keyList[i];
                    if (keys[i].Equals(current))
                        index = i;
                }
            }

            else
            {
                keys = new[] {string.Empty};
                index = 0;
            }

            return keys;
        }

        protected abstract List<string> GetPropertyName(ViewModel viewModel, List<string> list);
    }

    [CustomPropertyDrawer(typeof(BindProperty<string>))]
    public class StringBindPropertyEditor : BindPropertyEditor
    {
        protected override List<string> GetPropertyName(ViewModel viewModel, List<string> list)
        {
            list.Clear();
            viewModel.GetStringKeys(list);
            return list;
        }
    }

    [CustomPropertyDrawer(typeof(BindProperty<float>))]
    public class FloatBindPropertyEditor : BindPropertyEditor
    {
        protected override List<string> GetPropertyName(ViewModel viewModel, List<string> list)
        {
            list.Clear();
            viewModel.GetFloatKeys(list);
            return list;
        }
    }

    [CustomPropertyDrawer(typeof(BindProperty<bool>))]
    public class BoolBindPropertyEditor : BindPropertyEditor
    {
        protected override List<string> GetPropertyName(ViewModel viewModel, List<string> list)
        {
            list.Clear();
            viewModel.GetBoolKeys(list);
            return list;
        }
    }

    [CustomPropertyDrawer(typeof(BindProperty<int>))]
    public class IntBindPropertyEditor : BindPropertyEditor
    {
        protected override List<string> GetPropertyName(ViewModel viewModel, List<string> list)
        {
            list.Clear();
            viewModel.GetIntKeys(list);
            return list;
        }
    }
}