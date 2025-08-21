using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetSpeed", story: "Set [Speed] multiplier to [NavMovement]", category: "Action", id: "cf4c46d07df99300a0ec47246971fa68")]
    public partial class SetSpeedAction : Action
    {
        [SerializeReference] public BlackboardVariable<float> Speed;
        [SerializeReference] public BlackboardVariable<NavMovement> NavMovement;

        protected override Status OnStart()
        {
            NavMovement.Value.SpeedMultiplier = Speed;
            return Status.Success;
        }

    }
}

