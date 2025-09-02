using Blade.Combat;
using Blade.Enemies.BT.Events;
using Blade.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Blade.Enemies
{
    public class EnemyGoblin : Enemy, IKnockBackable
    {
        public UnityEvent<Vector3, MovementDataSO> OnKnockBack;
        private StateChange _stateChannel;
        
        protected override void Start()
        {
            base.Start();
            OnDeadEvent.AddListener(HandleDeathEvent);
            _stateChannel = GetBlackboardVariable<StateChange>("StateChannel").Value;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnDeadEvent.RemoveListener(HandleDeathEvent);
        }

        private void HandleDeathEvent()
        {
            if(IsDead) return;
            IsDead = true;
            _stateChannel.SendEventMessage(EnemyState.DEAD);
        }
        
        protected override void HandlePlayerDead(PlayerDead obj)
        {
            var target = GetBlackboardVariable<Transform>("Target");
            target.Value = null;
            _stateChannel.SendEventMessage(EnemyState.IDLE);
        }

        public void KnockBack(Vector3 direction, MovementDataSO movementData)
        {
            OnKnockBack?.Invoke(direction, movementData);
        }

        public void HandleChildAnimatorMove(Vector3 deltaPosition, Quaternion deltaRotation)
        {
            transform.position += deltaPosition;
            transform.rotation = deltaRotation * transform.rotation;
        }
    }
}