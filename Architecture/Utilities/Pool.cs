using System;
using System.Collections.Generic;

namespace Architecture.Utilities
{
    public interface IFabric<out T>
    {
        T Get();
    }

    public interface IReleaser<in T>
    {
        void Releaser(T obj);
    }

    public class Fabric<T> : IFabric<T>, IReleaser<T> where T : IPoolElement<T>
    {
        private readonly Stack<T> _poolStack = new Stack<T>();

        private readonly Func<T> _createFunc;

        public Fabric()
        {
            _createFunc = Activator.CreateInstance<T>;
        }
    
        public Fabric(Func<T> createFunc)
        {
            _createFunc = createFunc;
        }

        public T Get()
        {
            var elem = _poolStack.Count > 0 ? _poolStack.Pop() : _createFunc();
            elem.Releaser = this;
            return elem;
        }

        public void Releaser(T obj)
        {
            _poolStack.Push(obj);
        }
    }

    public interface IPoolElement<T> where T : IPoolElement<T>
    {
        IReleaser<T> Releaser { get; set; }
    }
}