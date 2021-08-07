using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Architecture.ViewModel.View.Single
{
    public abstract class BoolReactiveView : MonoBehaviour, IObserver<bool>
    {
        [SerializeField] private BindProperty<bool> _bind;

        private void Awake()
        {
            _bind.GetBind().Subscribe(this, this.GetCancellationTokenOnDestroy());
        }

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(bool value);
    }
}
