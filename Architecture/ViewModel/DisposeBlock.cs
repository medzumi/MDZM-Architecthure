using System;
using System.Collections.Generic;

namespace Architecture.ViewModel
{
    //todo : replace dispose block from zenject
    public class DisposeBlock
    {
        private readonly List<IDisposable> _disposable = new List<IDisposable>();

        public static DisposeBlock Spawn()
        {
            return new DisposeBlock();
        }
        
        public void Add(IDisposable disposable)
        {
            _disposable.Add(disposable);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposable)
            {
                disposable.Dispose();
            }
        }
    }
}