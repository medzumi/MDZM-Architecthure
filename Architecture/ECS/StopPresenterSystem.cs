using Architecture.Presenting;
using ECS;
using ECS.Collectors;

namespace Architecture.ECS
{
    public class StopPresenterSystem<T> : UpdateSystem
        where T : ICondition, new()
    {
        private readonly IPresenter<Entity> _presenter;
        
        public StopPresenterSystem(Context context, IPresenter<Entity> presenter) : base(context)
        {
            _presenter = presenter;
        }

        protected override ICollector CreateCollector(Context context)
        {
            return context.GetCollector<ReactiveCollector<T>>();
        }

        protected override void Execute(Entity entity)
        {
            _presenter.StopPresent(entity, entity.Index);
        }
    }
    
    public class StopPresenterSystem<T1, T2> : UpdateSystem
        where T1 : ICondition, new()
        where T2 : ICondition, new()
    {
        private readonly IPresenter<Entity> _presenter;
        
        public StopPresenterSystem(Context context, IPresenter<Entity> presenter) : base(context)
        {
            _presenter = presenter;
        }

        protected override ICollector CreateCollector(Context context)
        {
            return context.GetCollector<ReactiveCollector<T1, T2>>();
        }

        protected override void Execute(Entity entity)
        {
            _presenter.StopPresent(entity, entity.Index);
        }
    }
    
    public class StopPresenterSystem<T1, T2, T3> : UpdateSystem
        where T1 : ICondition, new()
        where T2 : ICondition, new()
        where T3 : ICondition, new()
    {
        private readonly IPresenter<Entity> _presenter;
        
        public StopPresenterSystem(Context context, IPresenter<Entity> presenter) : base(context)
        {
            _presenter = presenter;
        }

        protected override ICollector CreateCollector(Context context)
        {
            return context.GetCollector<ReactiveCollector<T1, T2, T3>>();
        }

        protected override void Execute(Entity entity)
        {
            _presenter.StopPresent(entity, entity.Index);
        }
    }
}