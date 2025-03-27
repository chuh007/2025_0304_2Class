using Blade.Entities;
using Blade.FSM;

namespace TempCode
{
    public class PlayerIdleState : EntityState
    {
        private Player _player;
        public PlayerIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnAttackPressed += HandleAttackPressed;
        }

        public override void Exit()
        {
            _player.PlayerInput.OnAttackPressed -= HandleAttackPressed;
            base.Exit();
        }

        private void HandleAttackPressed()
        {
            _player.Destination = _player.PlayerInput.GetWorldPosition();
            _player.stateMachine.ChangeState("MOVE");
        }
    }
}