using System;
using Architecture.Presenting;
using Architecture.ViewModel;
using ECS;

namespace Architecture.ECS.CreatingFeature
{
    public class TempCommand : ICommand
    {
        public string id;
        public Entity entity;
        
        public void Execute()
        {
            ActionRequest<string> request;
            if (entity.HasComponent<ActionRequest<string>>())
            {
                request = entity.GetComponent<ActionRequest<string>>();
                request.RequestData = id;
                entity.NotifyUpdateSingle<ActionRequest<string>>();
            }
            else
            {
                request = entity.CreateComponent<ActionRequest<string>>();
                request.RequestData = id;
            }
        }
    }
            
    public class DescriptionCreatingPresentInstruction : PresenterInstruction<ViewModel.ViewModel>
    {
        private string[] _temp = new[]
        {
            "a",
            "b",
            "c"
        };

        private Action<string, IViewModel> _action;

        private Collection list;

        private Entity _entity;

        public DescriptionCreatingPresentInstruction()
        {
            _action = FillAction;
        }
        
        public override void Present(Entity entity, ViewModel.ViewModel view)
        {
            _entity = entity;
            list = view.GetListViewModel("buildings");
            list.Fill(_temp, _action);
            view.GetBoolProperty("opened").Value = true;
        }

        public override void StopPresent(Entity entity, ViewModel.ViewModel view)
        {
            view.GetBoolProperty("opened").Value = false;
            list.Retain();   
        }

        private void FillAction(string t, IViewModel viewModel)
        {
            viewModel.GetStringProperty("id").Value = t;
            viewModel.AddCommand("click", new TempCommand(){ id = t, entity = _entity});
        }
    }
}