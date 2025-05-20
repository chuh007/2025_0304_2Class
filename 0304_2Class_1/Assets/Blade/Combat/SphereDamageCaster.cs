using System;
using UnityEngine;

namespace Blade.Combat
{
    public class SphereDamageCaster : DamageCaster
    {
        [SerializeField, Range(0.5f, 3f)] private float castRadius = 1f;
        [SerializeField, Range(0f, 1f)] private float castInterpolation = 1f;
        [SerializeField, Range(0f, 3f)] private float castRange = 1f;
        
        public override void CastDamage(DamageData damageData,Vector3 position, Vector3 direction, AttackDataSO attackData)
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
                if (hit.collider.TryGetComponent(out IDamageable idamageable))
                {
                    float damage = 5f; //차후 스탯 시스템과 연동한다.
                    idamageable.ApplyDamage(damageData, hit.point, hit.normal, attackData, _owner);
                    Debug.Log($"<color=red>Hit!</color> {hit.collider.name} : {damage}");
                }

                if (hit.collider.TryGetComponent(out IKnockBackable knockBackable))
                {
                    Vector3 force = transform.forward * attackData.knockForce;
                    knockBackable.KnockBack(force, attackData.knockBackDuration);
                }
            }

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