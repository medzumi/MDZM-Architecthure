using System;
using UnityEngine;
using UnityEngine.Events;

namespace Architecture.ViewModel.View.Single
{
    public class UnityEventBoolReactiveView : BoolReactiveView
    {
        [Serializable]
        private class BoolUnityEvent : UnityEvent<bool>
        {
            
        }

        [SerializeField] private BoolUnityEvent _boolUnityEvent;

        public override void OnCompleted()
        {
        }

        public override void OnError(Exception error)
        {
        }

        public override void OnNext(bool value)
        {
            _boolUnityEvent.Invoke(value);
        }
    }
}
