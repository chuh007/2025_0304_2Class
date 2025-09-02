using Blade.Entities;
using Blade.SkillSystem;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerSkillState : PlayerState
    {
        private SkillComponent _skillComponent;
        
        public PlayerSkillState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _skillComponent = entity.GetCompo<SkillComponent>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
            Debug.Assert(_skillComponent != null && _skillComponent.CurrentSkill != null,
                "CurrentSkill is null but you are in skill state");
            
            
            _skillComponent.CurrentSkill.UseSkill();
        }

        public override void Update()
        {
            base.Update();
            
            if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }
    }
}