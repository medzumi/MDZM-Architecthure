using System.Collections.Generic;
using Architecture.Presenting;
using Entitas;

namespace Architecture.Entitas.Presenting
{
    //Oh shit : 4 arg generic O_O
    public class PresentingComponentSystem<TListenerComponent ,TComponent, TIdComponent, TKey, TEntity> : IReactiveSystem, IExecuteSystem
        where TComponent : IComponent
        where TIdComponent : IComponent, IIdentifierComponent<TKey>
        where TEntity : Entity
        where TListenerComponent : IComponent
    {
        private readonly StartPresentComponentSystem<TListenerComponent, TComponent, TIdComponent, TKey, TEntity>
            _startPresentComponentSystem;

        private readonly StopPresentComponentSystem<TListenerComponent, TComponent, TIdComponent, TKey, TEntity>
            _stopPresentComponentSystem;
        
        public PresentingComponentSystem(AbstractPresenter<TEntity, TKey> presenter, IContext<TEntity> context)
        {
            _startPresentComponentSystem =
                new StartPresentComponentSystem<TListenerComponent, TComponent, TIdComponent, TKey, TEntity>(context,
                    presenter);
            _stopPresentComponentSystem =
                new StopPresentComponentSystem<TListenerComponent, TComponent, TIdComponent, TKey, TEntity>(context,
                    presenter);
        }
        
        public void Execute()
        {
            _startPresentComponentSystem.Execute();
            _stopPresentComponentSystem.Execute();
        }

        public void Activate()
        {
            _startPresentComponentSystem.Activate();
            _stopPresentComponentSystem.Activate();
        }

        public void Deactivate()
        {
            _startPresentComponentSystem.Deactivate();
            _stopPresentComponentSystem.Deactivate();
        }

        public void Clear()
        {
            _startPresentComponentSystem.Clear();
            _stopPresentComponentSystem.Clear();
        }
    }
}