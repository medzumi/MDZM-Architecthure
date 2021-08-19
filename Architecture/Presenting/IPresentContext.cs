namespace Architecture.Presenting
{
    public interface IPresentContext<TValue>
    {
        TValue Get(int key);

        void Destroy(int key);
    }
}