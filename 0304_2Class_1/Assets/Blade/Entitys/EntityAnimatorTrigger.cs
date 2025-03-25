using System;
using UnityEngine;

namespace Blade.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationEndTrigger;
        public Action<bool> OnRollingStatusChange;
        
        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void AnimationEnd()
        {
            OnAnimationEndTrigger?.Invoke();
        }
        
        private void RollingStart() => OnRollingStatusChange?.Invoke(true);
        private void RollingEnd() => OnRollingStatusChange?.Invoke(false);
    }
}