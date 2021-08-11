using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Architecture.ECS
{
    public class MEntity
    {
        internal int index;
        internal MContext _context;

        private List<object> _components = new List<object>();
        
        internal MContext Context => _context;

        public T CreateComponentOnEntity<T>()
            where T : class, new()
        {
            return AddComponent(Pool<T>.Get());
        }

        public bool HasComponent<T>()
        {
            return true;
        }

        public T AddComponent<T>(T component)
            where T : class, new()
        {
            var componentIndex = _context.GetComponentId<T>();
            
            CheckComponents(componentIndex);
            
            if (_components[componentIndex] != null)
                throw new Exception("Component Already Added");
            _components[componentIndex] = component;
            return component;
        }

        public T ReplaceComponent<T>(T component)
            where T : class, new()
        {
            var componentIndex = _context.GetComponentId<T>();

            CheckComponents(componentIndex);

            var oldComponent = (T)_components[componentIndex];
            if(oldComponent!=null)
                Pool<T>.Retain(oldComponent);

            _components[componentIndex] = component;
            return component;
        }

        public void RemoveComponent<T>()
            where T : class, new()
        {
            var componentIndex = _context.GetComponentId<T>();
            
            CheckComponents(componentIndex);

            var oldComponent = (T)_components[componentIndex];
            if (oldComponent == null)
                throw new Exception("Entity hasn't component");
            _components[componentIndex] = null;
            Pool<T>.Retain(oldComponent);
        }

        public T NotifyUpdateComponent<T>()
            where T : class, new()
        {
            return default;
        }
        public int Index => index;

        private void CheckComponents(int componentCount)
        {
            if (_components.Count <= componentCount)
            {
                for(int i = 0; i < _components.Count - componentCount; i++)
                    _components.Add(null);
            }
        }
    }
}