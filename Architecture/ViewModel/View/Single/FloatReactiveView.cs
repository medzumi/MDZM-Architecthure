using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Architecture.ViewModel.View.Single
{
    public abstract class FloatReactiveView : MonoBehaviour, IObserver<float>
    {
        [SerializeField] private BindProperty<float> _bindProperty;

        private void Awake()
        {
            _bindProperty.GetBind().Subscribe(this, this.GetCancellationTokenOnDestroy());
        }

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(float value);
    }
}
