using Blade.Effects;
using Blade.Entities;
using Chuh007Lib.Dependencies;
using Chuh007Lib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blade.Feedbacks
{
    public class HitSlashFeedback : Feedback
    {
        [SerializeField] private PoolItemSO slashEffect;
        [SerializeField] private float playDuration;
        [SerializeField] private EntityActionData actionData;
        
        [Inject] private PoolManagerMono _poolManager;
        
        public override async void CreateFeedback()
        {
            PoolingEffect effect = _poolManager.Pop<PoolingEffect>(slashEffect);
            
            effect.PlayVFX(actionData.HitPoint, Quaternion.identity);
            
            await Awaitable.WaitForSecondsAsync(playDuration);
            _poolManager.Push(effect);
        }

        public override void StopFeedback()
        {
            
        }
    }
}