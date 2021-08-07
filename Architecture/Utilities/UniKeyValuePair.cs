using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Architecture.Utilities
{
    [Serializable]
    public struct UniKeyValuePair<TKey, TValue>
    {
        private static StringBuilder _stringBuilder = new StringBuilder();
        private static bool _semaphoer = true;
    
        [SerializeField] private TKey _key;
        [SerializeField] private TValue _value;

        public UniKeyValuePair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }
    
        public TKey Key => _key;
        public TValue Value => _value;

        public override string ToString()
        {
            if (Volatile.Read(ref _semaphoer))
            {
                Volatile.Write(ref _semaphoer, false);
                _stringBuilder.Clear();
                _stringBuilder.Append('[');
                if (_key != null)
                {
                    _stringBuilder.Append(_key.ToString());
                }

                _stringBuilder.Append(", ");
                if (_value != null)
                {
                    _stringBuilder.Append(Value.ToString());
                }

                _stringBuilder.Append(']');
            }
            var result = _stringBuilder.ToString();
            Volatile.Write(ref _semaphoer, true);
            return result;
        }

        public static implicit operator KeyValuePair<TKey, TValue>(UniKeyValuePair<TKey, TValue> keyValuePair)
        {
            return new KeyValuePair<TKey, TValue>(keyValuePair._key, keyValuePair._value);
        }

        public static implicit operator UniKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> keyValuePair)
        {
            return new UniKeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value);
        }
    }
}