using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Patrol", story: "[Self] Patrol with [points]", category: "Action/Enemy", id: "30660e141d57e35941c80571aa989a2b")]
    public partial class PatrolAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<WayPoints> Points;

        private int _currentPointIdx;
        private NavMovement _navMovement;
        
        protected override Status OnStart()
        {
            Initialize();
            _navMovement.SetDestination(Points.Value[_currentPointIdx]);
            return Status.Running;
        }

        private void Initialize()
        {
            if (_navMovement == null)
                _navMovement =  Self.Value.GetCompo<NavMovement>();
        }

        protected override Status OnUpdate()
        {
            if(_navMovement.IsArrived)
                return Status.Success;
            return Status.Running;
        }

        protected override void OnEnd()
        {
            _currentPointIdx = (_currentPointIdx + 1) % Points.Value.Length;
        }
    }
}

