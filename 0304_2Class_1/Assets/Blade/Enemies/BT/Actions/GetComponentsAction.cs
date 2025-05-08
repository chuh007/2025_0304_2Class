using System;
using System.Collections.Generic;
using Blade.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "GetComponents", story: "Get components from [Self]", category: "Enemy", id: "799c2e2005dcf0eff852ea9fa61ca24a")]
    public partial class GetComponentsAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;

        protected override Status OnStart()
        {
            Enemy enemy = Self.Value;
            SetVariable(enemy, "Target", enemy.PlayerFinder.Target.transform);

            List<BlackboardVariable> varList = enemy.BtAgent.BlackboardReference.Blackboard.Variables;

            foreach (BlackboardVariable variable in varList)
            {
                if(typeof(IEntityComponent).IsAssignableFrom(variable.Type) == false) continue;
                
                SetComponent(enemy, variable.Name, enemy.GetCompo(variable.Type));
            }
            
            return Status.Success;
        }

        private void SetComponent(Enemy enemy, string varName, IEntityComponent component)
        {
            if (enemy.BtAgent.GetVariable(varName, out BlackboardVariable variable))
            {
                variable.ObjectValue = component;
            }
        }
        
        private void SetVariable<T>(Enemy enemy, string varName, T component)
        {
            Debug.Assert(component != null, $"Check {varName} in {enemy.name}");
            BlackboardVariable<T> target = enemy.GetBlackboardVariable<T>(varName);
            target.Value = component;
        }
    }
}

