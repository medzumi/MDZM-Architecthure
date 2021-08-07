using System;
using Architecture.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Architecture.ViewModel.View.Single
{
    public class ColorStringReactiveView : StringReactiveView
    {
        [SerializeField] private Graphic _graphic;
        [SerializeField] private SerializedDictionary<string, Color> _dictionary;
    
        public override void OnCompleted()
        {
        }

        public override void OnError(Exception error)
        {
        }

        public override void OnNext(string value)
        {
            if (_dictionary.TryGetValue(value, out var color))
            {
                _graphic.color = color;
            }
        }
    }
}
