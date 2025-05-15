using UnityEngine;

namespace Blade.Items
{
    [CreateAssetMenu(fileName = "DropTable", menuName = "SO/Item/DropTable", order = 0)]
    public class DropTableSO : ScriptableObject
    {
        public int dropExp;
        public int minDropGold;
        public int maxDropGold;
        
        public int GetRandomGoldAmount()
        {
            return Random.Range(minDropGold, maxDropGold + 1);
        }
    }
}