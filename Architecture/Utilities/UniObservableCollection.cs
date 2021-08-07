using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using UnityEngine;

namespace Architecture.Utilities
{
    [Serializable]
    public class UniObservableCollection<T> : ObservableCollection<T>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<T> _list;

        private const string FieldName = "items";
    
        public void OnBeforeSerialize()
        {
        
        }

        public void OnAfterDeserialize()
        {
            var field = (typeof(Collection<T>)).GetField(FieldName, BindingFlags.SetField | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(this, _list);
        }
    }
}
