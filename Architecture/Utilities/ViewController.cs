using UnityEngine;

namespace Architecture.Utilities
{
    public abstract class ViewController : MonoBehaviour
    {
        public abstract void SetModel(object modelValue);
    }

    public abstract class ViewController<T> : ViewController where T : class
    {
        [SerializeField] private T defaultModel;
    
        public T Model { get; private set; }

        protected void Awake()
        {
            if (Model == null)
            {
                SetModel(defaultModel);
            }
        }

        public sealed override void SetModel(object modelValue)
        {
            if (Model != null)
            {
                UnsetModelHandler(Model);
            }
            var model = modelValue as T;
            if (model != null)
            {
                SetModelHandler(model);
            }
        }

        protected virtual void SetModelHandler(T model)
        {
            Model = model;
        }

        protected virtual void UnsetModelHandler(T model)
        {
            Model = null;
        }
    }
}