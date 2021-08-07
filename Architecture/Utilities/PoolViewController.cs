using UnityEngine;

namespace Architecture.Utilities
{
    public class PoolViewController : ViewController, IPoolElement<PoolViewController>
    {
        [SerializeField] private ViewController _viewController;

        public IReleaser<PoolViewController> Releaser { get; set; } 
        
        public override void SetModel(object modelValue)
        {
            if (modelValue != null)
            {
                _viewController.SetModel(modelValue);
            }
            else
            {
                Releaser.Releaser(this);  
            }
        }
    }
}
