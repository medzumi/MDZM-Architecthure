using UnityEngine;

namespace Architecture.Utilities
{
    public class ComplexViewController : ViewController
    {
        [SerializeField] private ViewController[] _viewControllers;
    
        public override void SetModel(object modelValue)
        {
            foreach (var viewController in _viewControllers)
            {
                viewController.SetModel(modelValue);
            }
        }
    }
}
