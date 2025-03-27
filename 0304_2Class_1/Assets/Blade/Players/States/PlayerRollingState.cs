using Blade.Entities;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerRollingState : PlayerState
    {
        private bool _isRolling;
        private Vector3 _rollingDirection;
        
        public PlayerRollingState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _movement.CanManualMovement = false;
            _isRolling = false;
            _rollingDirection = _player.transform.forward; //차후 마우스로 변경 가능

            _animatorTrigger.OnRollingStatusChange += HandleRollingStatusChange;
        }

        public override void Exit()
        {
            _movement.StopImmediately();
            _movement.CanManualMovement = true;
            _animatorTrigger.OnRollingStatusChange -= HandleRollingStatusChange;
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

        private void HandleRollingStatusChange(bool isRolling)
        {
            float velocity = isRolling ? _player.rollingVelocity : _player.rollingVelocity * 0.2f;
            _movement.SetAutoMovement(_rollingDirection * velocity);
            
            _isRolling = isRolling;
        }
    }
}