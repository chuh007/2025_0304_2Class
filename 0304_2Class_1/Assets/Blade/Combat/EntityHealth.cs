using System;
using Blade.Entities;
using Chuh007Lib.StatSystem;
using UnityEngine;

namespace Blade.Combat
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IDamageable, IAfterInitialize
    {
        private Entity _entity;
        private EntityActionData _actionData;
        private EntityStat _statCompo;

        [SerializeField] private StatSO hpStat;
        [SerializeField] private float maxHealth;
        [field: SerializeField] public float CurrentHealth { get; private set; }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _actionData = entity.GetCompo<EntityActionData>();
            _statCompo = entity.GetCompo<EntityStat>();
        }
        
        public void AfterInitialize()
        {
            CurrentHealth = maxHealth = _statCompo.SubscribeStat(hpStat, HandleMaxHPChange, 10f);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(hpStat, HandleMaxHPChange);
        }

        private void HandleMaxHPChange(StatSO stat, float currentvalue, float prevvalue)
        {
            float changed = currentvalue - prevvalue;
            maxHealth = currentvalue;
            if (changed > 0)
            {
                CurrentHealth = Mathf.Clamp(CurrentHealth + changed, 0, maxHealth);
            }
            else
            {
                CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
            }
        }

        public void ApplyDamage(DamageData damageData, Vector3 hitPoint, Vector3 hitNormal, AttackDataSO attackData, Entity dealer)
        {
            _actionData.HitPoint = hitPoint;
            _actionData.HitNormal = hitNormal;
            _actionData.HitByPowerAttack = attackData.isPowerAttack;
            _actionData.LastDamageData = damageData;
            //넉백은 나중에 처리한다.
            //데미지도 나중에 처리한다.
            
            CurrentHealth = Mathf.Clamp(CurrentHealth - damageData.damage, 0, maxHealth);
            if (CurrentHealth <= 0)
            {
                _entity.OnDeadEvent?.Invoke();
            }

            _entity.OnHitEvent?.Invoke(); //아직 없다. 만들러 가야해.
        }

        public void ApplyHeal(float value)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, maxHealth);
        }
    }
}