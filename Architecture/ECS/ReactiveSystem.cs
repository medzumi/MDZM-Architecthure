using System.Collections.Generic;

namespace Architecture.ECS
{
    public class ReactiveSystem<T> : IExecuteSystem, ICleanupSystem
        where T : class
    {
        private readonly ReactiveExecuteSystem _reactiveExecuteSystem;
        private readonly ReactiveCleanUpSystem _reactiveCleanUpSystem;

        public ReactiveSystem(MContext context)
        {
            _reactiveExecuteSystem = new ReactiveExecuteSystem(context);
            _reactiveCleanUpSystem = new ReactiveCleanUpSystem(context);
        }

        public void Execute()
        {
            _reactiveExecuteSystem.Execute();
        }

        public void Cleanup()
        {
            _reactiveCleanUpSystem.Cleanup();
        }

        private class ReactiveExecuteSystem : IExecuteSystem
        {
            private readonly CollectorAll<UpdateComponent<T>, ListenerComponent<T>> _collector;

            public ReactiveExecuteSystem(MContext context)
            {
                _collector = context.GetCollector<CollectorAll<UpdateComponent<T>, ListenerComponent<T>>>();
            }
            
            public void Execute()
            {
                foreach (var mEntity in _collector.collectedEntities)
                {
                    var comp = mEntity.GetComponent<T>();
                    foreach (var listener in mEntity.GetComponent<ListenerComponent<T>>().Listeners)
                    {
                        listener.Notify(comp);
                    }
                }
            }
        }
        
        private class ReactiveCleanUpSystem : ICleanupSystem
        {
            private readonly Collector<UpdateComponent<T>> _collector;
            private List<MEntity> _buffer = new List<MEntity>();

            public ReactiveCleanUpSystem(MContext context)
            {
                _collector = context.GetCollector<Collector<UpdateComponent<T>>>();
            }
            
            public void Cleanup()
            {
                _buffer.AddRange(_collector.collectedEntities);
                foreach (var VARIABLE in _buffer)
                {
                    VARIABLE.RemoveComponent<UpdateComponent<T>>();
                }
                _buffer.Clear();
            }
        }
    }
}