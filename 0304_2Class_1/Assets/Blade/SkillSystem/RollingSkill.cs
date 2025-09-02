using Blade.Combat;
using Blade.Core;
using Blade.Events;
using Chuh007Lib.ObjectPool.RunTime;
using UnityEngine;

namespace Blade.SkillSystem
{
    public class RollingSkill : Skill
    {
        [field: SerializeField] public MovementDataSO MovementData { get; private set; }
        [field: SerializeField] public GameEventChannelSO EffectChannel { get; private set; }
        [SerializeField] private PoolItemSO rollingEffect;

        public override void UseSkill()
        {
            base.UseSkill();

            PlayPoolEffect effectEvt = EffectEvents.PlayPoolEffect.Initializer(
                _owner.transform.position + new Vector3(0, 1f), _owner.transform.rotation, rollingEffect, 2f);
            EffectChannel.RaiseEvent(effectEvt);
        }
    }
}