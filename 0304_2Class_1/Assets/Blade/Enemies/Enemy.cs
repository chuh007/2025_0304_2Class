using Blade.Entities;
using Unity.Behavior;
using UnityEngine;

namespace Blade.Enemies
{
    public abstract class Enemy : Entity
    {
        [field: SerializeField] public EntityFinderSO PlayerFinder { get; set; }
        public BehaviorGraphAgent BTAgent { get; private set; }

        #region Temp

        public float detectRange = 8f;
        public float attackRange = 2f;

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position,attackRange);
        }
        
        protected override void AddComponents()
        {
            base.AddComponents();
            BTAgent = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(BTAgent != null, $"{gameObject.name} does not have behavior graph agent");
        }

        public BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if (BTAgent.GetVariable(key, out BlackboardVariable<T> result))
            {
                return result;
            }
            return default;
        }
    }
}