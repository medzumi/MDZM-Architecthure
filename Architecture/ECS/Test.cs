using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Architecture.ECS
{
    public class TestComponent
    {
        public Vector3 position;
    }

    public class TestMComponent : MonoBehaviour, IComponentListener<TestComponent>
    {
        public void Notify(TestComponent component)
        {
            transform.position = component.position;
        }
    }
    
    public class TestContext : MContext<TestContext>
    {
        
    }
    
    public class Test : MonoBehaviour, IComponentListener<Transform>, IExecuteSystem
    {
        private TestContext _testContext = new TestContext();

        private Systems _systems;

        private Collector<TestComponent> _collector;

        private void Awake()
        {
            var reactiveSystem = new ReactiveSystem<TestComponent>(_testContext);
            _systems = new Systems(_testContext, new List<IExecuteSystem>()
            {
                this,
                reactiveSystem
            }, new List<ICleanupSystem>()
            {
                reactiveSystem
            });
            _collector = _testContext.GetCollector<Collector<TestComponent>>();
            for (int i = 0; i < 5000; i++)
            {
                var entity = _testContext.CreateEntity();
                var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var mcomp = primitive.AddComponent<TestMComponent>();
                var comp = entity.CreateComponentOnEntity<TestComponent>();
                comp.position = new Vector3(UnityEngine.Random.Range(-3f, 3f),
                    UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f));
                if(i < 5000)
                    entity.CreateComponentOnEntity<ListenerComponent<TestComponent>>().Listeners.Add(mcomp);
                entity.NotifyUpdateContinuously<TestComponent>();
            }
        }

        public void Notify(Transform component)
        {
         //   transform.position = component.position;
        }

        private void Update()
        {
            _systems.Execute();
        }

        public void Execute()
        {
            foreach (var VARIABLE in _collector.collectedEntities)
            {
                VARIABLE.GetComponent<TestComponent>().position = Vector3.one * UnityEngine.Random.Range(-1f, 1f);
            }
        }
    }
}