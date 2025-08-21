using Blade.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveAround", story: "[Self] Move around [Target]", category: "Action", id: "ad03fa72b229805ceac007134f2f92dc")]
public partial class MoveAroundAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> AroundDistance;
    [SerializeReference] public BlackboardVariable<float> DeltaDistance;

    private NavMovement _movement;
    private float _aroundDirection; // 왼쪽으로 돌지 오른쪽으로 돌지 결정하는 방향

    private Transform _selfTrm;
    private Transform _targetTrm;
    
    protected override Status OnStart()
    {
        if(Self.Value == null) return Status.Failure;
        _movement = Self.Value.GetCompo<NavMovement>();
        if(_movement == null) return Status.Failure;
        if(AroundDistance == 0 || DeltaDistance == 0) return Status.Failure;
        
        _aroundDirection = Random.value < 0.5f ? -1f : 1f; // 왼쪽으로 돌지 오른쪽으로 돌지 결정
        _selfTrm = Self.Value.transform;
        _targetTrm = Target.Value.transform;
        
        SetNextDestination();
        return Status.Running;
    }

    private void SetNextDestination()
    {
        float angle = _aroundDirection * Random.Range(25f, 35f);
        Vector3 direction = _selfTrm.position - _targetTrm.position;
        direction.y = 0;
        direction = Quaternion.Euler(0, angle, 0) * direction.normalized;
        
        Vector3 destination = _targetTrm.position + direction * AroundDistance;
        _movement.SetDestination(destination);
    }

    protected override Status OnUpdate()
    {
        if (_movement.RemainDistance < 0.5f)
        {
            SetNextDestination();
        }
        
        float distance = Vector3.Distance(_selfTrm.position, _targetTrm.position);
        if (distance > AroundDistance + DeltaDistance)
        {
            return Status.Failure;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

