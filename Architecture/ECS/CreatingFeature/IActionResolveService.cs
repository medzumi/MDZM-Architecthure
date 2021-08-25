using ECS;

namespace Architecture.ECS.CreatingFeature
{
    public interface IActionResolveService<T>
    {
        void Resolve(Context context, Entity target, Entity requester, T request);
    }
}