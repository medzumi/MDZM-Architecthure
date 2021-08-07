using UnityEngine;
using UnityEngine.UI;

namespace Architecture.ViewModel.View.CommandViews
{
    [RequireComponent(typeof(Button))]
    public class CommandButton : MonoBehaviour
    {
        [SerializeField] private BindCommand _bindCommand;
        private Button _button;
        private ICommand _command;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            _command = _bindCommand.GetCommand();
        }

        private void OnClick()
        {
            _command.Execute();
        }
    }
}
