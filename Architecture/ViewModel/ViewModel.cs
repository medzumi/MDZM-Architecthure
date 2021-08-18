using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Architecture.ViewModel
{
    public interface IViewModel
    {
        void GetIntKeys(List<string> list);
        void GetFloatKeys(List<string> list);
        void GetStringKeys(List<string> list);
        void GetBoolKeys(List<string> list);
        void GetCommandKeys(List<string> list);

        IAsyncReactiveProperty<int> GetIntProperty(string key);
        IAsyncReactiveProperty<float> GetFloatProperty(string key);
        IAsyncReactiveProperty<string> GetStringProperty(string key);
        IAsyncReactiveProperty<bool> GetBoolProperty(string key);

        void AddCommand(string key, ICommand command);

        ICommand GetCommand(string key);

        Collection GetListViewModel(string key);

        void Retain();

        CancellationToken CancellationToken
        {
            get;
        }
    }

    public class ViewModel : MonoBehaviour, ISerializationCallbackReceiver, IViewModel
    {
        [Serializable]
        private struct IntPropertyConfiguration
        {
            public string Key;
            public int DefaultValue;
        }

        [Serializable]
        private struct FloatPropertyConfiguration
        {
            public string Key;
            public int DefaultValue;
        }
    
        [Serializable]
        private struct StringPropertyConfiguration
        {
            public string Key;
            public string DefaultValue;
        }
    
        [Serializable]
        private struct BoolPropertyConfiguraytion
        {
            public string Key;
            public bool DefaultValue;
        }
    
        [Serializable]
        private struct EventPropertyConfiguration
        {
            public string Key;
        }

        [Serializable]
        private struct ListViewModelConfiguration
        {
            public string Key;
            public Collection ListViewModel;
        }

        [SerializeField] private List<string> _commandKeys;

        private Dictionary<string, ProxyCommand> _commands;

        [SerializeField] private List<IntPropertyConfiguration> _intPropertyConfigurations;
        private Dictionary<string, IAsyncReactiveProperty<int>> _intReactiveProperties;

        [SerializeField] private List<FloatPropertyConfiguration> _floatPropertyConfigurations;
        private Dictionary<string, IAsyncReactiveProperty<float>> _floatReactiveProperties;

        private Dictionary<string, AsyncReactiveProperty<float>> _floatAsyncReactiveProperties =
            new Dictionary<string, AsyncReactiveProperty<float>>();

        [SerializeField] private List<StringPropertyConfiguration> _stringPropertyConfigurations;
        private Dictionary<string, IAsyncReactiveProperty<string>> _stringReactiveProperties;

        [SerializeField] private List<BoolPropertyConfiguraytion> _boolPropertyConfigurations;
        private Dictionary<string, IAsyncReactiveProperty<bool>> _boolReactiveProperties;

        [SerializeField] private List<ListViewModelConfiguration> _listViewModelConfigurations;
        private Dictionary<string, Collection> _listViewModels;

        private CancellationTokenSource _cancellationTokenSource;
        public CancellationToken CancellationToken => (_cancellationTokenSource??= new CancellationTokenSource()).Token;
    
        public virtual IAsyncReactiveProperty<float> GetFloatProperty(string key)
        {
            return _floatReactiveProperties[key];
        }

        public virtual void GetIntKeys(List<string> list)
        {
            foreach (var VARIABLE in _intPropertyConfigurations)
            {
                list.Add(VARIABLE.Key);
            }
        }

        public virtual void GetFloatKeys(List<string> list)
        {
            foreach (var VARIABLE in _floatPropertyConfigurations)
            {
                list.Add(VARIABLE.Key);
            }
        }

        public virtual void GetStringKeys(List<string> list)
        {
            foreach (var VARIABLE in _stringPropertyConfigurations)
            {
                list.Add(VARIABLE.Key);
            }
        }

        public virtual void GetBoolKeys(List<string> list)
        {
            foreach (var VARIABLE in _boolPropertyConfigurations)
            {
                list.Add(VARIABLE.Key);
            }
        }

        public void GetCommandKeys(List<string> list)
        {
            list.AddRange(_commandKeys);
        }

        public virtual IAsyncReactiveProperty<int> GetIntProperty(string key)
        {
            return _intReactiveProperties[key];
        }

        public virtual IAsyncReactiveProperty<string> GetStringProperty(string key)
        {
            return _stringReactiveProperties[key];
        }

        public virtual IAsyncReactiveProperty<bool> GetBoolProperty(string key)
        {
            return _boolReactiveProperties[key];
        }

        public void AddCommand(string key, ICommand command)
        {
            _commands[key].AddCommand(command);
        }

        public void RemoveCommand(string key, ICommand command)
        {
            _commands[key].RemoveCommand(command);
        }

        public ICommand GetCommand(string key)
        {
            return _commands[key];
        }

        public virtual Collection GetListViewModel(string key)
        {
            return _listViewModels[key];
        }

        public void Retain()
        {
            var cancellationTokenSource = _cancellationTokenSource;
            _cancellationTokenSource = null;
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            foreach (var VARIABLE in _commands)
            {
                VARIABLE.Value.Clear();
            }

            foreach (var VARIABLE in _listViewModels)
            {
                VARIABLE.Value.Retain();
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _intReactiveProperties = new Dictionary<string, IAsyncReactiveProperty<int>>(_intPropertyConfigurations.Count);
            foreach (var intPropertyConfiguration in _intPropertyConfigurations)
            {
                _intReactiveProperties.Add(intPropertyConfiguration.Key, new AsyncReactiveProperty<int>(intPropertyConfiguration.DefaultValue));
            }

            _floatReactiveProperties = new Dictionary<string, IAsyncReactiveProperty<float>>(_floatPropertyConfigurations.Count);
            foreach (var floatPropertyConfiguration in _floatPropertyConfigurations)
            {
                _floatReactiveProperties.Add(floatPropertyConfiguration.Key, new AsyncReactiveProperty<float>(floatPropertyConfiguration.DefaultValue));
            }

            _stringReactiveProperties = new Dictionary<string, IAsyncReactiveProperty<string>>(_stringPropertyConfigurations.Count);
            foreach (var stringPropertyConfiguration in _stringPropertyConfigurations)
            {
                _stringReactiveProperties.Add(stringPropertyConfiguration.Key, new AsyncReactiveProperty<string>(stringPropertyConfiguration.DefaultValue));
            }

            _listViewModels = new Dictionary<string, Collection>(_listViewModelConfigurations.Count);
            foreach (var listViewModelConfiguration in _listViewModelConfigurations)
            {
                _listViewModels.Add(listViewModelConfiguration.Key, listViewModelConfiguration.ListViewModel);
            }

            _boolReactiveProperties = new Dictionary<string, IAsyncReactiveProperty<bool>>();
            foreach (var boolPropertyConfiguration in _boolPropertyConfigurations)
            {
                _boolReactiveProperties.Add(boolPropertyConfiguration.Key, new AsyncReactiveProperty<bool>(boolPropertyConfiguration.DefaultValue));
            }

            _commands = new Dictionary<string, ProxyCommand>();
            foreach (var VARIABLE in _commandKeys)
            {
                _commands.Add(VARIABLE, new ProxyCommand());
            }
        }
    }
}