using Blade.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsinDetect", story: "[Target] is in [Self] detectRange", category: "Enemy/Condition", id: "864d29c415c881a6460e5d09268b07c3")]
public partial class IsinDetectCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<Enemy> Self;

    public override bool IsTrue()
    {
        float distance = Vector3.Distance(Target.Value.position, Self.Value.transform.position);
        return distance < Self.Value.detectRange;
    }
}
