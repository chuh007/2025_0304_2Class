using Blade.Combat;
using Blade.Entities;
using Blade.SkillSystem;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerRollingState : PlayerState
    {
        private bool _isRolling;
        private Vector3 _rollingDirection;
        private SkillComponent _skillCompo;
        
        public PlayerRollingState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _skillCompo = entity.GetCompo<SkillComponent>();
        }

        public override void Enter()
        {
            base.Enter();
            MovementDataSO moveData = _skillCompo.GetSkill<RollingSkill>().MovementData;
            
            _movement.CanManualMovement = false;
            _isRolling = false;
            _rollingDirection = _player.transform.forward; //차후 마우스로 변경 가능
            
            _movement.ApplyMovementData(_rollingDirection, moveData);
        }

        public override void Exit()
        {
            _movement.StopImmediately();
            _movement.CanManualMovement = true;
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
            {
                _player.ChangeState("IDLE");
            }
        }
    }
}