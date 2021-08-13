using System.Collections.Generic;

namespace Architecture.ECS
{
    public class ReactiveSystem<T> : IExecuteSystem, ICleanupSystem
        where T : class
    {
        private readonly ReactiveExecuteSystem<UpdateContinuouslyComponent<T>> _reactiveExecuteSystem;
        private readonly ReactiveExecuteSystem<UpdateSingleComponent<T>> _reactiveExecuteSystem2;
        private readonly ReactiveCleanUpSystem _reactiveCleanUpSystem;

        public ReactiveSystem(MContext context)
        {
            _reactiveExecuteSystem = new ReactiveExecuteSystem<UpdateContinuouslyComponent<T>>(context);
            _reactiveExecuteSystem2 = new ReactiveExecuteSystem<UpdateSingleComponent<T>>(context);
            _reactiveCleanUpSystem = new ReactiveCleanUpSystem(context);
        }

        public void Execute()
        {
            _reactiveExecuteSystem.Execute();
            _reactiveExecuteSystem2.Execute();
        }

        public void Cleanup()
        {
            _reactiveCleanUpSystem.Cleanup();
        }

        private class ReactiveExecuteSystem<T1> : IExecuteSystem
        {
            private readonly CollectorAll<T1, ListenerComponent<T>> _collector;

            public ReactiveExecuteSystem(MContext context)
            {
                _collector = context.GetCollector<CollectorAll<T1, ListenerComponent<T>>>();
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
            private readonly Collector<UpdateSingleComponent<T>> _collector;
            private List<MEntity> _buffer = new List<MEntity>();

            public ReactiveCleanUpSystem(MContext context)
            {
                _collector = context.GetCollector<Collector<UpdateSingleComponent<T>>>();
            }
            
            public void Cleanup()
            {
                _buffer.AddRange(_collector.collectedEntities);
                foreach (var VARIABLE in _buffer)
                {
                    VARIABLE.RemoveComponent<UpdateSingleComponent<T>>();
                }
                _buffer.Clear();
            }
        }
    }
}