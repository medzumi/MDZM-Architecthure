using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using Zenject;

namespace Architecture.Entitas.Zenject
{
    public class EntitasExecuter : MonoBehaviour
    {
        private Systems _systems;

        [Inject]
        private void Construct(Systems systems, List<ISystem> systemList)
        {
            _systems = systems;
            foreach (var system in systemList)
            {
                _systems.Add(system);
            }
        }

        private void Update()
        {
            _systems.Execute();
        }
    }
}