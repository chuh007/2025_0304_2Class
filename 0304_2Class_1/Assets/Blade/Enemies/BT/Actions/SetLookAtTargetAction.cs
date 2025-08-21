using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetLookAtTarget", story: "Set [Target] to [NavMovement] LookAt", category: "Action", id: "ea2ea480cbd37850bb64f5c00b548b72")]
    public partial class SetLookAtTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<NavMovement> NavMovement;

        protected override Status OnStart()
        {
            NavMovement.Value.SetLookAtTarget(Target.Value);
            return Status.Success;
        }
    }
}

