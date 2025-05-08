using System;
using Unity.Behavior;
using UnityEngine;

namespace Blade.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "IsInAttack", story: "[Target] is in [Self] attackRange", category: "Enemy/Conditions", id: "ad4efdfe033b1b66059033c8252e39f0")]
    public partial class IsInAttackCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<Enemy> Self;

        public override bool IsTrue()
        {
            float distance = Vector3.Distance(Target.Value.position, Self.Value.transform.position);
            return distance < Self.Value.attackRange;
        }
    }
}
