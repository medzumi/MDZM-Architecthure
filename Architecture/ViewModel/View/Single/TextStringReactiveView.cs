using System;
using UnityEngine;
using UnityEngine.UI;

namespace Architecture.ViewModel.View.Single
{
    public class TextStringReactiveView : StringReactiveView
    {
        [SerializeField] private Text _text;

        public override void OnCompleted()
        {
        }

        public override void OnError(Exception error)
        {
        }

        public override void OnNext(string value)
        {
            _text.text = value;
        }
    }
}
