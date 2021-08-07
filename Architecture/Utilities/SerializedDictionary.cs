using System;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.Utilities
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<UniKeyValuePair<TKey, TValue>> _keyValuePairs;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            foreach (var keyValuePair in _keyValuePairs)
            {
                if (ContainsKey(keyValuePair.Key))
                {
                    this[keyValuePair.Key] = keyValuePair.Value;
                }
                else
                {
                    Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }
    }
}
