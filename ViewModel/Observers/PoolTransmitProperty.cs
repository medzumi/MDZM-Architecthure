using System;
using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine;
using Zenject;
using Zenject;

namespace Architecture.ViewModel.Observers
{
    public class PoolTransmitProperty<T> : ITransmitProperty<T>
    {
        public IAsyncReactiveProperty<T> Property { get; set; }

        public IMemoryPool memoryPool;
    
        public void OnCompleted()
        {
            memoryPool.Despawn(this);
        }

        public void OnError(Exception error)
        {
            memoryPool.Despawn(this);
        }

        public void OnNext(T value)
        {
            Property.Value = value;
        }
    }

    public class TransmitMemoryPool<T> : MemoryPool<PoolTransmitProperty<T>>
    {
        protected override void Reinitialize(PoolTransmitProperty<T> p1)
        {
            p1.memoryPool = this;
        }
    }
}