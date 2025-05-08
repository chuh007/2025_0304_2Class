using Assets.Bocch16Lib.ObjectPool.RunTime;
using UnityEngine;

namespace Chuh007Lib.ObjectPool.RunTime
{
    public interface IPoolable
    {
        public PoolItemSO PoolItem { get; }
        public GameObject GameObject { get; }
        public void ResetItem();
        public void SetUpPool(Pool pool);
    }
}

