using System;
using Blade.Enemies.Skeletons;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WarpTransform", story: "Warp [NavMovement] to [Self]", category: "Action", id: "ba5a442e9cbe2ba0f3c6b72331aaa0b3")]
    public partial class WarpTransformAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavMovement> NavMovement;
        [SerializeReference] public BlackboardVariable<CommonEnemy> Self;

        protected override Status OnStart()
        {
            NavMovement.Value.WarpToPosition(Self.Value.transform.position);
            return Status.Success;
        }
    }
}

