using Architecture.ViewModel;
using ECS;

namespace Architecture.ECS.CreatingFeature
{
    public class ButtonCloseRequestInstruction<T> : PresenterInstruction<ViewModel.ViewModel>, ICommand
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
}