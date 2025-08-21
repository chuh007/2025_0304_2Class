using Blade.Entities;
using UnityEngine;

namespace Blade.Combat
{
    public abstract class DamageCaster : MonoBehaviour
    {
        [SerializeField] protected LayerMask whatIsTarget;

        protected Entity _owner;
        
        public virtual void InitCaster(Entity owner)
        {
            _owner = owner;
        }

        public virtual void ApplyDamageAndKnockBack(Transform target, DamageData damageData, Vector3 position,
            Vector3 normal, AttackDataSO attackData)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(damageData, position, normal, attackData, _owner);
            }

            if (target.TryGetComponent(out IKnockBackable knockBackable))
            {
                Vector3 force = transform.forward * attackData.knockForce;
                knockBackable.KnockBack(force, attackData.knockBackDuration);
            }
        }
        
        public abstract bool CastDamage(DamageData damageData, Vector3 position, Vector3 direction, AttackDataSO attackData);
    }
}