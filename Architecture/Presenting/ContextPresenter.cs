namespace Architecture.Presenting
{
    public abstract class ContextPresenter<TView, TModel, TKey> : AbstractPresenter<TModel, TKey>
    {
        private readonly IPresentContext<TView, TKey> _context;
        
        protected ContextPresenter(AbstractPresenter<TKey>[] presenters, AbstractPresenter<TModel, TKey>[] presenters1, IPresentContext<TView, TKey> context) : base(presenters, presenters1)
        {
            _context = context;
        }
        
        protected sealed override void PresentHandler(TModel model, TKey key)
        {
            PresentHandler(model, _context.Get(key), key);
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