using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Architecture.ViewModel
{
    [Serializable]
    public class BindProperty<T> : ISerializationCallbackReceiver
    {
        private static readonly Dictionary<Type, Func<IViewModel, string, object>> _typeResolveDictionary =
            new Dictionary<Type, Func<IViewModel, string, object>>()
            {
                {typeof(int), ( vm, key) => vm?.GetIntProperty(key)},
                {typeof(string), ( vm, key) => vm?.GetStringProperty(key)},
                {typeof(float), ( vm, key) => vm?.GetFloatProperty(key)},
                {typeof(bool), ( vm, key) => vm?.GetBoolProperty(key)}
            };
    
        [SerializeField] private ViewModel _gameObject;
        [SerializeField] private string _key;
        public string Key => _key;

        private Func<IViewModel, string, object> _func;
        private IAsyncReactiveProperty<T> _reactive;

    
        public IAsyncReactiveProperty<T> GetBind()
        {
            return _reactive ??= _func(_gameObject?.GetComponent<IViewModel>(), _key) as IAsyncReactiveProperty<T>;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _func = _typeResolveDictionary[typeof(T)];
        }
    }
}
