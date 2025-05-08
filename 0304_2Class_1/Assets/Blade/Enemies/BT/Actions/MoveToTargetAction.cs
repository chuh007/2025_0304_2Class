using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "MoveToTarget", story: "[Movement] move to [Target]", category: "Enemy/Move", id: "7e77fb4cda4c0ef40db209b12b805d24")]
    public partial class MoveToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavMovement> Movement;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            Movement.Value.SetDestination(Target.Value.position);
            return Status.Success;
        }
    }
}

