using System;
using System.Collections.Generic;
using Architecture.ViewModel.Observers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Architecture.ViewModel
{
    public class PoolCollection : Collection
    {
        [SerializeField] private ViewModel _viewModel;

        private readonly List<ViewModel> _viewModels = new List<ViewModel>();

        protected override IObserver<T> CreateObserverForProperty<T>(IAsyncReactiveProperty<T> property)
        {
            return new TransmitProperty<T>(property);
        }

        public override void Retain()
        {
            foreach (var VARIABLE in _viewModels)
            {
                VARIABLE.Retain();
            }
        }

        public override void Fill<T>(IEnumerable<T> list, Action<T, IViewModel> action)
        {
            int i = 0;
            foreach (var VARIABLE in list)
            {
                if (_viewModels.Count <= i)
                {
                    _viewModels.Add(Instantiate(_viewModel, transform));
                }
                _viewModels[i].gameObject.SetActive(true);
                InitViewModel(_viewModels[i]);
                action(VARIABLE, _viewModels[i]);
                i++;
            }

            for (; i < _viewModels.Count; i++)
            {
                _viewModels[i].gameObject.SetActive(false);
            }
        }
    }
}
