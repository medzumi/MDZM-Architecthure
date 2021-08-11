using System.Collections.Generic;
using Architecture.Presenting;
using Entitas;

namespace Architecture.Entitas.Presenting
{
    public class StopPresentComponentSystem<TListenerComponent ,TComponent, TIdComponent, TKey, TEntity> : ReactiveSystem<TEntity>
        where TComponent : IComponent
        where TIdComponent : IComponent, IIdentifierComponent<TKey>
        where TListenerComponent : IComponent
        where TEntity : Entity
    {
        private static readonly int _componentId = GenericComponentsLookup<TEntity>.GetComponentId<TComponent>();

        private static readonly int _identifierComponentId =
            GenericComponentsLookup<TEntity>.GetComponentId<TIdComponent>();

        private static readonly int _listenerComponentId =
            GenericComponentsLookup<TEntity>.GetComponentId<TListenerComponent>();

        private readonly AbstractPresenter<TEntity, TKey> _presenter;

        public StopPresentComponentSystem(IContext<TEntity> context, AbstractPresenter<TEntity, TKey> presenter) : base(context)
        {
            _presenter = presenter;
        }

        public StopPresentComponentSystem(ICollector<TEntity> collector, AbstractPresenter<TEntity, TKey> presenter) : base(collector)
        {
            _presenter = presenter;
        }

        protected override ICollector<TEntity> GetTrigger(IContext<TEntity> context)
        {
            return context.CreateCollector(Matcher<TEntity>.AnyOf(_componentId, _identifierComponentId).Removed());
        }

        protected override bool Filter(TEntity entity)
        {
            return !entity.HasComponent(_listenerComponentId) && entity.HasComponent(_identifierComponentId);
        }

        protected override void Execute(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _presenter.StopPresent(entity.GetComponent<TIdComponent>(_identifierComponentId).Key);
            }
        }
    }
}