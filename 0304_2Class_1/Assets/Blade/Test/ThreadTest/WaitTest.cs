using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.Test.ThreadTest
{
    public class WaitTest : MonoBehaviour
    {
        private List<Task<int>> _tasks;

        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                ABC();
            }
        }

        private async void ABC()
        {
            _tasks = new List<Task<int>>();

            for (int i = 0; i < 10; i++)
            {
                _tasks.Add(Task<int>.Factory.StartNew(SomeAction, i));
            }
            
            var result = await Task.WhenAll(_tasks.ToArray());

            long total = result.Aggregate(0L, (total, i) => total + i);
            
            Debug.Log(total);
        }
        
        private int SomeAction(object index)
        {
            int intValue = (int)index;
            Thread.Sleep(intValue * 1000);
            
            int tickCnt = Environment.TickCount;

            Debug.Log($"Thread id {Thread.CurrentThread.ManagedThreadId}, Task id : {Task.CurrentId}" +
                      $"value : {intValue}, tick : {tickCnt}");
            return tickCnt;
        }
    }
}