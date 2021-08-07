namespace Architecture.Presenting
{
    public interface IPresentContext<TValue, TKey>
    {
        TValue Get(TKey key);

        void Destroy(TKey key);
    }
}