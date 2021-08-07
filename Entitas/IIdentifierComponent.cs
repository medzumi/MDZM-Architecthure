namespace Entitas
{
    public interface IIdentifierComponent<out TKey>
    {
        TKey Key { get; }
    }
}