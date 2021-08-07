using System;
using UnityEngine;

namespace Architecture.ViewModel.View.Single
{
    public class PositionFloatReactiveView : FloatReactiveView, ISerializationCallbackReceiver
    {
        public enum Orientation
        {
            X,
            Y
        }

        [SerializeField] private Orientation _orientation;

        private Func<float, Vector2> _func;
    
        public override void OnCompleted()
        {
        }

        public override void OnError(Exception error)
        {
        }

        public override void OnNext(float value)
        {
            transform.position = _func(value);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            switch (_orientation)
            {
                case Orientation.X:
                    _func = f => new Vector2(f, transform.position.y);
                    break;
                case Orientation.Y:
                    _func = f => new Vector2(transform.position.x, f);
                    break;
            }
        }
    }
}
