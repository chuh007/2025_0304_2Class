using System;
using Blade.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChangeClip", story: "[MainAnimator] change [oldClip] to [newClip]", category: "Action/Enemy", id: "dc1373cf5c799f8d8be15bcd67eb7e44")]
    public partial class ChangeClipAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityAnimator> MainAnimator;
        [SerializeReference] public BlackboardVariable<string> OldClip;
        [SerializeReference] public BlackboardVariable<string> NewClip;

        protected override Status OnStart()
        {
            int oldHash = Animator.StringToHash(OldClip.Value);
            int newHash = Animator.StringToHash(NewClip.Value);
            MainAnimator.Value.SetParam(oldHash, false);
            MainAnimator.Value.SetParam(newHash, true);
        
            OldClip.Value = NewClip.Value;
            return Status.Success;
        }
    }
}

