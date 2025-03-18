using Blade.Entities;
using Blade.FSM;
using System;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerMoveState : PlayerCanAttackState
    {
        
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnMovementChange += HandleMovementChange;
        }
        public override void Exit()
        {
            _player.PlayerInput.OnMovementChange -= HandleMovementChange;
            base.Exit();
        }

        private void HandleMovementChange(Vector2 movementKey)
        {
            _movement.SetMovementDirection(movementKey);
            if (movementKey.magnitude < _inputThreshold)
                _player.ChangeState("IDLE");
        }
    }
}

