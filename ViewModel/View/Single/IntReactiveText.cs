using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Architecture.ViewModel.View.Single
{
    public class IntReactiveText : IntReactiveView
    {
        [SerializeField] private Text _text;
    
        public override void OnCompleted()
        {
        }

        public override void OnError(Exception error)
        {
        }

        public override async void OnNext(int value)
        {
            await UniTask.SwitchToMainThread();
            _text.text = value.ToString();
        }
    }
}
