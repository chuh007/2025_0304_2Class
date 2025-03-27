using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TempCode
{
    [CreateAssetMenu(fileName = "TempInput", menuName = "Temp/Input", order = 0)]
    public class NewPlayerInput : ScriptableObject,Controls.IPlayerActions
    {
        [SerializeField] private LayerMask whatIsGround;
        public Action OnAttackPressed;

        private Vector2 _screenPosition;
        private Vector3 _worldPosition;

        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnAttackPressed?.Invoke();
        }

        public void OnRolling(InputAction.CallbackContext context)
        {
            
        }

        public void OnPointer(InputAction.CallbackContext context)
        {
            _screenPosition = context.ReadValue<Vector2>();
        }

        public Vector3 GetWorldPosition()
        {
            Camera mainCam = Camera.main;
            Ray camRay = mainCam.ScreenPointToRay(_screenPosition);

            bool isHit =Physics.Raycast(camRay, out RaycastHit hit, mainCam.farClipPlane, whatIsGround);

            if (isHit)
            {
                _worldPosition = hit.point;
            }

            return _worldPosition;
        }
    }
}