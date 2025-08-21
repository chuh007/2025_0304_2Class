using System;
using Blade.Combat;
using Blade.Core;
using Blade.Entities;
using Blade.Events;
using Blade.FSM;
using Blade.SkillSystem;
using Chuh007Lib.Dependencies;
using UnityEngine;

namespace Blade.Players
{
    
    public class Player : Entity, IDependencyProvider, IKnockBackable
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [field: SerializeField] public GameEventChannelSO PlayerChannel { get; private set; }

        [SerializeField] private StateDataSO[] states;
        
        private EntityStateMachine _stateMachine;

        [Provide]
        public Player ProvidePlayer() => this;

        private SkillComponent _skillComponent;
        
        #region Temp region

        public float rollingVelocity = 2.2f;
        private EntityActionData _actionData;
        private CharacterMovement _movement;
        
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);

            PlayerInput.OnRollingPressed += HandleRollingKeyPressed;
            _actionData = GetCompo<EntityActionData>();
            _movement = GetCompo<CharacterMovement>();
            _skillComponent = GetCompo<SkillComponent>();
            
            OnHitEvent.AddListener(HandleHitEvent);
            OnDeadEvent.AddListener(HandleDeadEvent);
        }

        private void OnDestroy()
        {
            PlayerInput.OnRollingPressed -= HandleRollingKeyPressed;
            OnHitEvent.RemoveListener(HandleHitEvent);
            OnDeadEvent.RemoveListener(HandleDeadEvent);
        }

        private void HandleDeadEvent()
        {
            if (IsDead) return;
            IsDead = true;
            PlayerChannel.RaiseEvent(PlayerEvents.PlayerDead);
            ChangeState("DEAD", true);
        }

        private void HandleHitEvent()
        {
            const string hit = "HIT";
            if (_actionData.HitByPowerAttack)
            {
                ChangeState(hit, true);
            }
        }

        private void HandleRollingKeyPressed()
        {
            RollingSkill skill = _skillComponent.GetSkill<RollingSkill>();
            if (skill.IsCooldown) return;
            
            skill.UseSkill();
            const string rolling = "ROLLING";
            ChangeState(rolling);
        }

        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }
        
        public void ChangeState(string newStateName, bool forced = false) 
            => _stateMachine.ChangeState(newStateName, forced);

        public void KnockBack(Vector3 force, float time)
        {
            _movement.KnockBack(force, time);
        }
    }
}