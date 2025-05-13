using Blade.Effects;
using Blade.Entities;
using Chuh007Lib.Dependencies;
using Chuh007Lib.ObjectPool.RunTime;
using DG.Tweening;
using UnityEngine;

namespace Blade.Feedbacks
{
    public class HitImpactFeedback : Feedback
    {
        [SerializeField] private PoolItemSO hitImpact;
        [SerializeField] private float playDuration;
        [SerializeField] private EntityActionData actionData;

        [Inject] private PoolManagerMono _poolManager;

        private PoolingEffect _effect;
        
        public override async void CreateFeedback()
        {
            _effect = _poolManager.Pop<PoolingEffect>(hitImpact);
            Quaternion rotation = Quaternion.LookRotation(actionData.HitNormal * -1);
            
            _effect.PlayVFX(actionData.HitPoint, rotation);
            
            await Awaitable.WaitForSecondsAsync(playDuration);
            StopFeedback();
            // DOVirtual.DelayedCall(playDuration, StopFeedback);
        }

        public override void StopFeedback()
        {
            if (_effect != null)
            {
                _poolManager.Push(_effect);
            }
        }
    }
}