using System.Collections.Generic;

namespace Architecture.Presenting
{
    public abstract class AbstractPresenter<TKey>
    {
        private readonly AbstractPresenter<TKey>[] _presenters;
        private readonly Dictionary<TKey, object> _dictionary = new Dictionary<TKey, object>();

        protected AbstractPresenter(AbstractPresenter<TKey>[] presenters)
        {
            _presenters = presenters;
        }

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

        public void Activate()
        {
            InternalActivate();
            ActivateHandler();
        }

        public void Deactivate()
        {
            DeactivateHandler();
            InternalDeactivate();
        }

        protected abstract void ActivateHandler();

        protected abstract void DeactivateHandler();

        protected abstract void PresentHandler(object model, TKey key);

        protected abstract void StopPresentHandler(object model, TKey key);

        protected internal virtual void InternalActivate()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Activate();
            }
        }

        protected internal virtual void InternalDeactivate()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Deactivate();
            }
        }
    }
    
    public abstract class AbstractPresenter<T, TKey> : AbstractPresenter<TKey>
    {
        private readonly AbstractPresenter<T, TKey>[] _presenters;

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

        protected internal override void InternalActivate()
        {
            base.InternalActivate();
            foreach (var keyValuePair in _dictionary)
            {
                PresentHandler(keyValuePair.Value, keyValuePair.Key);
            }
        }

        protected internal override void InternalDeactivate()
        {
            base.InternalDeactivate();
            foreach (var keyValuePair in _dictionary)
            {
                StopPresentHandler(keyValuePair.Value, keyValuePair.Key);
            }
        }

        protected abstract void PresentHandler(T model, TKey key);

        protected abstract void StopPresentHandler(T model, TKey key);

        protected AbstractPresenter(AbstractPresenter<TKey>[] presenters, AbstractPresenter<T, TKey>[] presenters1) : base(presenters)
        {
            _presenters = presenters1;
        }
    }
}