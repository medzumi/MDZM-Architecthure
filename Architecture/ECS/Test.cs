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
            var reactiveSystem = new ReactiveSystem<Transform>(_testContext);
            _systems = new Systems(_testContext, new List<IExecuteSystem>()
            {
                this,
                reactiveSystem
            }, new List<ICleanupSystem>()
            {
                reactiveSystem
            });
            _collector = _testContext.GetCollector<Collector<Transform>>();
            for (int i = 0; i < 5000; i++)
            {
                var entity = _testContext.CreateEntity();
                var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                primitive.transform.position = new Vector3(UnityEngine.Random.Range(-3f, 3f),
                    UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f));
                entity.AddComponent(primitive.transform);
                if(i < 5000)
                    entity.CreateComponentOnEntity<ListenerComponent<Transform>>().Listeners.Add(this);
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
                VARIABLE.GetComponent<Transform>().position = Vector3.one * UnityEngine.Random.Range(-1f, 1f);
                VARIABLE.NotifyUpdateComponent<Transform>();
            }
        }
    }
}