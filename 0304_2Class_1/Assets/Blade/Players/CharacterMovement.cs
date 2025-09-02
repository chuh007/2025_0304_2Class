using System;
using Blade.Combat;
using Blade.Entities;
using Chuh007Lib.StatSystem;
using DG.Tweening;
using UnityEngine;

namespace Blade.Players
{
    public class CharacterMovement : MonoBehaviour, IEntityComponent, IAfterInitialize, IKnockBackable
    {
        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private CharacterController controller;
        // [SerializeField] private Transform parent;
        
        private float _moveSpeed = 8f;
        public bool IsGround => controller.isGrounded;
        public bool CanManualMovement { get; set; } = true;
        private Vector3 _autoMovement;
        private float _autoMoveStartTime;
        private MovementDataSO _movementData;

        private Vector3 _velocity;
        public Vector3 Velocity => _velocity;
        private float _verticalVelocity;
        private Vector3 _movementDirection;

        private Entity _entity;
        private EntityStat _statCompo;
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
        }
        
        public void AfterInitialize()
        {
            _moveSpeed = _statCompo.SubscribeStat(moveSpeedStat, HandleMoveSpeedChange, 1f);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(moveSpeedStat, HandleMoveSpeedChange);
        }

        private void HandleMoveSpeedChange(StatSO stat, float currentvalue, float prevvalue)
        {
            _moveSpeed = currentvalue;
        }

        public void SetMovementDirection(Vector2 movementInput)
        {
            _movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            ApplyGravity();
            Move();
        }

        private void CalculateMovement()
        {
            if (CanManualMovement)
            {
                _velocity = Quaternion.Euler(0, -45f, 0) * _movementDirection;
                _velocity *= _moveSpeed * Time.fixedDeltaTime;
            }
            else
            {
                float normalizedTime = (Time.time - _autoMoveStartTime) / _movementData.duration;
                float currentSpeed = _movementData.maxSpeed * _movementData.moveCurve.Evaluate(normalizedTime);
                Vector3 currentMovement = _autoMovement * currentSpeed;
                _velocity = currentMovement * Time.fixedDeltaTime;
            }

            if (_velocity.magnitude > 0 && CanManualMovement)
            {
                Quaternion targetRot = Quaternion.LookRotation(_velocity);
                float rotationSpeed = 20f;
                Transform parent = _entity.transform;
                parent.rotation = Quaternion.Lerp(parent.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed);
            }
        }

        private void ApplyGravity()
        {
            if(IsGround && _verticalVelocity < 0)
                _verticalVelocity = -0.03f;
            else 
                _verticalVelocity += gravity * Time.fixedDeltaTime;
            
            _velocity.y = _verticalVelocity;
        }

        private void Move()
        {
            controller.Move(_velocity);
        }

        public void StopImmediately()
        {
            _movementDirection = Vector3.zero;
        }
        
        // public void SetAutoMovement(Vector3 autoMovement) => _autoMovement = autoMovement;
        
        public void ApplyMovementData(Vector3 direction, MovementDataSO movementData)
        {
            _autoMovement = direction;
            _autoMoveStartTime = Time.time;
            _movementData = movementData;
        }
        
        public void KnockBack(Vector3 direction, MovementDataSO movementData)
        {
            _autoMoveStartTime = Time.time;
            _autoMovement = direction;
            _movementData = movementData;
        }
    }
}