using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.Test.ThreadTest
{
    public class UniTaskTest : MonoBehaviour
    {
        [SerializeField] private RectTransform barRect1;
        [SerializeField] private RectTransform barRect2;

        private CancellationTokenSource _cts;

        private void Start()
        {
            SetXScale(barRect1,0);
            SetXScale(barRect2,0);
            _cts = new CancellationTokenSource();
        }

        private void SetXScale(RectTransform targetRect, float xValue)
        {
            Vector3 scale = targetRect.localScale;
            scale.x = xValue;
            targetRect.localScale = scale;
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                StartDualBar();
            }
        }

        private async void StartDualBar()
        {
            using (var cts = new CancellationTokenSource())
            {
                await UniTask.WhenAll(
                    BarFillWithUniTask(barRect1, cts.Token),
                    BarFillWithCoroutine(barRect2).WithCancellation(cts.Token)
                );
            }
        }

        private async UniTask BarFillWithUniTask(RectTransform bar, CancellationToken token)
        {
            for (int i = 0; i <= 100; i++)
            {
                destroyCancellationToken.ThrowIfCancellationRequested();
                await UniTask.Delay(50, cancellationToken: token);
                
                SetXScale(bar, i / 100f);
            }
        }

        public async void Test()
        {
            
        }
        
        private IEnumerator BarFillWithCoroutine(RectTransform bar)
        {
            for (int i = 0; i <= 100; i++)
            {
                yield return new WaitForSecondsRealtime(0.05f);
                SetXScale(barRect1, i / 100f);
            }
        }
    }
}