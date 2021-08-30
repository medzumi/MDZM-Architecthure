using System;
using System.Reflection;
using Architecture.Presenting;
using Architecture.Utilities;
using ECS;
using ECS.Unity;
using UnityEngine;

namespace Architecture.ECS
{
    public class GameObjectToEntityAndView : MonoBehaviour
    {
        [SerializeField] private UniDictionary<ViewModel.ViewModel, PresentContextScriptableObject> _pairs;
        [SerializeField] private Blueprint _blueprint;
        
        private Entity _entity = null;
        
        private void Awake()
        {
            var entity = Contexts.sharedContext.CreateEntity();
            var index = entity.Index;
            _entity = entity;
            if (_blueprint)
            {
                foreach (var component in _blueprint.Components)
                {
                    //ToDo remove allocation to _blueprint prebake
                    var t = component.GetType();
                    var newComponent = Activator.CreateInstance(t);
                    var fieldInfos = t.GetFields(BindingFlags.Instance | BindingFlags.Public);
                    foreach (var fieldInfo in fieldInfos)
                    {
                        fieldInfo.SetValue(newComponent, fieldInfo.GetValue(component));
                    }

                    entity.AddComponent(newComponent);
                }
            }

            if (TryGetComponent<IBlueprint>(out var blueprint))
            {
                foreach (var component in blueprint.Components)
                {
                    //ToDo remove allocation to _blueprint prebake
                    var t = component.GetType();
                    var newComponent = Activator.CreateInstance(t);
                    var fieldInfos = t.GetFields(BindingFlags.Instance | BindingFlags.Public);
                    foreach (var fieldInfo in fieldInfos)
                    {
                        fieldInfo.SetValue(newComponent, fieldInfo.GetValue(component));
                    }

                    entity.AddComponent(newComponent);
                }
            }

            foreach (var pair in _pairs)
            {
                pair.Value.ReserveView(pair.Key, index);
            }
        }
    }
}