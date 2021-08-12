using System;
using System.Collections.Generic;
using Entitas;

namespace Architecture.ECS
{
    public abstract class MContext<T> : MContext where T : MContext<T>
    {
        public sealed override int GetComponentId<T1>()
        {
            return GetStaticComponentId<T1>();
        }

        public static int GetStaticComponentId<T1>()
        {
            return Indexator<T, T1>.ComponentId;
        }

        public sealed override int ComponentCount()
        {
            return Indexator<T>.ComponentsCount + 1;
        }

        internal sealed override void ComponentAdded<T1>(MEntity entity)
        {
            var index = Indexator<T, T1>.ComponentId;
            if (index < _collectrors.Count)
            {
                foreach (var collector in _collectrors[index])
                {
                    collector.CheckAdded(entity);
                }
            }
        }

        internal sealed override void ComponentRemoved<T1>(MEntity entity)
        {
            var index = Indexator<T, T1>.ComponentId;
            if (index < _collectrors.Count)
            {
                foreach (var collector in _collectrors[index])
                {
                    collector.CheckRemove(entity);
                }
            }
        }

        internal sealed override void AddCollector<T1>(T1 collector)
        {
            var indexes = Pool<List<int>>.Get();
            foreach (var collectorComponentIndex in collector.GetComponentIds<T>(indexes))
            {
                for(int i = collectorComponentIndex - _collectrors.Count; i>=0; i--)
                    _collectrors.Add(Pool<List<Collector>>.Get());
                _collectrors[collectorComponentIndex].Add(collector);
            }
        }
    }

    public abstract class MContext : IDisposable
    {
        //ToDo : Create offseted list
        private int count = 0;

        private Stack<int> _reservedIndexes;

        private List<MEntity> _entities;

        internal protected Dictionary<Type, Collector> _dictCollectors;
        internal protected List<List<Collector>> _collectrors;

        public MContext()
        {
            _dictCollectors = Pool<Dictionary<Type, Collector>>.Get();
            _collectrors = Pool<List<List<Collector>>>.Get();
            _entities = Pool<List<MEntity>>.Get();
            _reservedIndexes = Pool<Stack<int>>.Get();
        }
        
        public T GetCollector<T>()
            where T:Collector, new()
        {
            if (!_dictCollectors.TryGetValue(typeof(T), out var collector))
            {
                collector = Pool<T>.Get();
                collector.Initialize();
                _dictCollectors.Add(typeof(T), collector);
                AddCollector(collector);
            }

            return collector as T;
        }

        internal abstract void AddCollector<T>(T collector) where T : Collector;

        public MEntity CreateEntity()
        {
            var entity = Pool<MEntity>.Get();
            entity._context = this;
            var index = entity.index = GetIndex();
            if (index >= _entities.Count)
            {
                for (int i = _entities.Count; i <= index; i++)
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

        internal abstract void ComponentAdded<T>(MEntity entity);

        internal abstract void ComponentRemoved<T>(MEntity entity);

        public void Dispose()
        {
            _dictCollectors.Clear();
            foreach (var collectors in _collectrors)
            {
                foreach (var collector in collectors)
                {
                    collector.Dispose();
                }
                collectors.Clear();   
                Pool<List<Collector>>.Retain(collectors);
            }
            _collectrors.Clear();
            _reservedIndexes.Clear();
            Pool<Dictionary<Type, Collector>>.Retain(_dictCollectors);
            Pool<List<List<Collector>>>.Retain(_collectrors);
            Pool<Stack<int>>.Retain(_reservedIndexes);
        }
    }
}