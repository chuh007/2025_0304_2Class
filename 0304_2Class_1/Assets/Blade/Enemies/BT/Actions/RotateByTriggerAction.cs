using Blade.Enemies;
using Blade.Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateByTrigger", story: "[Movement] rotate to [Target] by [Trigger]", category: "Enemy/Move", id: "30eb3326da6036751635cfbef988cf6e")]
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

