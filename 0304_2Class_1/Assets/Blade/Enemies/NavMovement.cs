using System;
using Blade.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Blade.Enemies
{
    public class NavMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float stopOffset = 0.05f;
        [SerializeField] private float rotationSpeed = 10f;
        
        private Entity _entity;

        public bool IsArrived => !agent.pathPending
                                 && agent.remainingDistance < agent.stoppingDistance + stopOffset;
        
        public float RemainDistance => agent.pathPending ? -1 : agent.remainingDistance;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            agent.speed = moveSpeed;
        }

        private void Update()
        {
            if (agent.hasPath && agent.isStopped == false && agent.path.corners.Length > 0)
            {
                LookAtTarget(agent.steeringTarget, true);
            }
        }
        
        /// <summary>
        /// 지정한 Target위치로 회전하는 함수
        /// </summary>
        /// <param name="target">Vector3 - 바라볼 위치</param>
        /// <param name="isSmooth">boolean - Lerp 적용여부</param>
        public void LookAtTarget(Vector3 target, bool isSmooth = true)
        {
            Vector3 direction = target - _entity.transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
            if (isSmooth)
            {
                _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation, lookRotation,
                    Time.deltaTime * rotationSpeed);
            }
            else
            {
                _entity.transform.rotation = lookRotation;
            }
        }

        public void SetStop(bool isStop) => agent.isStopped = isStop;
        public void SetVelocity(Vector3 velocity) => agent.velocity = velocity;
        public void SetSpeed(float speed) => agent.speed = speed;
        public void SetDestination(Vector3 destination) => agent.SetDestination(destination);
    }
}