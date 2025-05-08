using UnityEngine;

namespace Bocch16Lib.ObjectPool.RunTime
{
    [CreateAssetMenu(fileName = "PoolItem", menuName = "SO/Pool/Item")]
    public class PoolItemSO : ScriptableObject
    {
        public string poolingName;
        public GameObject prefab;
        public int initCount;
    }
}

