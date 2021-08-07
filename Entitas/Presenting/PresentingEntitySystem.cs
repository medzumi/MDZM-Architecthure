using System.Collections.Generic;
using Presenting;

namespace Entitas.Presenting
{
    public class PresentingEntitySystem<TIdComponent, TKey, TEntity> : ReactiveSystem<TEntity>
        where TIdComponent : IComponent, IIdentifierComponent<TKey>
        where TEntity : Entity
    {
        private static readonly int _identifierComponentId =
            GenericComponentsLookup<TEntity>.GetComponentId<TIdComponent>();

        private readonly AbstractPresenter<TIdComponent, TKey> _presenter;

        private readonly EntityComponentReplaced _onReplaceComponent;
        
        public PresentingEntitySystem(IContext<TEntity> context, AbstractPresenter<TIdComponent, TKey> presenter) : base(context)
        {
            _onReplaceComponent = ReplaceComponentHandler;
            _presenter = presenter;
        }

        public PresentingEntitySystem(ICollector<TEntity> collector, AbstractPresenter<TIdComponent, TKey> presenter) : base(collector)
        {
            _onReplaceComponent = ReplaceComponentHandler;
            _presenter = presenter;
        }

        protected override ICollector<TEntity> GetTrigger(IContext<TEntity> context)
        {
            return context.CreateCollector(Matcher<TEntity>.AllOf(_identifierComponentId).Added());
        }

        protected override bool Filter(TEntity entity)
        {
            return true;
        }

        protected override void Execute(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var component = entity.GetComponent<TIdComponent>(_identifierComponentId);
                entity.OnComponentReplaced += _onReplaceComponent;
                _presenter.Present(component, component.Key);
            }
        }

        private void ReplaceComponentHandler(IEntity entity, int index, IComponent previousComponent,
            IComponent newComponent)
        {
            if (index == _identifierComponentId)
            {
                //ToDo : add rename in presenter
                if(newComponent == null)
                    _presenter.StopPresent(((TIdComponent)previousComponent).Key);
            }
        }
    }
}