using System;
using Blade.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "RotateByTrigger", story: "[Movement] rotate to [Target] by [Trigger]", category: "Enemy/Move", id: "6d5487dd12580f1f267dd4afa9ec8e20")]
    public partial class RotateByTriggerAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavMovement> Movement;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<EntityAnimatorTrigger> Trigger;
        
        private bool _isRotate;
        protected override Status OnStart()
        {
            _isRotate = false;
            Trigger.Value.OnManualRotationTrigger += HandleManualRotation;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if(_isRotate)
                Movement.Value.LookAtTarget(Target.Value.position);
            return Status.Running;
        }

        protected override void OnEnd()
        {
            Trigger.Value.OnManualRotationTrigger -= HandleManualRotation;
        }
        
        private void HandleManualRotation(bool isRotate) => _isRotate = isRotate;
    }
}

