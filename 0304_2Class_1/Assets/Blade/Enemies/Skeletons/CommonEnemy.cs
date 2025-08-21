using Blade.Combat;
using Blade.Enemies.BT.Events;
using Blade.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Blade.Enemies.Skeletons
{
    public class CommonEnemy : Enemy, IKnockBackable
    {
        public UnityEvent<Vector3, float> OnKnockBack;
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
        
        public void KnockBack(Vector3 force, float time)
        {
            OnKnockBack?.Invoke(force, time);
        }

        protected override void HandlePlayerDead(PlayerDead obj)
        {
            var target = GetBlackboardVariable<Transform>("Target");
            target.Value = null;
            _stateChannel.SendEventMessage(EnemyState.IDLE);
        }
        
        public void HandleAnimationMove(Vector3 deltaPosition, Quaternion deltaRotation)
        {
            transform.position += deltaPosition;
            transform.rotation = deltaRotation * transform.rotation;
        }
    }
}