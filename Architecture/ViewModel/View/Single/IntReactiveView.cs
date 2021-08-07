using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Architecture.ViewModel.View.Single
{
    public abstract class IntReactiveView : MonoBehaviour, IObserver<int>
    {
        [SerializeField] private BindProperty<int> _bind;

        protected void Awake()
        {
            _bind.GetBind().Subscribe(this, this.GetCancellationTokenOnDestroy());
        }

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int value);
    }
}
