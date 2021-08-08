using System;
using System.Collections.Generic;
using Architecture.Entitas;
using Architecture.Services.SaveLoad;
using Entitas;

namespace Architecture.Entitas.LoadSystem
{
    public class LoadComponent<T, TComponent, TIdComponent, TKey> : ReactiveSystem<T>
        where T : Entity
        where TComponent : IComponent
        where TIdComponent : IComponent, IIdentifierComponent<TKey>
    {
        private readonly int _indexComponentId;
        private readonly int _tComponentId;
        private readonly ILoadService<TComponent, TKey> _loadService;

        public LoadComponent(IContext<T> context, ILoadService<TComponent, TKey> loadService) : base(context)
        {
            _loadService = loadService;
            _indexComponentId = GenericComponentsLookup<T>.GetComponentId<TIdComponent>();
            _tComponentId = GenericComponentsLookup<T>.GetComponentId<TComponent>();
        }

        public LoadComponent(ICollector<T> collector, ILoadService<TComponent, TKey> loadService) : base(collector)
        {
            _loadService = loadService;
            _indexComponentId = GenericComponentsLookup<T>.GetComponentId<TIdComponent>();
            _tComponentId = GenericComponentsLookup<T>.GetComponentId<TComponent>();
        }

        protected override ICollector<T> GetTrigger(IContext<T> context)
        {
            return context.CreateCollector(
                Matcher<T>.AllOf(
                    GenericComponentsLookup<T>.GetComponentId<TIdComponent>()
                    ).Added()
            );
        }

        protected override bool Filter(T entity)
        {
            return entity.HasComponent(_indexComponentId);
        }

        protected override void Execute(List<T> entities)
        {
            foreach (var entity in entities)
            {
                var id = entity.GetComponent<TIdComponent, T>().Key;
                if (_loadService.TryLoad(id, out var component))
                {
                    if (entity.HasComponent(_tComponentId))
                    {
                        entity.ReplaceComponent(_tComponentId, component);
                    }
                    else
                    {
                        entity.AddComponent(_tComponentId, component);
                    }
                }
                else
                {
                    if (entity.HasComponent(_tComponentId))
                    {
                        entity.RemoveComponent(_tComponentId);
                    }
                }
            }
        }
    }
}