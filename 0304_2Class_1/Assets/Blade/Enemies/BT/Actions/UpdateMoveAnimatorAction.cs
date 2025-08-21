using Blade.Enemies;
using Blade.Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UpdateMoveAnimator", story: "[Self] Update [Animator] from [Movement]", category: "Action", id: "732d59f17711035a641f7f3979d43d8b")]
public partial class UpdateMoveAnimatorAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<EntityAnimator> Animator;
    [SerializeReference] public BlackboardVariable<NavMovement> Movement;

    private readonly int _xMoveHash = UnityEngine.Animator.StringToHash("X_MOVE");
    private readonly int _zMoveHash = UnityEngine.Animator.StringToHash("Z_MOVE");
    
    protected override Status OnUpdate()
    {
        Transform trm = Self.Value.transform;
        Vector3 velocity = Movement.Value.Velocity;
        float xMove = Vector3.Dot(trm.right, velocity);
        float zMove = Vector3.Dot(trm.forward, velocity);
        
        Animator.Value.SetParam(_xMoveHash, xMove, 0.1f);
        Animator.Value.SetParam(_zMoveHash, zMove, 0.1f);
        
        return Status.Running;
    }

}

