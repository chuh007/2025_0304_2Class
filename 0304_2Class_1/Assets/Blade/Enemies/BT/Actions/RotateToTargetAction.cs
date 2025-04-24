using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "RotateToTarget", story: "[Self] rotate to [Target] in [Second]", category: "Enemy/Move", id: "8c9dca49df130773ce60f3ce853300f2")]
    public partial class RotateToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<float> Second;

        private float _startTime;
        protected override Status OnStart()
        {
            _startTime = Time.time;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            LookTargetSmoothly();
            if(Time.time - _startTime >= Second.Value)
                return Status.Success;
            return Status.Running;
        }

        private void LookTargetSmoothly()
        {
            const float rotationSpeed = 10f;
            Vector3 direction = Target.Value.position - Self.Value.transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            Quaternion rotation = Quaternion.Slerp(
                Self.Value.transform.rotation, 
                targetRotation, 
                Time.deltaTime * rotationSpeed);
            
            Self.Value.transform.rotation = rotation;
        }

    }
}