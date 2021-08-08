using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using TypeReferences;
using UnityEngine;
using Zenject;

namespace Architecture.Entitas.Zenject
{
    public class EntitasInstaller : MonoInstaller
    {
        [SerializeField] [Inherits(typeof(Systems), UseBuiltInNames = true, IncludeAdditionalAssemblies = new []{"Assembly-CSharp"})]
        private TypeReference _systemsType;

        [SerializeField]
        [Inherits(typeof(IContexts), UseBuiltInNames = true, IncludeAdditionalAssemblies = new[] {"Assembly-CSharp"})]
        private TypeReference _contextstype;
        
        [SerializeField]
        [Inherits(typeof(IContext), UseBuiltInNames = true, IncludeAdditionalAssemblies = new[] {"Assembly-CSharp"})]
        private TypeReference[] _contextTypes;

        [SerializeField] [Inherits(typeof(ISystem), AllowInternal = true, UseBuiltInNames = true, IncludeAdditionalAssemblies = new []{"Assembly-CSharp"})]
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