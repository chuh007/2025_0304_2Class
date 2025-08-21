using Blade.Entities;
using Blade.SkillSystem;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerChargingState : PlayerState
    {
        private readonly int _chargingTrigger = Animator.StringToHash("CHARGING_END");

        private CharacterMovement _movement;
        private SkillComponent _skillComponent;
        private IChargeable _targetSkill;
        private bool _isReleased;
        
        public PlayerChargingState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _movement = entity.GetCompo<CharacterMovement>();
            _skillComponent = entity.GetCompo<SkillComponent>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
            _targetSkill = _skillComponent.CurrentSkill as IChargeable;
            Debug.Assert(_targetSkill != null, "TargetSkill is null");

            _player.PlayerInput.OnSkillPressed += HandleSkillReleased;
            _isReleased = false;
        }

        public override void Exit()
        {
            _player.PlayerInput.OnSkillPressed -= HandleSkillReleased;
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            
            Vector3 mousePosition = _player.PlayerInput.GetWorldPosition();
            _player.RotateToTarget(mousePosition);
            
            if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }

        private void HandleSkillReleased(bool isPressed)
        {
            if(isPressed || _isReleased) return;

            _isReleased = true;
            _targetSkill.ReleaseCharging();
            _entityAnimator.SetParam(_chargingTrigger);
        }
    }
}