using UnityEngine;
using UnityEngine.Events;

namespace Architecture.ViewModel.Applicator
{
    public class UnityEventFloatApplicator : CompareApplicator<float>
    {
        [SerializeField] private UnityEvent<bool> _notifyCompare;

        protected override void NotifyBool(bool value)
        {
            _notifyCompare.Invoke(value);
        }
    }
}
