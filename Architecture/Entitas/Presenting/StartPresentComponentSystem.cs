using System;
using System.Collections.Generic;
using System.Reflection;
using Architecture.Presenting;
using Entitas;

namespace Architecture.Entitas.Presenting
{
    public class StartPresentComponentSystem<TListenerComponent ,TComponent, TIdComponent, TKey, TEntity> : ReactiveSystem<TEntity>
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

        public StartPresentComponentSystem(IContext<TEntity> context, AbstractPresenter<TEntity, TKey> presenter) : base(context)
        {
            _presenter = presenter;
        }

        public StartPresentComponentSystem(ICollector<TEntity> collector, AbstractPresenter<TEntity, TKey> presenter) : base(collector)
        {
            _presenter = presenter;
        }

        protected override ICollector<TEntity> GetTrigger(IContext<TEntity> context)
        {
            return context.CreateCollector(Matcher<TEntity>.AllOf(_componentId, _identifierComponentId).Added());
        }

        protected override bool Filter(TEntity entity)
        {
            return entity.HasComponent(_componentId) && !entity.HasComponent(_listenerComponentId);
        }

        protected override void Execute(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.AddComponent(_listenerComponentId, entity.CreateComponent(_listenerComponentId, typeof(TListenerComponent)));
                _presenter.Present(entity, entity.GetComponent<TIdComponent>(_identifierComponentId).Key);
            }
        }
    }
}