using Blade.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blade.Combat
{
    public class EntityHealth : MonoBehaviour,IEntityComponent, IDamageable
    {
        private Entity _entity;
        [SerializeField] private EntityActionData actionData;
        private IDamageable damageableImplementation;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        public void ApplyDamage(float damage, Vector3 hitPoint, Vector3 hitNomal, AttackDataSO attackData, Entity dealer)
        {
            actionData.HitPoint = hitPoint;
            actionData.HitNormal = hitNomal;
            // TODO 넉벡, 데미지

            _entity.OnHitEvent?.Invoke();
        }
    }
}