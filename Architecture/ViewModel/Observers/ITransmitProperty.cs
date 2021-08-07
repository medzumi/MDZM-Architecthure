using System;
using Cysharp.Threading.Tasks;

namespace Architecture.ViewModel.Observers
{
    public interface ITransmitProperty<T> : IObserver<T>
    {
        IAsyncReactiveProperty<T> Property { get; set; }
    }
}
