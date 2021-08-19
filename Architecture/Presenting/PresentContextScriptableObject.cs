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

        private Dictionary<int, ViewModel.ViewModel> _dictionary = new Dictionary<int, ViewModel.ViewModel>();
        
        public ViewModel.ViewModel Get(int key)
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
        
        public void ReserveView(ViewModel.ViewModel view, int key)
        {
            _dictionary.Add(key, view);
        }

        public void Destroy(int key)
        {
            var pref = _dictionary[key];
            Object.Destroy(pref);
            _dictionary.Remove(key);
        }
    }
}