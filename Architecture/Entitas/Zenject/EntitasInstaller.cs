using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.TypeProperty;
using Entitas;
using UnityEngine;
using Zenject;

namespace Architecture.Entitas.Zenject
{
    public class EntitasInstaller : MonoInstaller
    {
        [SerializeField] [Inherits(typeof(Systems))]
        private TypeReference _systemsType;

        [SerializeField]
        [Inherits(typeof(IContexts))]
        private TypeReference _contextstype;
        
        [SerializeField]
        [Inherits(typeof(IContext))]
        private TypeReference[] _contextTypes;

        [SerializeField] [Inherits(typeof(ISystem))]
        private List<TypeReference> _systemTypes;

        private Systems _systems;

        public override void InstallBindings()
        {
            _systems = (Systems)Activator.CreateInstance(_systemsType);
            Container.Bind<Systems>().FromInstance(_systems);
            Container.BindInterfacesAndSelfTo(_contextstype).FromNew().AsCached();
            foreach (var VARIABLE in _contextTypes)
            {
                Container.BindInterfacesAndSelfTo(VARIABLE).FromMethodUntyped(ctx =>
                {
                    return ctx.Container.Resolve<IContexts>().allContexts
                        .FirstOrDefault(ectx => ectx.GetType().Equals(VARIABLE));
                }).AsCached();
            }

            foreach (var type in _systemTypes)
            {
                Container.Bind<ISystem>().To(type).FromNew().AsCached();
            }
            Container.Bind<List<ISystem>>().FromResolveGetter<Context>(ctx => ctx.Container.ResolveAll<ISystem>()).AsCached();
        }
    }
}