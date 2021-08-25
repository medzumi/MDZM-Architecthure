using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.ECS;
using Architecture.ECS.CreatingFeature;
using ECS;
using ModestTree;
using UnityEditor;
using UnityEngine;

namespace Architecture.TypeProperty.Editor
{
    [CustomPropertyDrawer(typeof(TypeReference))]
    [CustomPropertyDrawer(typeof(InheritsAttribute))]
    public class TypeReferenceDrawer : PropertyDrawer
    {
        private const float _customHeight = 20f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetRecursivePropertyHeight(property);
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

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as InheritsAttribute;
            DrawRecusive(position, property, label.text, new []{ attr != null ? attr.Type : typeof(object) });

            //     if (index >= 0)
            //     {
            //         nameProperty.stringValue = attr.TypeNames[index];
            //         type = attr.AvailableTypes[index];
            //         if (type.IsGenericTypeDefinition)
            //         {
            //             var size = type.GetGenericArguments().Length;
            //             if (size != listProperty.arraySize)
            //             {
            //                 for (int i = listProperty.arraySize; i < size; i++)
            //                     listProperty.InsertArrayElementAtIndex(i - 1);
            //                 for (int i = listProperty.arraySize; i > size; i--)
            //                 {
            //                     listProperty.DeleteArrayElementAtIndex(i - 1);
            //                 }
            //             }
            //
            //             DrawGenericParameters(position, listProperty, type);
            //         }
            //     }
            // }
        }

        private void DrawRecusive(Rect position, SerializedProperty property, string label, IEnumerable<Type> inheritTypes, bool availableAbstract = true)
        {
            var listProperty = property.FindPropertyRelative("_parametersName");
            var nameProperty = property.FindPropertyRelative("name");
            var availableTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(ass => ass.GetTypes())
                    .Where(t =>
                    {
                        var result = availableAbstract || !t.IsAbstract;
                        foreach (var VARIABLE in inheritTypes)
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

                                if(!(result &= subResult))
                                    break;
                            }
                            else
                            {
                                if (VARIABLE == typeof(PresenterSystem) && t == typeof(PresenterSystem<,,>))
                                {
                                    
                                }
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
                                    if(!(result &= subResult))
                                        break;
                                }
                                else
                                {
                                    if(!(result &= true))
                                        break;
                                }
                            }
                        }

                        return result;
                    })
                    .ToArray();

            
            var drawNames = availableTypes.Select(t =>
                    $"{(string.IsNullOrWhiteSpace(t.Namespace) ? "DEFAULT" : t.Namespace).Replace('.', '/')}/{t.Name}")
                .ToArray();
                
            var index = availableTypes.IndexOf(Type.GetType(nameProperty.stringValue));
            index = EditorGUI.Popup(
                new Rect(position.x, position.y, position.width, _customHeight), label, index,
                drawNames);

            if (index > -1)
            {
                nameProperty.stringValue = availableTypes[index].AssemblyQualifiedName;
                var arguments = availableTypes[index].GetGenericArguments();
                if (arguments.Length != listProperty.arraySize)
                {
                    for (int i = listProperty.arraySize; i < arguments.Length; i++)
                    {
                        listProperty.InsertArrayElementAtIndex(i);
                    }
                    for(int i = listProperty.arraySize; i > arguments.Length; i--)
                    {
                        listProperty.DeleteArrayElementAtIndex(i - 1);
                    }
                }
                else
                {
                    var offset = position.width * 0.1f;
                    var currentHeight = position.y + _customHeight;
                    for (int i = 0; i < listProperty.arraySize; i++)
                    {
                        var elementProperty = listProperty.GetArrayElementAtIndex(i);
                        var height = GetRecursivePropertyHeight(elementProperty);
                        var constrainst = arguments[i].GetGenericParameterConstraints().Select(t =>
                        {
                            if (t.IsGenericType)
                            {
                                return t.GetGenericTypeDefinition();
                            }
                            else
                            {
                                return t;
                            }
                        }).ToArray();
                        DrawRecusive(new Rect(position.x + offset, currentHeight, position.width - offset, height), elementProperty, arguments[i].Name, constrainst);
                        currentHeight += height;
                    }
                }
            }
            else
            {
                nameProperty.stringValue = string.Empty;
            }
        }
    }
}