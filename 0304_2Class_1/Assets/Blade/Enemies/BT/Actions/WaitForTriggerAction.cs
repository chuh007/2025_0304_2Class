using System;
using Blade.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitForTrigger", story: "Wait for [Trigger] end", category: "Enemy/Animation", id: "2c0e6183e6599094aa34b8784923d8f1")]
    public partial class WaitForTriggerAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityAnimatorTrigger> Trigger;

        private bool _isTrigger;
    
        protected override Status OnStart()
        {
            _isTrigger = false;
            Trigger.Value.OnAnimationEndTrigger += HandleAnimationEnd;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return _isTrigger ? Status.Success : Status.Running;
        }

    
        protected override void OnEnd()
        {
            Trigger.Value.OnAnimationEndTrigger -= HandleAnimationEnd;
        }
    
        private void HandleAnimationEnd() => _isTrigger = true;

    }
}

