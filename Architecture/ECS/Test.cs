using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Architecture.ECS
{
    public class TestContext : MContext<TestContext>
    {
        
    }
    
    public class Test : MonoBehaviour, IComponentListener<Transform>, IExecuteSystem
    {
        private TestContext _testContext = new TestContext();

        private Systems _systems;

        private Collector<Transform> _collector;

        private void Awake()
        {
            _systems = new Systems(_testContext, new List<IExecuteSystem>()
            {
                this,
                new ReactiveSystem<Transform>(_testContext)
            });
            _collector = _testContext.GetCollector<Collector<Transform>>();
            var entity = _testContext.CreateEntity();
            entity.AddComponent(transform);
            entity.CreateComponentOnEntity<ListenerComponent<Transform>>();
            entity.GetComponent<ListenerComponent<Transform>>().Listeners.Add(this);
        }

        public void Notify(Transform component)
        {
           ////// Debug.Log(component.position);
        }

        private void Update()
        {
            _systems.Execute();
        }

        public void Execute()
        {
            foreach (var VARIABLE in _collector.collectedEntities)
            {
                VARIABLE.GetComponent<Transform>().position = Vector3.one * UnityEngine.Random.Range(-1f, 1f);
                VARIABLE.NotifyUpdateComponent<Transform>();
            }
        }
    }
}