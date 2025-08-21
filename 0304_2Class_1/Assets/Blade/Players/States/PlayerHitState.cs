using Blade.Entities;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerHitState : PlayerState
    {
        private EntityActionData _actionData;
        
        public PlayerHitState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _actionData = entity.GetCompo<EntityActionData>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.CanManualMovement = false;
            Vector3 lookDirection = _actionData.HitNormal;
            lookDirection.y = 0;
            _entityAnimator.transform.rotation = Quaternion.LookRotation(lookDirection.normalized);
        }

        public override void Update()
        {
            base.Update();
            if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            _movement.CanManualMovement = true;
            base.Exit();
        }
    }
}