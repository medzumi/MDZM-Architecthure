using System;
using UnityEngine;

namespace Architecture.TypeProperty
{
    public class InheritsAttribute : PropertyAttribute
    {
        public Type Type;

        public string[] DrawNames;
        public string[] TypeNames;

        public Type[] AvailableTypes;

        public InheritsAttribute(Type t)
        {
            Type = t;
        }
    }
}