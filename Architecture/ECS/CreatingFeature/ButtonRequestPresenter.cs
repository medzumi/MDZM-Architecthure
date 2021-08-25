using Architecture.Presenting;
using Architecture.ViewModel;
using ECS;

namespace Architecture.ECS.CreatingFeature
{
    public class ButtonRequestPresenter<T> : ECSPresenter<ViewModel.ViewModel, ButtonRequestPresenter<T>.Instruction>
    {
        public class Instruction : PresenterInstruction, ICommand
        {
            private Entity _entity;
            
            public override void Present(Entity entity, ViewModel.ViewModel view)
            {
                _entity = entity;
                view.AddCommand("click", this);
            }

            public override void StopPresent(Entity entity, ViewModel.ViewModel view)
            {
                _entity = null;
                view.RemoveCommand("click", this);
            }

            public void Execute()
            {
                if (!_entity.HasComponent<ActionRequester<T>>())
                {
                    var cmp = _entity.CreateComponent<ActionRequester<T>>();
                    cmp.Requester = _entity.GetComponent<T>();
                }
            }
        }

        public ButtonRequestPresenter(IPresentContext<ViewModel.ViewModel> context) : base(context)
        {
        }
    }
}