using System;
using Architecture.ECS;
using ECS;
using UnityEngine;
using Zenject;
using Context = ECS.Context;

namespace Architecture.Injecting
{
    public class PresenterInstaller : MonoInstaller
    {
        [SerializeField] private ConcretePresenterInstaller[] _installers;

        public override void InstallBindings()
        {
            foreach (var VARIABLE in _installers)
            {
                VARIABLE.InstallBindings(Container);
            }
        }

        private void Start()
        {
            foreach (var VARIABLE in Container.ResolveAll<PresenterSystem>())
            {
                Contexts.sharedContext.systems.AddSystem(VARIABLE);
            }

            foreach (var VARIABLE in Container.ResolveAll<StopPresenterSystem>())
            {
                Contexts.sharedContext.systems.AddSystem(VARIABLE);
            }
        }
    }
}