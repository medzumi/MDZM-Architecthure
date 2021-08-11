namespace Architecture.ECS
{
    public static class ContextFabric
    {
        internal static int _activeContexts = -1;
        
        public static MContext CreateContext()
        {
            _activeContexts++;
            var ctx = Pool<MContext>.Get();
            ctx.id = _activeContexts;
        }
    }
}