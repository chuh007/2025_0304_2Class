using Blade.Entities;
using UnityEngine;

namespace Blade.Combat
{
    public interface IDamageable
    {
        public void ApplyDamage(DamageData damageData, Vector3 hitPoint, Vector3 hitNomal, AttackDataSO attackData,
            Entity dealer);
    }
}