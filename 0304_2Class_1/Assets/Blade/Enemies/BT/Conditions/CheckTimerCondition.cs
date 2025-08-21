using System;
using Unity.Behavior;
using UnityEngine;

namespace Blade.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CheckTimer", story: "Check [Timer] pass [Sec]", category: "Conditions", id: "8a948aec93e91f63793ff0726b934ca4")]
    public partial class CheckTimerCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<float> Timer;
        [SerializeReference] public BlackboardVariable<float> Sec;

        public override bool IsTrue()
        {
            return Timer + Sec < Time.time;
        }

    }
}
