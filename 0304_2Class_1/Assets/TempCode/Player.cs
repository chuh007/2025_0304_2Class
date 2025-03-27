using System;
using Blade.Entities;
using Blade.FSM;
using UnityEngine;

namespace TempCode
{
    public class Player : Entity
    {
        [field: SerializeField] public NewPlayerInput PlayerInput { get; private set; }
        [SerializeField] private Transform debugTrm;

        public EntityStateMachine stateMachine;
        [SerializeField] private StateDataSO[] playerStates;
        
        public Vector3 Destination { get; set; }
        
        protected override void Awake()
        {
            base.Awake();
            stateMachine = new EntityStateMachine(this, playerStates);
            PlayerInput.OnAttackPressed += HandleAttackPressed;
        }

        private void Start()
        {
            stateMachine.ChangeState("IDLE");
        }

        private void Update()
        {
            stateMachine.UpdateStateMachine();
        }

        private void OnDestroy()
        {
            PlayerInput.OnAttackPressed -= HandleAttackPressed;
        }

        private void HandleAttackPressed()
        {
            debugTrm.position = PlayerInput.GetWorldPosition();
        }
    }
}