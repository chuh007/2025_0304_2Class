using System;
using Chuh007Lib.Dependencies;
using UnityEngine;

namespace Chuh007Lib.ObjectPool.RunTime
{
    [Provide]
    public class PoolManagerMono : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private PoolManagerSO poolManager;

        private void Awake()
        {
            poolManager.Initialize(transform);
        }

        public T Pop<T>(PoolItemSO poolItem) where T : IPoolable
        {
            return (T)poolManager.Pop(poolItem);
        }

        public void Push(IPoolable item)
        {
            poolManager.Push(item);
        }
    }
}