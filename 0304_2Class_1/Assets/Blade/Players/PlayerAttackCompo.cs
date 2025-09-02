using System;
using Blade.Combat;
using Blade.Entities;
using Chuh007Lib.StatSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Blade.Players
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [Header("Impulse Settings")]
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private bool canImpulseOnlyHit = true;
        
        [SerializeField] private AttackDataSO[] attackDataList;   //1
        [SerializeField] private float comboWindow = 0.7f;
        [SerializeField] private StatSO attackSpeedStat;
        [SerializeField] private StatSO meleeDamageStat;
        
        [SerializeField] private DamageCaster damageCaster;
        
        private Entity _entity;
        private EntityAnimator _entityAnimator;
        private EntityVFX _vfxCompo;
        private EntityAnimatorTrigger _animatorTrigger;
        private EntityStat _statCompo;
        private DamageCalcCompo _damageCompo;
        
        private readonly int _attackSpeedHash = Animator.StringToHash("ATTACK_SPEED");
        private readonly int _comboCounterHash = Animator.StringToHash("COMBO_COUNTER");

        private float _attackSpeed = 1f;
        private float _lastAttackTime;

        public bool useMouseDirection;
        public int ComboCounter { get; set; } = 0;
        public float AttackSpeed
        {
            get => _attackSpeed;
            set
            {
                _attackSpeed = value;
                _entityAnimator.SetParam(_attackSpeedHash, _attackSpeed);
            }
        }

        public AttackDataSO GetCurrentAttackData()  //2
        {
            Debug.Assert(attackDataList.Length > ComboCounter, "Combo counter is out of range");
            return attackDataList[ComboCounter];
        }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _entityAnimator = entity.GetCompo<EntityAnimator>();
            _vfxCompo = entity.GetCompo<EntityVFX>();
            _animatorTrigger = entity.GetCompo<EntityAnimatorTrigger>();
            _statCompo = entity.GetCompo<EntityStat>();
            _damageCompo = entity.GetCompo<DamageCalcCompo>();

            damageCaster.InitCaster(_entity); //오너 설정해주고 
        }
        
        public void AfterInitialize()
        {
            AttackSpeed = _statCompo.SubscribeStat(attackSpeedStat, HandleAttackSpeedChange, 1f);
            _animatorTrigger.OnAttackVFXTrigger += HandleAttackVFXTrigger;
            _animatorTrigger.OnDamageCastTrigger += HandleDamageCastTrigger;
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(attackSpeedStat, HandleAttackSpeedChange);
            _animatorTrigger.OnAttackVFXTrigger -= HandleAttackVFXTrigger; 
            _animatorTrigger.OnDamageCastTrigger -= HandleDamageCastTrigger;
        }

        private void HandleAttackSpeedChange(StatSO stat, float currentvalue, float prevvalue)
        {
            AttackSpeed = currentvalue;
        }

        private void HandleDamageCastTrigger()
        {
            AttackDataSO attackData = GetCurrentAttackData();
            DamageData data = _damageCompo.CalculateDamage(meleeDamageStat, attackData);
            Vector3 position = damageCaster.transform.position;
            bool isSuccess = damageCaster.CastDamage(data, position, _entity.transform.forward, attackData);
            
            if(attackData.isPowerAttack == false) return;
            
            if (canImpulseOnlyHit == false || isSuccess)
            {
                impulseSource.GenerateImpulse(attackData.impulseForce);
            }
        }

        private void HandleAttackVFXTrigger()  //4
        {
            _vfxCompo.PlayVFX($"Blade{ComboCounter}", Vector3.zero, Quaternion.identity);
        }

        public void Attack()
        {
            bool comboCounterOver = ComboCounter > 2;
            bool comboWindowExhaust = Time.time >= _lastAttackTime + comboWindow;
            if (comboCounterOver || comboWindowExhaust)
                ComboCounter = 0;
            
            _entityAnimator.SetParam(_comboCounterHash, ComboCounter);
        }

        public void EndAttack()
        {
            ComboCounter++;
            if(ComboCounter > 2) ComboCounter = 0;
            _lastAttackTime = Time.time;
        }
        
    }
}