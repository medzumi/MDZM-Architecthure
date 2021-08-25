using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Architecture.TypeProperty
{
    [Serializable]
    public class TypeReference
    {
        [SerializeField] private string name;
        [SerializeField] private TypeReference[] _parametersName = new TypeReference[0];

        public static implicit operator Type(TypeReference typeReference)
        {
            var type = Type.GetType(typeReference.name);
            if (type.IsGenericTypeDefinition)
            {
                type = type.MakeGenericType(typeReference._parametersName.Select(t => (Type)t).ToArray());
            }
            return type;
        }
    }
}