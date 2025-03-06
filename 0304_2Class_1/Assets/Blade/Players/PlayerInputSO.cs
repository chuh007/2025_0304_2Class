using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.Players
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/PlayerInputSO")]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public event Action<Vector2> OnMovementChange;
        public event Action OnAttackPressed;
        public event Action OnRollingPressed;

        private Controls _controls;

        private void OnEnable()
        {
            if(_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();
            OnMovementChange?.Invoke(movement);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnAttackPressed?.Invoke();
        }

        public void OnRolling(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnRollingPressed?.Invoke();
        }
    }
}
