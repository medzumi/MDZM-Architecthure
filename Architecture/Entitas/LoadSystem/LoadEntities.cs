using Architecture.Entitas;
using Architecture.Services.SaveLoad;
using Entitas;

namespace Architecture.Entitas.LoadSystem
{
    public class LoadEntities<T, TIdComponent, TComponentKey, TKey> : IInitializeSystem where T : Entity
        where TIdComponent : IComponent, IIdentifierComponent<TComponentKey>
    {
        private readonly ILoadService<TIdComponent[], TKey> _loadService;
        private readonly Context<T> _context;
        private readonly int _componentId;
        private readonly TKey _key;

        public LoadEntities(Context<T> context, ILoadService<TIdComponent[], TKey> loadService, TKey key)
        {
            _loadService = loadService;
            _context = context;
            _componentId = GenericComponentsLookup<T>.GetComponentId<TIdComponent>();
            _key = key;
        }
        
        public void Initialize()
        {
            foreach (var component in _loadService.Load(_key))
            {
                _context.CreateEntity()
                    .AddComponent(_componentId, component);
            }
        }
    }
}