namespace Architecture.ECS
{
    public class ReactiveSystem<T> : IExecuteSystem
        where T : class
    {
        private readonly CollectorUpdated<T> _collector;
        
        public ReactiveSystem(MContext ctx)
        {
            _collector = ctx.GetCollector<CollectorUpdated<T>>();
        }
        
        public void Execute()
        {
            foreach (var entity in _collector.collectedEntities)
            {
                var component = entity.GetComponent<T>();
                foreach (var VARIABLE in entity.GetComponent<ListenerComponent<T>>().Listeners)
                {
                    VARIABLE.Notify(component);   
                }
            }
        }
    }
}