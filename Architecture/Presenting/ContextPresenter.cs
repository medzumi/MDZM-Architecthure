using System.Collections.Generic;

namespace Architecture.Presenting
{
    public abstract class ContextPresenter<TView, TModel, TKey> : AbstractPresenter<TModel, TKey>
    {
        private readonly IPresentContext<TView, TKey> _context;

        private readonly List<ContextPresenter<TView, TModel, TKey>> _presenters;

        public ContextPresenter(IEnumerable<ContextPresenter<TView, TModel, TKey>> presenters, IPresentContext<TView, TKey> context)
        {
            _context = context;
            _presenters = new List<ContextPresenter<TView, TModel, TKey>>(presenters);
        }
        
        protected sealed override void PresentHandler(TModel model, TKey key)
        {
            var view = _context.Get(key);
            PresentHandler(model, view, key);
            foreach (var presenter in _presenters)
            {
                presenter.Present(model, key);
            }

            foreach (var presenter in _presenters)
            {
                presenter.AttachToParent(view, key);
            }
        }

        public override void Activate()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Activate();
            }
        }

        public override void Deactivate()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Deactivate();
            }
        }

        protected override void StopPresentHandler(TModel model, TKey key)
        {
            foreach (var presenter in _presenters)
            {
                presenter.StopPresent(key);   
            }
            StopPresentHandler(model, _context.Get(key), key);
        }

        protected void DestroyView(TKey key)
        {
            _context.Destroy(key);
        }

        protected virtual void AttachToParent(TView parent, TKey key)
        {
        }

        protected abstract void PresentHandler(TModel model, TView view, TKey key);

        protected abstract void StopPresentHandler(TModel model, TView view, TKey key);
    }
}