using System;
using ECS;

namespace Architecture.ECS.CreatingFeature
{
    [Serializable]
    public class ActionRequesterTarget<T>
    {
        public Entity Target;
    }
}