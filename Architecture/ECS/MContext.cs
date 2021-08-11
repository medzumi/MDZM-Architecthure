using System;
using System.Collections.Generic;

namespace Architecture.ECS
{
    public class MContext
    {
        private Stack<MEntity> _stack;
        private int count = Int32.MinValue;

        internal int id;
        
        public MEntity CreateEntity()
        {
            return _stack.Count != 0 ? _stack.Pop() : createEntity();
        }

        private MEntity createEntity()
        {
            var entity = new MEntity();
            entity.index = count;
            count++;
            return entity;
        }

        public void RetainEntity(MEntity entity)
        {
            _stack.Push(entity);
        }

        internal void TearDown()
        {
            ContextFabric._activeContexts--;
            Pool<MContext>.Retain(this);
        }
    }
}