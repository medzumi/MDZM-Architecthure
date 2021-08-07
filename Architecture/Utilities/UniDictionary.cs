using System;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.Utilities
{
    [Serializable]
    public class UniDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private UniKeyValuePair<TKey, TValue>[] _keyValuePairs;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var VARIABLE in _keyValuePairs)
            {
                Add(VARIABLE.Key, VARIABLE.Value);
            }
        }
    }
}