using UnityEngine;

namespace Architecture.Presenting
{
    [DefaultExecutionOrder(-105)]
    public class ReserveView : MonoBehaviour
    {
        [SerializeField] private ViewModel.ViewModel _viewModel;
        [SerializeField] private int _index;

        [SerializeField] private PresentContextScriptableObject _context;

        private void Awake()
        {
            _context.ReserveView(_viewModel, _index);
        }
    }
}