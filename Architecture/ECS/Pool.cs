using System.Collections.Generic;

namespace Architecture.ECS
{
    internal static class Pool<TComponent>
        where TComponent : new()
    {
        private static readonly Stack<TComponent> _stack = new Stack<TComponent>();

        public static TComponent Get()
        {
            return _stack.Count != 0 ? _stack.Pop() : new TComponent();
        }

        public static void Retain(TComponent component)
        {
            _stack.Push(component);
        }
    }
}