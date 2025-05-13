using System;
using Blade.Entities;
using Blade.FSM;
using Chuh007Lib.Dependencies;
using UnityEngine;

namespace Blade.Players
{
    
    public class Player : Entity, IDependencyProvider
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [SerializeField] private StateDataSO[] states;
        
        private EntityStateMachine _stateMachine;

        [Provide]
        public Player ProvidePlayer() => this;

        #region Temp region

        public float rollingVelocity = 2.2f;

        #endregion
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);
            
            PlayerInput.OnRollingPressed += HandleRollingKeyPressed;
        }

        private void OnDestroy()
        {
            PlayerInput.OnRollingPressed -= HandleRollingKeyPressed;
        }

        private void HandleRollingKeyPressed()
        {
            ChangeState("ROLLING");
        }

        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }
        
        public void ChangeState(string newStateName) => _stateMachine.ChangeState(newStateName);
        
    }
}