namespace Architecture.ECS
{
    public static class EntityExtensions
    {
        public static MEntity NotifyUpdateContinuously<T>(this MEntity entity)
        {
            entity.CreateComponentOnEntity<UpdateContinuouslyComponent<T>>();
            return entity;
        }

        public static MEntity NotifyStopUpdateContinuously<T>(this MEntity entity)
        {
            entity.RemoveComponent<UpdateContinuouslyComponent<T>>();
            return entity;
        }

        public static MEntity NotifyUpdateSingle<T>(this MEntity entity)
        {
            entity.CreateComponentOnEntity<UpdateSingleComponent<T>>();
            return entity;
        }
    }
}