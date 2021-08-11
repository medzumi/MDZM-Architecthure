using System;
using System.Collections.Generic;

namespace Architecture.ECS
{
    public abstract class Collector
    {
        public readonly List<MEntity> collectedEntities = new List<MEntity>();

        internal void CheckEntity(MEntity entity)
        {
            if(IsMatched(entity))
                collectedEntities.Add(entity);
        }

        internal void Complete()
        {
            collectedEntities.Clear();
        }
        
        public abstract bool IsMatched(MEntity entity);
    }

    public class Collector<T1> : Collector
    {
        public override bool IsMatched(MEntity entity)
        {
            return entity.HasComponent<T1>();
        }
    }

    public class CollectorAll<T1, T2> : Collector<T1>
    {
        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) && entity.HasComponent<T2>();
        }
    }

    public class CollectorAll<T1, T2, T3> : CollectorAll<T1, T2>
    {
        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) && entity.HasComponent<T3>();
        }
    }

    public class CollectorAll<T1, T2, T3, T4> : CollectorAll<T1, T2, T3>
    {
        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) && entity.HasComponent<T4>();
        }
    }

    public class CollectorAny<T1, T2> : Collector<T1>
    {
        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) || entity.HasComponent<T2>();
        }
    }

    public class CollectorAny<T1, T2, T3> : CollectorAny<T1, T2>
    {
        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) || entity.HasComponent<T3>();
        }
    }

    public class CollectorAny<T1, T2, T3, T4> : CollectorAny<T1, T2, T3>
    {
        public override bool IsMatched(MEntity entity)
        {
            return base.IsMatched(entity) || entity.HasComponent<T4>();
        }
    }

    public class CollectorUpdated<T1> : Collector<UpdateComponent<T1>>
    {
    }

    public class CollectorAllUpdated<T1, T2> : CollectorAll<UpdateComponent<T1>, UpdateComponent<T2>>
    {
    }

    public class
        CollectorAllUpdated<T1, T2, T3> : CollectorAll<UpdateComponent<T1>, UpdateComponent<T2>, UpdateComponent<T3>>
    {
        
    }

    public class CollectorAnyUpdated<T1, T2> : CollectorAny<UpdateComponent<T1>, UpdateComponent<T2>>
    {
        
    }

    public class
        CollectorAnyUpdated<T1, T2, T3> : CollectorAny<UpdateComponent<T1>, UpdateComponent<T2>, UpdateComponent<T3>>
    {
        
    }
}