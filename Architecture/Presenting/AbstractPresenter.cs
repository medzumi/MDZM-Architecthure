using System.Collections.Generic;
using System.Linq;

namespace Architecture.Presenting
{
    public abstract class AbstractPresenter<TKey>
    {
        private readonly Dictionary<TKey, object> _dictionary = new Dictionary<TKey, object>();

        public virtual void Present(object model, TKey key)
        {
            _dictionary.Add(key, model);
            PresentHandler(model, key);
        }

        public virtual void StopPresent(TKey key)
        {
            StopPresentHandler(_dictionary[key], key);
            _dictionary.Remove(key);
        }

        protected abstract void StopPresentHandler(object model, TKey key);

        protected abstract void PresentHandler(object mode, TKey key);

        public abstract void Activate();

        public abstract void Deactivate();
    }

    public interface IPresenter<T, TKey>
    {
        void Present(T model, TKey key);

        void StopPresent(T model, TKey key);
    }
    
    public abstract class AbstractPresenter<T, TKey> : AbstractPresenter<TKey>
    {
        private readonly Dictionary<TKey, T> _dictionary = new Dictionary<TKey, T>();

        public sealed override void Present(object model, TKey key)
        {
            Present((T)model, key);
            PresentHandler(model, key);
        }

        public sealed override void StopPresent(TKey key)
        {
            var model = _dictionary[key];
            StopPresentHandler((object)model, key);
            StopPresentHandler(model, key);
            _dictionary.Remove(key);
        }

        public void Present(T model, TKey key)
        {
            _dictionary.Add(key, model);
            PresentHandler(model, key);
        }

        protected sealed override void PresentHandler(object mode, TKey key)
        {
            
        }

        protected sealed override void StopPresentHandler(object model, TKey key)
        {
            
        }

        protected abstract void PresentHandler(T model, TKey key);

        protected abstract void StopPresentHandler(T model, TKey key);
    }
}