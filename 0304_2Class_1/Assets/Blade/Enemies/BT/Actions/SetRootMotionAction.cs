using System;
using Blade.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetRootMotion", story: "Set [MainAnimator] rootMotion to [NewValue]", category: "Action", id: "149bd392f72b9ca445d193adc6ced462")]
    public partial class SetRootMotionAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityAnimator> MainAnimator;
        [SerializeReference] public BlackboardVariable<bool> NewValue;

        protected override Status OnStart()
        {
            MainAnimator.Value.ApplyRootMotion = NewValue;
            return Status.Success;
        }

    }
}

