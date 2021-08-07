using System;
using System.Collections.Generic;
using System.Reflection;
using Entitas;

namespace Architecture.Entitas
{
    public static class GenericComponentsLookup<T> where T : Entity
    {
        private const string ENTITY = "Entity";
        private const string COMPONENTS_LOOKUP = "ComponentsLookup";
        
        private static readonly Dictionary<Type, int> _idDictionary;
        
        public static int GetComponentId<T>() where T : IComponent
        {
            return GetComponentId(typeof(T));
        }

        public static int GetComponentId(Type type)
        {
            return _idDictionary[type];
        }
        
        static GenericComponentsLookup()
        {
            _idDictionary = new Dictionary<Type, int>();
            var className = (typeof(T)).ToString().Replace(ENTITY, COMPONENTS_LOOKUP);
            var type = Type.GetType(className);
            var flag = BindingFlags.Static | BindingFlags.GetField
                | BindingFlags.Public;
            var fieldInfo = type.GetField("componentTypes", flag);
            var componentTypes = (Type[])fieldInfo.GetValue(null);
            int index = 0;
            foreach (var VARIABLE in componentTypes)
            {
                _idDictionary.Add(VARIABLE, index);
                index++;
            }
        }
    }
}