using System;
using UnityEngine;

namespace Architecture.TypeProperty
{
    [Serializable]
    public class TypeReference
    {
        [SerializeField] private string name;

        public static implicit operator Type(TypeReference typeReference)
        {
            return Type.GetType(typeReference.name);
        }
    }
}