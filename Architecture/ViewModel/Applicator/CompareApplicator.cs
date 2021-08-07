using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

//TODO : need refactor
namespace Architecture.ViewModel.Applicator
{
    public abstract class CompareApplicator<T> : MonoBehaviour where T : IComparable<T>
    {
        [SerializeField] private List<UnitApplicator> _applicators;
    
        private readonly DisposeBlock _disposeBlock = DisposeBlock.Spawn();

        private void Awake()
        {
            foreach (var VARIABLE in _applicators)
            {
                VARIABLE.Init();
                _disposeBlock.Add(VARIABLE.ApplicableValue.Subscribe(Check));
            
                _disposeBlock.Add(VARIABLE);
            }
        }

        private void Check(bool value)
        {
            foreach (var VARIABLE in _applicators)
            {
                if (!VARIABLE.ApplicableValue)
                {
                    NotifyBool(false);
                    return;
                }
            }
            NotifyBool(true);
        }

        private void OnDestroy()
        {
            _disposeBlock.Dispose();
        }

        [Serializable]
        public class UnitApplicator : IDisposable
        {
            [SerializeField] private BindProperty<T> _bindLeft;
            [SerializeField] private BindProperty<T> _bindRight;
        
            [NonSerialized]
            public T leftValue = default;
            [NonSerialized]
            public T rightValue = default;
        
            public readonly AsyncReactiveProperty<bool> ApplicableValue = new AsyncReactiveProperty<bool>(false);

            private bool isDisposed = false;
            private readonly DisposeBlock _disposeBlock = DisposeBlock.Spawn();

            public void Init()
            {
                _disposeBlock.Add(_bindLeft.GetBind().Subscribe(new LeftCompareObserver(this)));
                _disposeBlock.Add(_bindRight.GetBind().Subscribe(new RightCompareObserver(this)));
            }

            public void Dispose()
            {
                if (!isDisposed)
                {
                    _disposeBlock.Dispose();
                }
            }
        }
    
        private class LeftCompareObserver : IObserver<T>
        {
            private readonly UnitApplicator _applicator;

            public LeftCompareObserver(UnitApplicator applicator)
            {
                _applicator = applicator;
            }
        
            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public async void OnNext(T value)
            {
                await UniTask.SwitchToMainThread();
                _applicator.leftValue = value;
                _applicator.ApplicableValue.Value = _applicator.leftValue.CompareTo(_applicator.rightValue) >= 0;
            }
        }
    
        private class RightCompareObserver : IObserver<T>
        {
            private readonly UnitApplicator _applicator;

            public RightCompareObserver(UnitApplicator applicator)
            {
                _applicator = applicator;
            }
        
            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public async void OnNext(T value)
            {
                await UniTask.SwitchToMainThread();
                _applicator.rightValue = value;
                _applicator.ApplicableValue.Value = _applicator.leftValue.CompareTo(_applicator.rightValue) >= 0;
            }
        }

        protected abstract void NotifyBool(bool value);
    }
}
