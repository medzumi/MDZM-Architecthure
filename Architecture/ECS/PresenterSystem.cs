using Architecture.Presenting;
using ECS;
using ECS.Collectors;

namespace Architecture.ECS
{
    public abstract class PresenterSystem : UpdateSystem
    {
        protected PresenterSystem(Context context) : base(context)
        {
        }
    }
    
    public class PresenterSystem<T> : PresenterSystem
        where T : ICondition, new()
    {
        private readonly IPresenter<Entity> _presenter;
        
        public PresenterSystem(Context context, IPresenter<Entity> presenter) : base(context)
        {
            _presenter = presenter;
        }

        protected override ICollector CreateCollector(Context context)
        {
            foreach (var VARIABLE in context.GetCollector<Collector<T>>().collectedEntities)
            {
                _presenter.Present(VARIABLE, VARIABLE.Index);
            }
            return context.GetCollector<ReactiveCollector<T>>();
        }

        protected override void Execute(Entity entity)
        {
            _presenter.Present(entity, entity.Index);
        }
    }
    
    public class PresenterSystem<T1, T2> : PresenterSystem
        where T1 : ICondition, new()
        where T2 : ICondition, new()
    {
        private readonly IPresenter<Entity> _presenter;
        
        public PresenterSystem(Context context, IPresenter<Entity> presenter) : base(context)
        {
            _presenter = presenter;
        }

        protected override ICollector CreateCollector(Context context)
        {
            foreach (var VARIABLE in context.GetCollector<CollectorAll<T1, T2>>().collectedEntities)
            {
                _presenter.Present(VARIABLE, VARIABLE.Index);
            }
            return context.GetCollector<ReactiveCollector<T1, T2>>();
        }

        protected override void Execute(Entity entity)
        {
            _presenter.Present(entity, entity.Index);
        }
    }
    
    public class PresenterSystem<T1, T2, T3> : PresenterSystem
        where T1 : ICondition, new()
        where T2 : ICondition, new()
        where T3 : ICondition, new()
    {
        private readonly IPresenter<Entity> _presenter;
        
        public PresenterSystem(Context context, IPresenter<Entity> presenter) : base(context)
        {
            _presenter = presenter;
        }

        protected override ICollector CreateCollector(Context context)
        {
            foreach (var VARIABLE in context.GetCollector<CollectorAll<T1, T2, T3>>().collectedEntities)
            {
                _presenter.Present(VARIABLE, VARIABLE.Index);
            }
            return context.GetCollector<ReactiveCollector<T1, T2, T3>>();
        }

        protected override void Execute(Entity entity)
        {
            _presenter.Present(entity, entity.Index);
        }
    }
}