using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Architecture.ViewModel
{
    public abstract class Collection : MonoBehaviour
    {
        [Header("Translate reactive to child view model")]
        [SerializeField] private List<BindProperty<string>> _stringInheritanceProperty;
        [SerializeField] private List<BindProperty<float>> _floatInheritanceProperty;
        [SerializeField] private List<BindProperty<int>> _intInheritanceProperty;
        [SerializeField] private List<BindProperty<bool>> _boolInheritanceProperty;
        [SerializeField] private List<BindCommand> _commands;

        //TODO : Make Two-Way Property
        [Header("Translate reactive to parent view model"), Space(20)]
        [SerializeField] private List<BindProperty<string>> _stringAscendingProperty;
        [SerializeField] private List<BindProperty<float>> _floatAscendingProperty;
        [SerializeField] private List<BindProperty<int>> _intAscendingProperty;
        [SerializeField] private List<BindProperty<bool>> _boolAscendingProperty;

        protected abstract IObserver<T> CreateObserverForProperty<T>(IAsyncReactiveProperty<T> property);

        public abstract void Retain();
        
        protected T InitViewModel<T>(T viewModel) where T: IViewModel
        {
            foreach (var VARIABLE in _commands)
            {
                viewModel.AddCommand(VARIABLE.Key, VARIABLE.GetCommand());
            }
            
            foreach (var VARIABLE in _stringInheritanceProperty)
            {
                VARIABLE.GetBind().Subscribe(CreateObserverForProperty(viewModel.GetStringProperty(VARIABLE.Key)), viewModel.CancellationToken);
            }

            foreach (var VARIABLE in _floatInheritanceProperty)
            {
                VARIABLE.GetBind().Subscribe(CreateObserverForProperty(viewModel.GetFloatProperty(VARIABLE.Key)), viewModel.CancellationToken);
            }

            foreach (var VARIABLE in _intInheritanceProperty)
            {
                VARIABLE.GetBind().Subscribe(CreateObserverForProperty(viewModel.GetIntProperty(VARIABLE.Key)), viewModel.CancellationToken);
            }

            foreach (var VARIABLE in _boolInheritanceProperty)
            {
                VARIABLE.GetBind().Subscribe(CreateObserverForProperty(viewModel.GetBoolProperty(VARIABLE.Key)), viewModel.CancellationToken);
            }

            foreach (var VARIABLE in _stringAscendingProperty)
            {
                viewModel.GetStringProperty(VARIABLE.Key).Subscribe(CreateObserverForProperty(VARIABLE.GetBind()), viewModel.CancellationToken);
            }
            foreach (var VARIABLE in _intAscendingProperty)
            {
                viewModel.GetIntProperty(VARIABLE.Key).Subscribe(CreateObserverForProperty(VARIABLE.GetBind()), viewModel.CancellationToken);
            }
            foreach (var VARIABLE in _floatAscendingProperty)
            {
                viewModel.GetFloatProperty(VARIABLE.Key).Subscribe(CreateObserverForProperty(VARIABLE.GetBind()), viewModel.CancellationToken);
            }
            foreach (var VARIABLE in _boolAscendingProperty)
            {
                viewModel.GetBoolProperty(VARIABLE.Key).Subscribe(CreateObserverForProperty(VARIABLE.GetBind()), viewModel.CancellationToken);
            }

            return viewModel;
        }
        
        
        public abstract void Fill<T>(IEnumerable<T> list, Action<T, IViewModel> action);
    }
}
