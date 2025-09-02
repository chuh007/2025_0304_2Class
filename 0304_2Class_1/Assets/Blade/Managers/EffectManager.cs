using System;
using Blade.Core;
using Blade.Effects;
using Blade.Events;
using Chuh007Lib.Dependencies;
using Chuh007Lib.ObjectPool.RunTime;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Blade.Managers
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO effectChannel;
        [Inject] private PoolManagerMono _poolManager;

        private void Awake()
        {
            effectChannel.AddListener<PlayPoolEffect>(HandlePlayPoolEffect);
        }

        private void OnDestroy()
        {
            effectChannel.RemoveListener<PlayPoolEffect>(HandlePlayPoolEffect);
        }

        private async void HandlePlayPoolEffect(PlayPoolEffect evt)
        {
            PoolingEffect effect = _poolManager.Pop<PoolingEffect>(evt.targetItem);
            effect.PlayVFX(evt.position, evt.rotation);
            await UniTask.WaitForSeconds(evt.duration);
            _poolManager.Push(effect);
        }
    }
}