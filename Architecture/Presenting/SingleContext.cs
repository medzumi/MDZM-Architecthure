using System;

namespace Architecture.Presenting
{
    public class SingleContext<T> : IPresentContext<T>
    {
        private readonly T _example;

        private int? _key;
        
        public SingleContext(T example)
        {
            _example = example;
        }

        public T Get(int key)
        {
            if (_key == null)
            {
                _key = key;
                return _example;
            }
            else
            {
                throw  new Exception("There can be only one view for all context");
            }
        }

        public void Destroy(int key)
        {
            if (key == _key)
            {
                _key = null;
            }
        }
    }
}