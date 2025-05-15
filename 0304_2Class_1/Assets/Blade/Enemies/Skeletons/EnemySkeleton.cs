using Blade.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Blade.Enemies.Skeletons
{
    public class EnemySkeleton : Enemy, IKnockBackable
    {
        public UnityEvent<Vector3, float> OnKnockBack;

        
        public void KnockBack(Vector3 force, float time)
        {
            OnKnockBack?.Invoke(force, time);
        }
    }
}