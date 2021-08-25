using ECS;
using ECS.Collectors;

namespace Architecture.ECS.CreatingFeature
{
    public class ActionResolveSystem<T, TRequester> : UpdateSystem
    {
        //ToDo : separate to create resolve, update resolve, cleanup resolve ?
        private readonly IActionResolveService<T> _resolveService;
        
        public ActionResolveSystem(Context context, IActionResolveService<T> resolveService) : base(context)
        {
            _resolveService = resolveService;
        }

        protected override ICollector CreateCollector(Context context)
        {
            return context.GetCollector<CollectorAll<Has<ActionRequest<T>>, Has<ActionRequester<TRequester>>, Has<ActionRequesterTarget<TRequester>>>>();
        }

        protected override void Execute(Entity entity)
        {
            _resolveService.Resolve(context, entity.GetComponent<ActionRequesterTarget<TRequester>>().Target, entity, entity.GetComponent<ActionRequest<T>>().RequestData);
        }
    }

    public class ActionResolveFeature<T, TRequester> : IUpdateSystem, ICleanupSystem
    {
        private readonly ActionResolveSystem<T, TRequester> _actionResolveSystem;
        private readonly ActionCleanupSystem<T, TRequester> _actionCleanupSystem;

        public ActionResolveFeature(Context context, IActionResolveService<T> resolveService)
        {
            _actionResolveSystem = new ActionResolveSystem<T, TRequester>(context, resolveService);
            _actionCleanupSystem = new ActionCleanupSystem<T, TRequester>(context);
        }
        
        public void Update()
        {
            _actionResolveSystem.Update();
        }

        public void Cleanup()
        {
            _actionCleanupSystem.Cleanup();
        }
    }

    public class ActionCleanupSystem<T, TRequester> : CleanupSystem
    {
        public ActionCleanupSystem(Context context) : base(context)
        {
        }

        protected override ICollector CreateCollector(Context context)
        {
            return context.GetCollector<CollectorAll<Has<ActionRequester<TRequester>>, Has<ActionRequesterTarget<TRequester>>, Has<ActionRequest<T>>>>();
        }

        protected override void Execute(Entity entity)
        {
            entity.DestroyComponent<ActionRequester<TRequester>>();
            entity.DestroyComponent<ActionRequest<T>>();
            entity.DestroyComponent<ActionRequesterTarget<TRequester>>();
        }
    }
}