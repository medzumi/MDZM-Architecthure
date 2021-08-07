using UnityEngine;
using UnityEngine.Events;

namespace Architecture.ViewModel.Applicator
{
    public class UnityEventIntApplicator : CompareApplicator<int>
    {
        [SerializeField] private UnityEvent<bool> _notifyCompare;

        protected override void NotifyBool(bool value)
        {
            _notifyCompare.Invoke(value);
        }
    }
}
