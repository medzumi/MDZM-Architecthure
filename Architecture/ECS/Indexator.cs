using Codice.Client.Common;

namespace Architecture.ECS
{
    internal static class Indexator<TContext> where TContext : MContext<TContext>
    {
        public static int ComponentsCount = -1;
    }
    
    internal static class Indexator<TContext, TComponent> where TContext : MContext<TContext>
    {
        public readonly static int ComponentId;

        static Indexator()
        {
            Indexator<TContext>.ComponentsCount++;
            ComponentId = Indexator<TContext>.ComponentsCount;
        }
    }
}