using System.Collections.Generic;

namespace Architecture.ECS
{
    public class ListenerComponent<T>
    {
        public readonly List<IComponentListener<T>> Listeners = new List<IComponentListener<T>>();
    }

    public interface IComponentListener<T>
    {
        void Notify(T component);
    }
}