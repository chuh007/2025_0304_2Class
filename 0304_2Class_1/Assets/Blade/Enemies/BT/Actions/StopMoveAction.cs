using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "StopMove", story: "Set [Movement] isStop to [NewValue]", category: "Enemy/Move", id: "58fa7b68ec1e67c96e57bb98e58fdd66")]
    public partial class StopMoveAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavMovement> Movement;
        [SerializeReference] public BlackboardVariable<bool> NewValue;

        protected override Status OnStart()
        {
            Movement.Value.SetStop(NewValue.Value);
            Movement.Value.SetDestination(Movement.Value.transform.position);
            return Status.Success;
        }
    }
}

