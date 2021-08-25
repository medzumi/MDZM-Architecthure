using System.Collections.Generic;
using System.Linq;

namespace Architecture.Presenting
{
    public interface IPresenter<T>
    {
        void Present(T model, int key);

        void StopPresent(T model, int key);
    }
}