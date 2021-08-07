using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Architecture.ViewModel.View.Single
{
    public abstract class StringReactiveView : MonoBehaviour, IObserver<string>
    {
        [SerializeField] private BindProperty<string> _stringReactive;

        private void Awake()
        {
            _stringReactive.GetBind().Subscribe(this, this.GetCancellationTokenOnDestroy());
        }

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(string value);
    }
}
