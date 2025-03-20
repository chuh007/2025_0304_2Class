using Blade.Combat;
using Blade.Entities;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerAttackState : PlayerState
    {
        private PlayerAttackCompo _attackCompo;
        
        public PlayerAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
        } 

        public override void Enter()
        {
            base.Enter();
            _attackCompo.Attack();
            _movement.CanManualMovement = false;

            ApplyAttackData();
        }

        private void ApplyAttackData()
        {
            AttackDataSO currentAttackData = _attackCompo.GetCurrentAttackData();
            Vector3 playerDirection = GetPlayerDirection();
            _player.transform.rotation = Quaternion.LookRotation(playerDirection);

            Vector3 movement = playerDirection * currentAttackData.movementPower;
            _movement.SetAutoMovement(movement);
        }

        private Vector3 GetPlayerDirection()
        {
            if (_attackCompo.useMouseDirection == false)
                return _player.transform.forward;
            
            
            Vector3 targetPosition = _player.PlayerInput.GetWorldPosition();
            Vector3 direction = targetPosition - _player.transform.position;
            direction.y = 0;
            return direction.normalized;
        }
        
        public override void Exit()
        {
            _attackCompo.EndAttack();
            _movement.CanManualMovement = true;
            _movement.StopImmediately();
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }
    }
}