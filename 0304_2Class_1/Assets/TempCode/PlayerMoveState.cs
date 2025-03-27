using Blade.Entities;
using Blade.FSM;
using Blade.Players;
using UnityEngine;

namespace TempCode
{
    public class PlayerMoveState : EntityState
    {
        private Player _player;
        private CharacterMovement _movement;
        
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
            _movement = entity.GetCompo<CharacterMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            Vector3 moveDirection = _player.Destination - _player.transform.position;
            moveDirection.y = 0;
            _movement.SetMovementDirection(new Vector2(moveDirection.x, moveDirection.z).normalized);
        }

        public override void Update()
        {
            base.Update();
            float distance = Vector3.Distance(_player.Destination, _player.transform.position);

            if (distance < 0.6f)
            {
                _player.stateMachine.ChangeState("IDLE");
            }
        }
    }
}