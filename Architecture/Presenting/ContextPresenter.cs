using System.Collections.Generic;

namespace Architecture.Presenting
{
    public abstract class ContextPresenter<TView, TModel> : IPresenter<TModel>
    {
        private readonly IPresentContext<TView> _context;

        public ContextPresenter(IPresentContext<TView> context)
        {
            _context = context;
        }
        
        protected void PresentHandler(TModel model, int key)
        {
            var view = _context.Get(key);
            PresentHandler(model, view, key);
        }

        public virtual void Activate()
        {
        }

        public virtual void Deactivate()
        {
        }

        protected void StopPresentHandler(TModel model, int key)
        {
            StopPresentHandler(model, _context.Get(key), key);
        }

        protected void DestroyView(int key)
        {
            _context.Destroy(key);
        }

        protected abstract void PresentHandler(TModel model, TView view, int key);

        protected abstract void StopPresentHandler(TModel model, TView view, int key);
        public void Present(TModel model, int key)
        {
            PresentHandler(model, key);
        }

        public void StopPresent(TModel model, int key)
        {
            StopPresentHandler(model, key);
        }
    }
}