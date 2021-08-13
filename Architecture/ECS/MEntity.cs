using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Architecture.Data;

namespace Architecture.ECS
{
    public class MEntity
    {
        internal int index;
        internal MContext _context;
        internal List<object> _components = new List<object>();
        
        internal MContext Context => _context;
        
        public int Index => index;

        public T CreateComponentOnEntity<T>()
            where T : class, new()
        {
            return AddComponent(Pool<T>.Get());
        }

        [Obsolete("Use only for optimize index getting. You can broke logic with this method")]
        public T CreateComponentOnEntity<T>(int componentIndex)
            where T : class, new()
        {
            var obj = Pool<T>.Get();
            AddComponent(componentIndex, obj);
            return obj;
        }

        public bool HasComponent<T>()
        {
            var componentIndex = _context.GetComponentId<T>();
            return HasComponent(componentIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasComponent(int componentIndex)
        {
            return !ReferenceEquals(_components[componentIndex], null);
        }

        public T AddComponent<T>(T component)
            where T : class
        {
            var componentIndex = _context.GetComponentId<T>();
            AddComponent(componentIndex, component);
            return component;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddComponent(int componentIndex, object obj)
        {
            CheckComponents(componentIndex);
            if (_components[componentIndex] != null)
                throw new Exception("Component Already Added");
            _components[componentIndex] = obj;
            Context.ComponentAdded(this, componentIndex);
        }

        public T GetComponent<T>()
            where T : class
        {
            return (T)GetComponent(_context.GetComponentId<T>());
        }

        [Obsolete("Use only for optimize index getting")]
        public object GetComponent(int componentIndex)
        {
            object oldComponent = null;
            if (componentIndex >= _components.Count && (oldComponent = _components[componentIndex]) == null)
                throw new Exception("Entity hasn't component");
            return _components[componentIndex];
        }
        
        public void RemoveComponent<T>()
            where T : class, new()
        {
            var componentIndex = _context.GetComponentId<T>();
            Pool<T>.Retain((T)GetComponent(componentIndex));
            _components[componentIndex] = null;
            Context.ComponentRemoved(this, componentIndex);
        }

        public void NotifyUpdateComponent<T>()
            where T : class
        {
            var componentIndex = _context.GetComponentId<UpdateSingleComponent<T>>();
            if (componentIndex >= _components.Count || _components[componentIndex] == null)
                CreateComponentOnEntity<UpdateSingleComponent<T>>();
        }

        private void CheckComponents(int componentCount)
        {
            if (_components.Count <= componentCount)
            {
                for(int i = _components.Count; i <= componentCount; i++)
                    _components.Add(null);
            }
        }
    }
}