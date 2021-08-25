using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Architecture.Presenting
{
    [CreateAssetMenu]
    public class PresentContextScriptableObject : ScriptableObject, IPresentContext<ViewModel.ViewModel>
    {
        [SerializeField] private ViewModel.ViewModel _prefab;

        private Dictionary<int, KeyValuePair<int, ViewModel.ViewModel>> _dictionary = new Dictionary<int, KeyValuePair<int, ViewModel.ViewModel>>();
        
        public ViewModel.ViewModel Get(int key)
        {
            if (!_dictionary.TryGetValue(key, out var pref))
            {
                pref = new KeyValuePair<int, ViewModel.ViewModel>(0, Object.Instantiate(_prefab));
                _dictionary.Add(key, pref);
            }
            else
            {
                if (!pref.Value)
                {
                    pref = new KeyValuePair<int, ViewModel.ViewModel>(pref.Key, Object.Instantiate(_prefab));
                }
            }

            pref = new KeyValuePair<int, ViewModel.ViewModel>(pref.Key + 1, pref.Value);
            _dictionary[key] = pref;
            return pref.Value;
        }
        
        public void ReserveView(ViewModel.ViewModel view, int key)
        {
            _dictionary.Add(key, new KeyValuePair<int, ViewModel.ViewModel>(1, view));
        }

        public void Destroy(int key)
        {
            var pref = _dictionary[key];
            pref = new KeyValuePair<int, ViewModel.ViewModel>(pref.Key - 1, pref.Value);
            if (pref.Key <= 0)
            {
                Object.Destroy(pref.Value);
                _dictionary.Remove(key);
            }
        }
    }
}