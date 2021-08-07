using System;
using System.Data;
using Cysharp.Threading.Tasks;

namespace Architecture.ViewModel.Observers
{
    public class TransmitProperty<T> : ITransmitProperty<T>
    {
        private readonly IAsyncReactiveProperty<T> _property;

        public IAsyncReactiveProperty<T> Property
        {
            get => _property;
            set
            {
                throw new ReadOnlyException();
            }
        }
        public TransmitProperty(IAsyncReactiveProperty<T> property)
        {
            _property = property;
        }
            
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(T value)
        {
            _property.Value = value;
        }
    }
}
