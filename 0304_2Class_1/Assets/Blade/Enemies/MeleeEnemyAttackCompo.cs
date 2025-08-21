using System;
using System.Linq;
using Blade.Combat;
using Blade.Entities;
using Chuh007Lib.StatSystem;
using UnityEngine;

namespace Blade.Enemies
{
    public class MeleeEnemyAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        private Entity _entity;
        private DamageCalcCompo _damageCompo;
        private EntityStat _statCompo;
        private EntityAnimatorTrigger _animatorTrigger;

        [SerializeField] private AttackDataSO attackData;
        [SerializeField] private StatSO physicalDamageStat;
        [SerializeField] private OverlapDamageCaster[] damageCaster; //무기마다 캐스터가 여러개일 수도 있어.

        private bool _isActive;

        private DamageData _currentDamageData;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
            _animatorTrigger = entity.GetCompo<EntityAnimatorTrigger>();
            
            _damageCompo = entity.GetCompo<DamageCalcCompo>();
            damageCaster = entity.GetComponentsInChildren<OverlapDamageCaster>(true);
            damageCaster.ToList().ForEach(caster => caster.InitCaster(entity));
        }
        
        public void AfterInitialize()
        {
            _animatorTrigger.OnDamageToggleTrigger += SetDamageCaster;
        }

        private void OnDestroy()
        {
            _animatorTrigger.OnDamageToggleTrigger -= SetDamageCaster;
        }

        public void SetDamageCaster(bool isActive)
        {
            _isActive = isActive;
            if (isActive)
            {
                foreach (OverlapDamageCaster caster in damageCaster)
                {
                    caster.StartCasting();
                }

                _currentDamageData = _damageCompo.CalculateDamage(
                    _statCompo.GetStat(physicalDamageStat), attackData);
            }
        }

        private void FixedUpdate()
        {
            if (_isActive)
            {
                foreach (OverlapDamageCaster caster in damageCaster)  
                {
                    caster.CastDamage(_currentDamageData, transform.position, transform.forward, attackData);
                }
            }
        }

        
    }
    
}