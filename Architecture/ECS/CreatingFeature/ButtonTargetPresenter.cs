using System;
using Architecture.Presenting;
using Architecture.ViewModel;
using ECS;

namespace Architecture.ECS.CreatingFeature
{
    public class ButtonTargetPresenter : ECSPresenter<ViewModel.ViewModel, ButtonTargetPresenter.Instruction>
    {
        public event Action<Entity> ClickedEntity = delegate(Entity entity) {  };
        
        public class Instruction : PresenterInstruction<ViewModel.ViewModel>, ICommand
        {
            public ButtonTargetPresenter buttonTargetPresenter;
            private Entity _entity;
            
            public override void Present(Entity entity, ViewModel.ViewModel view)
            {
                view.AddCommand("click", this);
                _entity = entity;
            }

            public override void StopPresent(Entity entity, ViewModel.ViewModel view)
            {
                view.RemoveCommand("click", this);
            }

            public void Execute()
            {
                buttonTargetPresenter.ClickedEntity(_entity);
            }
        }

        public ButtonTargetPresenter(IPresentContext<ViewModel.ViewModel> context) : base(context)
        {
        }

        protected override void InjectInstruction(Instruction instruction)
        {
            base.InjectInstruction(instruction);
            instruction.buttonTargetPresenter = this;
        }
    }
}