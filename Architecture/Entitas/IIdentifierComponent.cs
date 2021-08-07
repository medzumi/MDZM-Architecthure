namespace Architecture.Entitas
{
    public interface IIdentifierComponent<out TKey>
    {
        TKey Key { get; }
    }
}