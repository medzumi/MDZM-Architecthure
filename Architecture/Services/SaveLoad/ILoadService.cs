namespace Architecture.Services.SaveLoad
{
    public interface ILoadService<TValue, in TKey>
    {
        TValue Load(TKey loadId);
        void Load(out TValue value, TKey loadId);

        bool TryLoad(TKey loadId, out TValue value);
    }
}