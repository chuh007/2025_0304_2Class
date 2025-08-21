using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetTimer", story: "Set time to [Timer] if reset", category: "Action", id: "df5bbaaaa994ac6fee68e7481a62cadb")]
    public partial class SetTimerAction : Action
    {
        [SerializeReference] public BlackboardVariable<float> Timer;

        protected override Status OnStart()
        {
            Timer.Value = Time.time;
            return Status.Success;
        }
    }
}

