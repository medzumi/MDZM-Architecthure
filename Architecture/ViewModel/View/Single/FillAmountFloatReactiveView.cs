using System;
using UnityEngine;
using UnityEngine.UI;

namespace Architecture.ViewModel.View.Single
{
    public class FillAmountFloatReactiveView : FloatReactiveView
    {
        [SerializeField] private Image _image;

        public override void OnCompleted()
        {
        }

        public override void OnError(Exception error)
        {
        }

        public override void OnNext(float value)
        {
            _image.fillAmount = value;
        }
    }
}
