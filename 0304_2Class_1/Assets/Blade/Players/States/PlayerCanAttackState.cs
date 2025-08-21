using Blade.Entities;
using Blade.SkillSystem;
using UnityEngine;

namespace Blade.Players.States
{
    public abstract class PlayerCanAttackState : PlayerState
    {
        private SkillComponent _skillComponent;
        public PlayerCanAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _skillComponent = entity.GetCompo<SkillComponent>();
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnAttackPressed += HandleAttackPressed;
            _player.PlayerInput.OnSkillPressed += HandleSkillPressed;
        }

        public override void Exit()
        {
            _player.PlayerInput.OnAttackPressed -= HandleAttackPressed;
            _player.PlayerInput.OnSkillPressed -= HandleSkillPressed;
            base.Exit();
        }

        private void HandleSkillPressed(bool isPressed)
        {
            Skill targetSkill = _skillComponent.CurrentSkill;
            if (targetSkill == null || targetSkill.IsCooldown) return;

            if (isPressed && targetSkill is IChargeable { IsCharging: false } chargeable)
            {
                chargeable.StartCharging();
                _player.ChangeState("CHARGING");
            }
        }

        private void HandleAttackPressed()
        {
            _player.ChangeState("ATTACK");
        }
    }
}