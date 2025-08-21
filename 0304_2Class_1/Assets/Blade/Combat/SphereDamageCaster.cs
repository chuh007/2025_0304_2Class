using System;
using UnityEngine;

namespace Blade.Combat
{
    public class SphereDamageCaster : DamageCaster
    {
        [SerializeField, Range(0.5f, 3f)] private float castRadius = 1f;
        [SerializeField, Range(0f, 1f)] private float castInterpolation = 1f;
        [SerializeField, Range(0f, 3f)] private float castRange = 1f;
        
        public override bool CastDamage(DamageData damageData,Vector3 position, Vector3 direction, AttackDataSO attackData)
        {
            Vector3 startPosition = position + direction * -castInterpolation * 2f; //- 붙어있음.
            bool isHit = Physics.SphereCast(startPosition,
                castRadius,
                transform.forward,
                out RaycastHit hit,
                castRange,
                whatIsTarget);
            if (isHit)
            {
                ApplyDamageAndKnockBack(hit.collider.transform, damageData, hit.point, hit.normal, attackData);
            }
            return isHit;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 startPosition = transform.position + transform.forward * -castInterpolation * 2f;
            Gizmos.color = Color.green;            
            Gizmos.DrawWireSphere(startPosition, castRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(startPosition + transform.forward * castRange, castRadius);
        }
#endif
    }
}