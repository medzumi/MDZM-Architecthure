using Architecture.Presenting;
using Architecture.ViewModel;
using ECS;

namespace Architecture.ECS.CreatingFeature
{
    public class ButtonCloseRequestPresenter<T> : ECSPresenter<ViewModel.ViewModel, ButtonCloseRequestPresenter<T>.ButtonCloseRequestInstruction>
    {
        public class ButtonCloseRequestInstruction : PresenterInstruction, ICommand
        {
            private Entity _entity;
            
            public override void Present(Entity entity, ViewModel.ViewModel view)
            {
                view.AddCommand("close_click", this);
            }

            public override void StopPresent(Entity entity, ViewModel.ViewModel view)
            {
                view.RemoveCommand("close_click", this);
            }

            public void Execute()
            {
                _entity.DestroyComponent<ActionRequester<T>>();
            }
        }

        public ButtonCloseRequestPresenter(IPresentContext<ViewModel.ViewModel> context) : base(context)
        {
        }
    }
}