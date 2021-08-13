using System;
using System.Collections.Generic;

namespace Architecture.ECS
{
    public abstract class Collector
    {
        internal MContext _mContext;

        private Dictionary<int, MEntity> _collectedEntities;
        public Dictionary<int, MEntity>.ValueCollection collectedEntities => _collectedEntities.Values;

        internal Collector()
        {
            
        }

        public void Initialize()
        {
            _collectedEntities = Pool<Dictionary<int, MEntity>>.Get();
            InitializeHandler();
        }

        protected virtual void InitializeHandler()
        {
            
        }

        public virtual void Dispose()
        {
            _collectedEntities.Clear();
            Pool<Dictionary<int, MEntity>>.Retain(_collectedEntities);
            _collectedEntities = null;
        }

        public abstract List<int> GetComponentIds<TContext>(List<int> list) where TContext : MContext<TContext>;

        internal void CheckAdded(MEntity entity)
        {
            var contains = _collectedEntities.ContainsKey(entity.index);
            var isMatched = IsMatched(entity);
            if(isMatched && !contains)
                _collectedEntities.Add(entity.index, entity);
        }

        internal void CheckRemove(MEntity entity)
        {
            var contains = _collectedEntities.ContainsKey(entity.index);
            var isMatched = IsMatched(entity);
            if (!isMatched && contains)
            {
                _collectedEntities.Remove(entity.index);
            }
        }
        
        public abstract bool IsMatched(MEntity entity);
    }

    public class Collector<T1> : Collector
    {
        public override List<int> GetComponentIds<TContext>(List<int> list)
        {
            list.Add(MContext<TContext>.GetStaticComponentId<T1>());
            return list;
        }

        public override bool IsMatched(MEntity entity)
        {
            return entity.HasComponent<T1>();
        }
    }

    public class CollectorAll<T1, T2> : Collector<T1>
    {
        public override List<int> GetComponentIds<TContext>(List<int> list)
        {
            list.Add(MContext<TContext>.GetStaticComponentId<T2>());
            return base.GetComponentIds<TContext>(list);;
        }

        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) && entity.HasComponent<T2>();
        }
    }

    public class CollectorAll<T1, T2, T3> : CollectorAll<T1, T2>
    {
        public override List<int> GetComponentIds<TContext>(List<int> list)
        {
            list.Add(MContext<TContext>.GetStaticComponentId<T3>());
            return base.GetComponentIds<TContext>(list);
        }

        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) && entity.HasComponent<T3>();
        }
    }

    public class CollectorAll<T1, T2, T3, T4> : CollectorAll<T1, T2, T3>
    {
        public override List<int> GetComponentIds<TContext>(List<int> list)
        {
            list.Add(MContext<TContext>.GetStaticComponentId<T4>());
            return base.GetComponentIds<TContext>(list);
        }

        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) && entity.HasComponent<T4>();
        }
    }

    public class CollectorAny<T1, T2> : Collector<T1>
    {
        public override List<int> GetComponentIds<TContext>(List<int> list)
        {
            list.Add(MContext<TContext>.GetStaticComponentId<T2>());
            return base.GetComponentIds<TContext>(list);
        }

        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) || entity.HasComponent<T2>();
        }
    }

    public class CollectorAny<T1, T2, T3> : CollectorAny<T1, T2>
    {
        public override List<int> GetComponentIds<TContext>(List<int> list)
        {
            list.Add(MContext<TContext>.GetStaticComponentId<T3>());
            return base.GetComponentIds<TContext>(list);
        }

        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) || entity.HasComponent<T3>();
        }
    }

    public class CollectorAny<T1, T2, T3, T4> : CollectorAny<T1, T2, T3>
    {
        public override List<int> GetComponentIds<TContext>(List<int> list)
        {
            list.Add(MContext<TContext>.GetStaticComponentId<T4>());
            return base.GetComponentIds<TContext>(list);
        }

        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) || entity.HasComponent<T4>();
        }
    }

    public class CollectorUpdated<T1> : Collector<UpdateSingleComponent<T1>>
    {
    }

    public class CollectorAllUpdated<T1, T2> : CollectorAll<UpdateSingleComponent<T1>, UpdateSingleComponent<T2>>
    {
    }

    public class
        CollectorAllUpdated<T1, T2, T3> : CollectorAll<UpdateSingleComponent<T1>, UpdateSingleComponent<T2>, UpdateSingleComponent<T3>>
    {
        
    }

    public class CollectorAnyUpdated<T1, T2> : CollectorAny<UpdateSingleComponent<T1>, UpdateSingleComponent<T2>>
    {
        
    }

    public class
        CollectorAnyUpdated<T1, T2, T3> : CollectorAny<UpdateSingleComponent<T1>, UpdateSingleComponent<T2>, UpdateSingleComponent<T3>>
    {
        
    }
}