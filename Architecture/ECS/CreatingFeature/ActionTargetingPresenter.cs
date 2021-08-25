using Architecture.Presenting;
using ECS;

namespace Architecture.ECS.CreatingFeature
{
    public class ActionTargetingPresenter<T> : ECSPresenter<ButtonTargetPresenter, ActionTargetingPresenter<T>.Instruction>
    {
        public class Instruction : PresenterInstruction<ButtonTargetPresenter>
        {
            private Entity _entity;
            public override void Present(Entity entity, ButtonTargetPresenter view)
            {
                _entity = entity;
                //ToDo : allocation
                view.ClickedEntity += ClickedEntityHandler;
            }

            private void ClickedEntityHandler(Entity obj)
            {
                if (_entity.HasComponent<ActionRequesterTarget<T>>())
                {
                    _entity.GetComponent<ActionRequesterTarget<T>>().Target = obj;
                    _entity.NotifyUpdateSingle<ActionRequesterTarget<T>>();
                }
                else
                {
                    var comp = _entity.CreateComponent<ActionRequesterTarget<T>>();
                    comp.Target = obj;
                }
            }

            public override void StopPresent(Entity entity, ButtonTargetPresenter view)
            {
                view.ClickedEntity -= ClickedEntityHandler;
            }
        }

        public ActionTargetingPresenter(IPresentContext<ButtonTargetPresenter> context) : base(context)
        {
        }
    }
}