using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Architecture.Presenting
{
    [Serializable]
    public class PresentContextScriptableObject<TView, TKey> : IPresentContext<TView, TKey>
        where TView : Object
    {
        [SerializeField] private TView _prefab;

        private Dictionary<TKey, TView> _dictionary = new Dictionary<TKey, TView>();
        
        public TView Get(TKey key)
        {
            if (!_dictionary.TryGetValue(key, out var pref))
            {
                pref = Object.Instantiate(_prefab);
                _dictionary.Add(key, pref);
            }
            else
            {
                if (!pref)
                {
                    pref = Object.Instantiate(_prefab);
                    _dictionary[key] = pref;
                }
            }

            return pref;
        }

        public void Destroy(TKey key)
        {
            var pref = _dictionary[key];
            Object.Destroy(pref);
            _dictionary.Remove(key);
        }
    }
}