using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.Test.ThreadTest
{
    public class ThreadMono : MonoBehaviour
    {
        private int number = 0;
        
        private async void Start()
        {
            WorkSequence();
            
            Destroy(gameObject);
        }

        private async void WorkSequence()
        {
            Debug.Log("work 1 start");
            await Awaitable.NextFrameAsync();
            Debug.Log("work 2 start");
            await Awaitable.EndOfFrameAsync();
            await Awaitable.FixedUpdateAsync();
            await Awaitable.WaitForSecondsAsync(3f);
            Debug.Log("work end");
        }
        
        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                Debug.Log("밍");
            }
        }

        private void WorkJob(int milliSec)
        {
            Debug.Log($"Work start");
            while (true)
            {
                if(destroyCancellationToken.IsCancellationRequested)
                    destroyCancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}