using System;
using System.Collections.Generic;
using Entitas;

namespace Architecture.ECS
{
    public abstract class MContext<T> : MContext where T : MContext<T>
    {
        public sealed override int GetComponentId<T1>()
        {
            return Indexator<T, T1>.ComponentId;
        }

        public sealed override int ComponentCount()
        {
            return Indexator<T>.ComponentsCount + 1;
        }

        internal sealed override void ComponentAdded<T1>(MEntity entity, T1 component)
        {
        }
    }

    public abstract class MContext : IDisposable
    {
        //ToDo : Create offseted list
        private int count = 0;

        private Stack<int> _reservedIndexes = new Stack<int>();

        private readonly List<MEntity> _entities = new List<MEntity>();

        private readonly Dictionary<Type, Collector> _collectors = new Dictionary<Type, Collector>();

        public T GetCollector<T>()
            where T:Collector, new()
        {
            if (!_collectors.TryGetValue(typeof(T), out var collector))
            {
                collector = new T();
                _collectors.Add(typeof(T), collector);
            }

            return collector as T;
        }

        public void Update()
        {
            foreach (var keyValuePair in _collectors)
            {
                foreach (var entity in _entities)
                {
                    keyValuePair.Value.CheckEntity(entity);
                }
            }
        }

        public void Complete()
        {
            foreach (var keyValuePair in _collectors)
            {
                keyValuePair.Value.Complete();
            }
        }

        public MEntity CreateEntity()
        {
            var entity = Pool<MEntity>.Get();
            entity._context = this;
            var index = entity.index = GetIndex();
            if (index >= _entities.Count)
            {
                for (int i = 0; i < _entities.Count; i++)
                {
                    _entities.Add(null);
                }
            }

            _entities[index] = entity;
            return entity;
        }

        public MEntity GetEntity(int index)
        {
            return _entities[index];
        }

        private int GetIndex()
        {
            var index = _reservedIndexes.Count > 0 ? _reservedIndexes.Pop() : count;
            count++;
            return index;
        }

        public abstract int GetComponentId<T>();

        public abstract int ComponentCount();

        public void RetainEntity(MEntity entity)
        {
            _entities[entity.index] = null;
            Pool<MEntity>.Retain(entity);
        }

        internal abstract void ComponentAdded<T>(MEntity entity, T component);

        public void Dispose()
        {
        }
    }
}