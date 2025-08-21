using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.Test.ThreadTest
{
    public static class AwaitableExtension
    {
        public static Task<T> AsTask<T>(this Awaitable<T> awaitable)
        {
            return Task.Run(async () => await awaitable);
        }
    }
    
    public class AwaitableWait : MonoBehaviour
    {
        private List<Awaitable<int>> _tasks;

        private void Awake()
        {
            _tasks = new List<Awaitable<int>>();
        }

        private void Update()
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                TaskWait();
            }
        }

        private async void TaskWait()
        {
            for (int i = 0; i < 10; i++)
            {
                _tasks.Add(SomeAction(i));
            }

            int[] result = await Task.WhenAll(_tasks.Select(task => task.AsTask()));
        }

        private async Awaitable<int> SomeAction(int i)
        {
            await Awaitable.WaitForSecondsAsync(i);
            
            int tick = Environment.TickCount;
            
            return tick;
        }
    }
}