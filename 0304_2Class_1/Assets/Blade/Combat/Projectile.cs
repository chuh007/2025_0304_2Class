using System;
using Assets.Bocch16Lib.ObjectPool.RunTime;
using Blade.Entities;
using Chuh007Lib.ObjectPool.RunTime;
using UnityEngine;

namespace Blade.Combat
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private AttackDataSO attackData;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private PoolItemSO impactEffect;
        
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        private Entity _owner;
        private Pool _pool;
        private DamageData _damageData;

        public void SetUpProjectile(Entity entity, DamageData damageData, Vector3 position, Quaternion rotation,
            Vector3 velocity)
        {
            _owner = entity;
            _damageData = damageData;
            transform.SetPositionAndRotation(position, rotation);
            rigidbody.linearVelocity = velocity;
            
            damageCaster.InitCaster(_owner);
        }

        private void OnTriggerEnter(Collider other)
        {
            bool isHit = damageCaster.CastDamage(_damageData, transform.position, transform.forward, attackData);
            
            _pool.Push(this);
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }
                
        public void ResetItem()
        {
        }

    }
}