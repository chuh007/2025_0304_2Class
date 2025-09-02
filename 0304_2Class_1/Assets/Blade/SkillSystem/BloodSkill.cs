using Blade.Combat;
using Blade.Combat.Debuff;
using Blade.Effects;
using Blade.Entities;
using Chuh007Lib.Dependencies;
using Chuh007Lib.ObjectPool.RunTime;
using Chuh007Lib.StatSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Blade.SkillSystem
{
    public class BloodSkill : Skill
    {
        [SerializeField] private PoolItemSO countingCurseEffect;
        [SerializeField] private PoolItemSO nihilEffect;
        
        [SerializeField] private AttackDataSO skillAttackData;
        [SerializeField] private StatSO damageStat;
        [SerializeField] private Volume bloodVolume;
        [SerializeField] private float currentRadius = 5f;
        [SerializeField] private float damageMultiplier = 1.5f;
        [SerializeField] private float hpHealValue = 5f;

        [Inject] private PoolManagerMono _poolManager;
        
        private EntityAnimatorTrigger _trigger;
        private DamageCalcCompo _damageCompo;
        private EntityHealth _entityHealth; 
        
        private int _curseCount = 0;
        private int _callCnt = 0;
        public override void InitializeSkill(Entity owner, SkillComponent skillComponent)
        {
            base.InitializeSkill(owner, skillComponent);
            bloodVolume.weight = 0;
            _trigger = owner.GetCompo<EntityAnimatorTrigger>();
            _damageCompo = owner.GetCompo<DamageCalcCompo>();
            _entityHealth = owner.GetCompo<EntityHealth>();
            _trigger.OnAnimationEndTrigger += HandleAnimEnd;
        }
        
        public override void UseSkill()
        {
            base.UseSkill();
            _curseCount++;
            if (_curseCount > 3)
            {
                DOTween.To(() => bloodVolume.weight, x => bloodVolume.weight = x, 1f, 1f);
                _callCnt = 0;
                _trigger.OnCastSkillTrigger += HandleCastSkill;
                // 니힐
                _curseCount = 0;
            }
            else
            {
                _trigger.OnDamageToggleTrigger += HandleCastCurseSkill;
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    int enemyCount = _skillComponent.GetEnemiesInRange(transform.position, currentRadius);
            
                    for (int i = 0; i < enemyCount; ++i)
                    {
                        Collider target = _skillComponent.Colliders[i];
                        if (target.TryGetComponent(out IDebuffable debuffable))
                        {
                            if ((debuffable.CurrentDebuff & (int)DebuffType.Curse) == 0)
                                debuffable.PlusDebuff(DebuffType.Curse);
                            else if ((debuffable.CurrentDebuff & (int)DebuffType.DuetCurse) == 0)
                                debuffable.PlusDebuff(DebuffType.DuetCurse);
                            else debuffable.PlusDebuff(DebuffType.TrioCurse);
                        }
                    }
            
                    PoolingEffect effect = _poolManager.Pop<PoolingEffect>(countingCurseEffect);
                    effect.PlayVFX(transform.position + Vector3.up * 2f, Quaternion.identity);
                    DelayPooling(effect, 2f);
                });
                
            }
            
        }

        private void HandleCastCurseSkill(bool obj)
        {
            _trigger.OnAnimationEndTrigger.Invoke();
            _trigger.OnDamageToggleTrigger -= HandleCastCurseSkill;
        }

        private void HandleCastSkill()
        {
            _callCnt++;
            PoolingEffect effect = _poolManager.Pop<PoolingEffect>(nihilEffect);
            DamageData damageData = _damageCompo.CalculateDamage(damageStat, skillAttackData, damageMultiplier);
            int enemyCount = _skillComponent.GetEnemiesInRange(transform.position, currentRadius * 5f);
            for (int i = 0; i < enemyCount; ++i)
            {
                Collider target = _skillComponent.Colliders[i];
                if (target.TryGetComponent(out IDebuffable debuffable))
                {
                    if ((debuffable.CurrentDebuff & (int)DebuffType.Curse) != 0)
                        debuffable.MinusDebuff(DebuffType.Curse);
                    else if ((debuffable.CurrentDebuff & (int)DebuffType.DuetCurse) != 0)
                        debuffable.MinusDebuff(DebuffType.DuetCurse);
                    else if ((debuffable.CurrentDebuff & (int)DebuffType.TrioCurse) != 0)
                        debuffable.MinusDebuff(DebuffType.TrioCurse);
                    else continue;
                }
                if (target.TryGetComponent(out IDamageable damageable))
                {
                    Debug.Log(damageData.damage);
                    damageable.ApplyDamage(damageData, transform.transform.position,
                        transform.forward, skillAttackData, _owner);
                    _entityHealth.ApplyHeal(hpHealValue);
                }
            }
            effect.PlayVFX(transform.position + Vector3.up * 2f, Quaternion.identity);
            DelayPooling(effect, 2f);
        }
        
        private void HandleAnimEnd()
        {
            _trigger.OnCastSkillTrigger -= HandleCastSkill;
            DOTween.To(() => bloodVolume.weight, x => bloodVolume.weight = x, 0f, 1f);
        }
        
        private async void DelayPooling(PoolingEffect effect, float duration)
        {
            await Awaitable.WaitForSecondsAsync(duration);
            _poolManager.Push(effect);
        }
    }
}