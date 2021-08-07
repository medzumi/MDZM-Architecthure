using System.Collections.Generic;
using Presenting;

namespace Entitas.Presenting
{
    //Oh shit : 4 arg generic O_O
    public class PresentingComponentSystem<TComponent, TIdComponent, TKey, TEntity> : ReactiveSystem<TEntity>
        where TComponent : IComponent
        where TIdComponent : IComponent, IIdentifierComponent<TKey>
        where TEntity : Entity
    {
        private static readonly int _componentId = GenericComponentsLookup<TEntity>.GetComponentId<TComponent>();

        private static readonly int _identifierComponentId =
            GenericComponentsLookup<TEntity>.GetComponentId<TIdComponent>();

        private readonly AbstractPresenter<TComponent, TKey> _presenter;
        
        public PresentingComponentSystem(IContext<TEntity> context, AbstractPresenter<TComponent, TKey> presenter) : base(context)
        {
            _presenter = presenter;
        }

        public PresentingComponentSystem(ICollector<TEntity> collector, AbstractPresenter<TComponent, TKey> presenter) : base(collector)
        {
            _presenter = presenter;
        }

        protected override ICollector<TEntity> GetTrigger(IContext<TEntity> context)
        {
            return context.CreateCollector(
                Matcher<TEntity>.AnyOf(_componentId, _identifierComponentId).AddedOrRemoved());
        }

        protected override bool Filter(TEntity entity)
        {
            return entity.HasComponent(_identifierComponentId);
        }

        protected override void Execute(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.HasComponent(_componentId))
                {
                    _presenter.Present(entity.GetComponent<TComponent>(_componentId), entity.GetComponent<TIdComponent>(_identifierComponentId).Key);
                }
                else
                {
                    _presenter.StopPresent(entity.GetComponent<TIdComponent>(_identifierComponentId).Key);
                }
            }
        }
    }
}