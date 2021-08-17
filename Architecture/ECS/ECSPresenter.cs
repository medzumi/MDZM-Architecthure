using System.Collections.Generic;
using Architecture.Presenting;
using MedzuzaECS.ECS;

namespace Architecture.ECS
{
    public class ECSPresenter<TView, T> : ContextPresenter<TView, Entity, int>
        where T : ECSPresenter<TView, T>.PresenterInstruction, new()
    {
        public abstract class PresenterInstruction
        {
            public abstract void Present(Entity entity, TView view);

            public abstract void StopPresent(Entity entity, TView view);
        }

        private Dictionary<int, T> _presenterInstructions =
            new Dictionary<int, T>();
        
        
        
        public ECSPresenter(IPresentContext<TView, int> context) : base(context)
        {
        }

        protected override void PresentHandler(Entity model, TView view, int key)
        {
            var presenter = Pool<T>.Get();
            presenter.Present(model, view);
            _presenterInstructions.Add(key, presenter);
        }

        protected override void StopPresentHandler(Entity model, TView view, int key)
        {
            var presenter = _presenterInstructions[key];
            presenter.StopPresent(model, view);
            _presenterInstructions.Remove(key);
            Pool<T>.Retain(presenter);
        }
    }
}