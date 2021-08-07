using System;
using UnityEngine;

namespace Architecture.ViewModel
{
    [Serializable]
    public struct BindCommand 
    {
        [SerializeField] private ViewModel _viewModel;
        [SerializeField] private string _key;

        public string Key => _key;
    
        public ICommand GetCommand()
        {
            return _viewModel.GetCommand(_key);
        }
    }
}
