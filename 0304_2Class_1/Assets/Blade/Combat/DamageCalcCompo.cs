using System;
using Blade.Entities;
using Chuh007Lib.StatSystem;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Blade.Combat
{
    public class DamageCalcCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private StatSO criticalStat, criticalDamageStat;

        private EntityStat _statCompo;
        private float _critical, _criticalDamage;
        
        public void Initialize(Entity entity)
        {
            _statCompo = entity.GetCompo<EntityStat>();
        }

        public void AfterInitialize()
        {
            if (criticalStat is null)
                _critical = 0;
            else
            {
                _critical = _statCompo.SubscribeStat(criticalStat, HandleCriticalChange, 0f);
            }
            if (criticalDamageStat is null)
                _critical = 1;
            else
            {
                _criticalDamage = _statCompo.SubscribeStat(criticalDamageStat, HandleCriticalDamageChange, 1f);
            }
        }

        private void OnDestroy()
        {
            if(criticalStat is not null)
                _statCompo.UnSubscribeStat(criticalStat, HandleCriticalChange);
            if(criticalDamageStat is not null)
                _statCompo.UnSubscribeStat(criticalDamageStat, HandleCriticalDamageChange);
        }


        private void HandleCriticalChange(StatSO stat, float currentvalue, float prevvalue)
            => _critical = currentvalue;
        
        private void HandleCriticalDamageChange(StatSO stat, float currentvalue, float prevvalue)
            => _criticalDamage = currentvalue;

        public DamageData CalculateDamage(StatSO majorStat, AttackDataSO attackData, float multiplier = 1f)
        {
            DamageData data = new DamageData();
            data.damage = _statCompo.GetStat(majorStat).Value * attackData.damageMultiplier +
                          attackData.damageIncrease * multiplier;

            if (Random.value < _critical)
            {
                data.damage *= _criticalDamage;
                data.isCritical = true;
            }
            else
            {
                data.isCritical = false;
            }

            return data;
        }
    }
}