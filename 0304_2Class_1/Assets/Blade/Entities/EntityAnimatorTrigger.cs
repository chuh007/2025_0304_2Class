using System;
using UnityEngine;

namespace Blade.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationEndTrigger;
        public Action<bool> OnRollingStatusChange;
        public Action OnAttackVFXTrigger;
        public Action<bool> OnManualRotationTrigger;
        
        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void AnimationEnd() //매서드 명 오타나면 안된다. (이벤트 이름과 동일하게 만들어야 해.)
        {
            OnAnimationEndTrigger?.Invoke();
        }

        private void RollingStart() => OnRollingStatusChange?.Invoke(true);
        private void RollingEnd() => OnRollingStatusChange?.Invoke(false);
        private void PlayAttackVFX() => OnAttackVFXTrigger?.Invoke();
        private void StartManualRotationTrigger() => OnManualRotationTrigger?.Invoke(true);
        private void StopManualRotationTrigger() => OnManualRotationTrigger?.Invoke(false);
        
    }
}