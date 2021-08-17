using System.Collections.Generic;

namespace Architecture.Presenting
{
    public abstract class ContextPresenter<TView, TModel, TKey> : AbstractPresenter<TModel, TKey>
    {
        private readonly IPresentContext<TView, TKey> _context;

        public ContextPresenter(IPresentContext<TView, TKey> context)
        {
            _context = context;
        }
        
        protected sealed override void PresentHandler(TModel model, TKey key)
        {
            var view = _context.Get(key);
            PresentHandler(model, view, key);
        }

        public override void Activate()
        {
        }

        public override void Deactivate()
        {
        }

        protected override void StopPresentHandler(TModel model, TKey key)
        {
            StopPresentHandler(model, _context.Get(key), key);
        }

        protected void DestroyView(TKey key)
        {
            _context.Destroy(key);
        }

        protected abstract void PresentHandler(TModel model, TView view, TKey key);

        protected abstract void StopPresentHandler(TModel model, TView view, TKey key);
    }
}