using Entitas;

namespace Architecture.Entitas
{
    public static class EntityExtension
    {
        public static T GetComponent<T, TEntity>(this TEntity entity) where TEntity : Entity
        where T : IComponent
        {
            return (T) entity.GetComponent(GenericComponentsLookup<TEntity>.GetComponentId<T>());
        }

        public static T GetComponent<T>(this IEntity entity, int index)
        {
            return (T) entity.GetComponent(index);
        }
    }
}