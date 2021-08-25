using Architecture.ECS;
using Architecture.Presenting;
using Architecture.TypeProperty;
using UnityEngine;
using Zenject;

namespace Architecture.Injecting
{
    [CreateAssetMenu]
    public class ConcretePresenterInstaller : ScriptableObject
    {
        [SerializeField] private string _presenterInjectId = string.Empty;

        [SerializeField] private ConcretePresenterInstaller[] _subPresenterInstallers;

        [SerializeField] private PresentContextScriptableObject _context;

        [SerializeField, Inherits(t: typeof(ECSPresenter<,>))]
        private TypeReference _presenterType;

        [SerializeField, Inherits(typeof(PresenterSystem))]
        private TypeReference[] _presentSystems;

        [SerializeField, Inherits(t: typeof(StopPresenterSystem))]
        private TypeReference[] _stopPresentSystems;
        
        public void InstallBindings(DiContainer Container)
        {
            var subContainer = Container.CreateSubContainer();
            if (string.IsNullOrEmpty(_presenterInjectId))
            {
                subContainer.Bind(_presenterType).AsCached();
            }
            else
            {
                subContainer.Bind(_presenterType).WithId(_presenterInjectId).AsCached();
            }

            subContainer.BindInstance(_context).WhenInjectedInto(_presenterType);

            foreach (var VARIABLE in _presentSystems)
            {
                if (!Container.HasBinding(VARIABLE))
                {
                    subContainer.Bind(VARIABLE).AsCached().WhenInjectedInto();
                    subContainer.Bind(_presenterType).WhenInjectedInto(VARIABLE);
                }
            }
            foreach (var VARIABLE in _stopPresentSystems)
            {
                if (!Container.HasBinding(VARIABLE))
                {
                    subContainer.Bind(VARIABLE).AsCached().WhenInjectedInto();
                    subContainer.Bind(_presenterType).WhenInjectedInto(VARIABLE);
                }
            }

            foreach (var VARIABLE in _subPresenterInstallers)
            {
                VARIABLE.InstallBindings(subContainer);
            }
        }
    }
}