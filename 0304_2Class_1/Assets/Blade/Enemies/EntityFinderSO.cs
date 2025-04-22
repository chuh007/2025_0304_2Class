using Blade.Entities;
using UnityEngine;

namespace Blade.Enemies
{
    [CreateAssetMenu(fileName = "EntityFinder", menuName = "SO/EntityFinder", order = 0)]
    public class EntityFinderSO : ScriptableObject
    {
        public Entity Target { get; private set; }

        public void SetTarget(Entity target)
        {
            Target = target;
        }
    }
}