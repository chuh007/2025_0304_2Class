using System;
using Blade.Combat;
using Blade.Entities;
using Chuh007Lib.StatSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Blade.Enemies
{
    public class NavMovement : MonoBehaviour, IEntityComponent, IKnockBackable, IAfterInitialize
    {
        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float stopOffset = 0.05f;
        [SerializeField] private float rotationSpeed = 10f;
        
        private Entity _entity;
        private EntityStat _statCompo;
        
        public bool IsArrived => !agent.pathPending 
                                 && agent.remainingDistance <= agent.stoppingDistance + stopOffset;

        public float RemainDistance => agent.pathPending ? -1 : agent.remainingDistance;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
        }

        
        public void AfterInitialize()
        {
            agent.speed = _statCompo.SubscribeStat(moveSpeedStat, HandleMoveSpeedChange, 1f);
        }

        private void HandleMoveSpeedChange(StatSO stat, float currentvalue, float prevvalue)
        {
            agent.speed = currentvalue;
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(moveSpeedStat, HandleMoveSpeedChange);
            _entity.transform.DOKill();
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
        public void KnockBack(Vector3 force, float time)
        {
            SetStop(true);
            Vector3 destination = GetKnockBackEndPosition(force);
            Vector3 delta = destination - _entity.transform.position;
            float knockBackDuration = delta.magnitude * time / force.magnitude;
            // force : time = delta : ?
            
            _entity.transform.DOMove(destination, knockBackDuration)
                .SetEase(Ease.OutCirc)
                .OnComplete(() =>
                {
                    agent.Warp(transform.position); // 에이전트를 내 현제 transform 기준으로 이동시킨다.
                    SetStop(false); // 네비게이션을 가동하도록 한다.
                });
            
        }

        /// <summary>
        /// force를 주면 어디까지 밀려날 수 있는지를 계산해주는 메서드
        /// </summary>
        /// <param name="force"></param>
        /// <returns></returns>
        private Vector3 GetKnockBackEndPosition(Vector3 force)
        {
            Vector3 startPosition = _entity.transform.position + new Vector3(0, 0, 0.5f);
            if (Physics.Raycast(startPosition, force.normalized, out RaycastHit hit, force.magnitude))
            {
                Vector3 hitPoint = hit.point;
                hitPoint.y = _entity.transform.position.y;
                return hitPoint;
            }
            return _entity.transform.position + force;
        }

    }
}