using Blade.Combat;
using Blade.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blade.Players
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent
    {
        [FormerlySerializedAs("attackDatas")] [SerializeField] private AttackDataSO[] attackDataList;
        [SerializeField] private float comboWindow = 0.7f;
        private Entity _entity;
        private EntityAnimator _entityAnimator;

        private readonly int _attackSpeedHash = Animator.StringToHash("ATTACK_SPEED");
        private readonly int _comboCounterHash = Animator.StringToHash("COMBO_COUNTER");

        private float _attackSpeed = 1f;
        private float _lastAttackTime;

        public bool useMouseDirection;
        
        public int ComboCounter { get; set; } = 0;

        public AttackDataSO GetCurrentAttackData()
        {
            Debug.Assert(attackDataList.Length>ComboCounter, "Comdo counter is out if range");
            return attackDataList[ComboCounter];
        }
        
        public float AttackSpeed
        {
            get => _attackSpeed;
            set
            {
                _attackSpeed = value;
                _entityAnimator.SetParam(_attackSpeedHash, _attackSpeed);
            }
        }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _entityAnimator = entity.GetCompo<EntityAnimator>();
            AttackSpeed = 1f;
        }

        public void Attack()
        {
            bool comboCounterOver = ComboCounter > 2;
            bool comboWindowExhaust = Time.time > _lastAttackTime + comboWindow;
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