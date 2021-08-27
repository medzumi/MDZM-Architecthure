using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.ECS;
using Architecture.ECS.CreatingFeature;
using ECS;
using ECS.Collectors;
using ModestTree;
using UnityEditor;
using UnityEngine;

namespace Architecture.TypeProperty.Editor
{
    [CustomPropertyDrawer(typeof(TypeReference))]
    [CustomPropertyDrawer(typeof(InheritsAttribute))]
    public class TypeReferenceDrawer : PropertyDrawer
    {
        private List<Type[]> _types = new List<Type[]>();
        private List<List<List<Type>>> _currentConstraints = new List<List<List<Type>>>();

        private static List<List<Type>> _constaints
        {
            get => _test;
            set
            {
                _test = value;
            }
        }
        private static List<List<Type>> _test; 
        private static int _counter = 0;
        private static List<List<Type>> _startConstraint = new List<List<Type>>() {new List<Type>(){null}};
        private List<string[]> _names = new List<string[]>();
        
        private const float _customHeight = 20f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var parameters = property.FindPropertyRelative("_parametersName");
            var height = 0f;
            for (int i = 0; i < parameters.arraySize; i++)
            {
                height += EditorGUI.GetPropertyHeight(parameters.GetArrayElementAtIndex(i));
            }
            return _customHeight + height;
        }

        private float GetRecursivePropertyHeight(SerializedProperty property)
        {
            var height = _customHeight;
            var relativeProperty = property.FindPropertyRelative("_parametersName");
            for (int i = 0; i < relativeProperty.arraySize; i++)
            {
                height += GetRecursivePropertyHeight(relativeProperty.GetArrayElementAtIndex(i));
            }

            return height;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as InheritsAttribute; 
            if (_constaints == null)
            {
                
                _startConstraint[0][0] = attr != null ? attr.Type : typeof(object);
                _constaints = _startConstraint;
            }
            DrawRecusive(position, property, label.text);
        }

        private void DrawRecusive(Rect position, SerializedProperty property, string label,
            bool availableAbstract = true)
        {
            var listProperty = property.FindPropertyRelative("_parametersName");
            var nameProperty = property.FindPropertyRelative("name");
            if (_types.Count != _constaints.Count)
            {
                for (int i = _types.Count; i < _constaints.Count; i++)
                {
                    _types.Add(null);
                }
                for (int i = _types.Count; i > _constaints.Count; i--)
                {
                    _types.RemoveAt(i - 1);
                }
                for (int i = _names.Count; i < _constaints.Count; i++)
                {
                    _names.Add(null);
                }
                for (int i = _names.Count; i > _constaints.Count; i--)
                {
                    _names.RemoveAt(i - 1);
                }
                for (int i = _currentConstraints.Count; i < _constaints.Count; i++)
                {
                    _currentConstraints.Add(null);
                }
                for (int i = _currentConstraints.Count; i > _constaints.Count; i--)
                {
                    _currentConstraints.RemoveAt(i - 1);
                }
            }
            if (_types.Count > 0 && _types[_counter] == null)
            {
                _types[_counter] = AppDomain.CurrentDomain.GetAssemblies().SelectMany(ass => ass.GetTypes())
                    .Where(t =>
                    {
                        var result = (availableAbstract || !t.IsAbstract);
                        foreach (var VARIABLE in _constaints[_counter])
                        {
                            if (VARIABLE.IsGenericTypeDefinition)
                            {
                                var subResult = false;
                                while (t != null && t != typeof(object))
                                {
                                    if (t.IsGenericType)
                                    {
                                        if (t.GetGenericTypeDefinition() == VARIABLE)
                                        {
                                            subResult = true;
                                            break;
                                        }
                                    }

                                    t = t.BaseType;
                                }

                                if (!(result &= subResult))
                                    break;
                            }
                            else
                            {
                                if (!VARIABLE.IsAssignableFrom(t))
                                {
                                    var subResult = false;
                                    while (t != null && t != typeof(object))
                                    {
                                        if (t.IsSubclassOf(VARIABLE))
                                        {
                                            subResult = true;
                                            break;
                                        }

                                        t = t.BaseType;
                                    }

                                    if (!(result &= subResult))
                                        break;
                                }
                                else
                                {
                                    if (!(result &= true))
                                        break;
                                }
                            }
                        }

                        return result;
                    })
                    .ToArray();

                _names[_counter] = _types[_counter].Select(t =>
                        $"{(string.IsNullOrWhiteSpace(t.Namespace) ? "DEFAULT" : t.Namespace).Replace('.', '/')}/{t.Name}")
                    .ToArray();
                var oldIndex = _types[_counter].IndexOf(Type.GetType(nameProperty.stringValue));
                _currentConstraints[_counter] = new List<List<Type>>();
                if (oldIndex > -1)
                {
                    foreach (var genericArgument in _types[_counter][oldIndex].GetGenericArguments())
                    {
                        var list = new List<Type>();
                        _currentConstraints[_counter].Add(list);
                        foreach (var constraint in genericArgument.GetGenericParameterConstraints())
                        {
                            if (constraint.IsGenericType)
                            {
                                list.Add(constraint.GetGenericTypeDefinition());
                            }
                            else
                            {
                                list.Add(constraint);
                            }
                        }
                    }
                }

                for (int i = listProperty.arraySize; i < _currentConstraints[_counter].Count; i++) 
                {
                    listProperty.InsertArrayElementAtIndex(i - 1);
                }

                for (int i = listProperty.arraySize; i > _currentConstraints[_counter].Count; i--)
                {
                    listProperty.DeleteArrayElementAtIndex(i - 1); 
                }
            }

            var index = _types[_counter].IndexOf(Type.GetType(nameProperty.stringValue));
            var newIndex = EditorGUI.Popup(
                new Rect(position.x, position.y, position.width, _customHeight), label, index, _names[_counter]);
            
            if (index != newIndex)
            {
                if (newIndex > -1)
                {
                    nameProperty.stringValue = _types[_counter][newIndex].AssemblyQualifiedName;
                    _currentConstraints[_counter].Clear();
                    foreach (var genericArgument in _types[_counter][newIndex].GetGenericArguments())
                    {
                        var list = new List<Type>();
                        _currentConstraints[_counter].Add(list);
                        foreach (var constraint in genericArgument.GetGenericParameterConstraints())
                        {
                            if (constraint.IsGenericType)
                            {
                                list.Add(constraint.GetGenericTypeDefinition());
                            }
                            else
                            {
                                list.Add(constraint);
                            }
                        }
                    }

                    for (int i = listProperty.arraySize; i < _currentConstraints[_counter].Count; i++)
                    {
                        listProperty.InsertArrayElementAtIndex(i);
                    }

                    for (int i = listProperty.arraySize; i > _currentConstraints[_counter].Count; i--)
                    {
                        listProperty.DeleteArrayElementAtIndex(i - 1);
                    }
                }
                else
                {
                    nameProperty.stringValue = string.Empty;
                    for (int i = listProperty.arraySize - 1; i >= 0; i--)
                    {
                        listProperty.DeleteArrayElementAtIndex(i);
                    }

                    _currentConstraints[_counter].Clear(); 
                }
            }

            var offset = position.width * 0.1f;
            var currentHeight = position.y + _customHeight;
            var previousCount = _counter;
            _counter = 0;
            for (int i = 0; i < listProperty.arraySize; i++)
            {
                var elementProperty = listProperty.GetArrayElementAtIndex(i);
                var height = GetPropertyHeight(elementProperty.Copy(), GUIContent.none);
                _constaints = _currentConstraints[previousCount];  
                EditorGUI.PropertyField(new Rect(position.x + offset, currentHeight, position.width - offset, height),
                    elementProperty, GUIContent.none, true);
                _counter++;
                _startConstraint[0][0] = typeof(object);
                _constaints = null;
                currentHeight += height;
            }

            _counter = previousCount;
        }
    }
}