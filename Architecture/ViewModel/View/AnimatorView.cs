using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Architecture.ViewModel.View
{
    public class AnimatorView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
    
        [SerializeField] private List<BindProperty<float>> _floatBinds;
        [SerializeField] private List<BindProperty<bool>> _boolBinds;
        [SerializeField] private List<BindProperty<int>> _intBinds;

        private void OnEnable()
        {
            foreach (var VARIABLE in _floatBinds)
            {
                VARIABLE.GetBind().Subscribe(async (v) =>
                {
                    await UniTask.SwitchToMainThread(); _animator.SetFloat(VARIABLE.Key, v); 
                });
            }
            foreach (var VARIABLE in _intBinds)
            {
                VARIABLE.GetBind().Subscribe(async (v) =>
                {
                    await UniTask.SwitchToMainThread(); _animator.SetInteger(VARIABLE.Key, v); 
                });
            }
            foreach (var VARIABLE in _boolBinds)
            {
                VARIABLE.GetBind().Subscribe(async (v) =>
                {
                    await UniTask.SwitchToMainThread(); _animator.SetBool(VARIABLE.Key, v); 
                });
            }
        }
    }
}
