using System;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace Architecture.Utilities
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    [Serializable]
    public class FolderPathAttribute : PropertyAttribute
    {
#if UNITY_EDITOR
        public int contolId = 0;
        public AssetObject assetObject;
        public SerializedObject serializedObject;
        public SerializedProperty serializedProperty;

        public class AssetObject : ScriptableObject
        {
            public DefaultAsset defaultAsset;
        }
#endif
    }
}
