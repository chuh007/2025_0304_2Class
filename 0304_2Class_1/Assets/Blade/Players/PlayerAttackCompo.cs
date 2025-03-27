using System;
using Blade.Combat;
using Blade.Entities;
using UnityEngine;

namespace Blade.Players
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private AttackDataSO[] attackDataList;   //1
        [SerializeField] private float comboWindow = 0.7f;
        private Entity _entity;
        private EntityAnimator _entityAnimator;
        private EntityVFX _vfxCompo;
        private EntityAnimatorTrigger _animatorTrigger;
        
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
            AttackSpeed = 1f;

            _animatorTrigger.OnAttackVFXTrigger += HandleAttackVFXTrigger;
        }

        private void OnDestroy()
        {
            _animatorTrigger.OnAttackVFXTrigger += HandleAttackVFXTrigger;
        }

        private void HandleAttackVFXTrigger()
        {
            _vfxCompo.PlayVFX($"Blade{ComboCounter + 1}", Vector3.zero, Quaternion.identity);
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
            _lastAttackTime = Time.time;
        }

    }
}