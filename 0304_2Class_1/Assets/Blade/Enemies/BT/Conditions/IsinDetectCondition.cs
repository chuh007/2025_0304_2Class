using System;
using Unity.Behavior;
using UnityEngine;

namespace Blade.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "IsInDetect", story: "[Target] is in [Self] detectRange", category: "Enemy/Conditions", id: "797985c50de7237260c7d02aaebddb5d")]
    public partial class IsInDetectCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<Enemy> Self;

        public override bool IsTrue()
        {
            float distance = Vector3.Distance(Target.Value.position, Self.Value.transform.position);
            return distance < Self.Value.detectRange; //탐지 거리내에 있는가?
        }
    }
}
