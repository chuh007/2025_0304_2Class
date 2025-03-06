using System;
using UnityEngine;

namespace Blade.Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private PlayerInputSO playerInput;

        private void Awake()
        {
            playerInput.OnMovementChange += HandleMovementChange;
        }

        private void OnDestroy()
        {
            playerInput.OnMovementChange -= HandleMovementChange;
        }

        private void HandleMovementChange(Vector2 movementInput)
        {
            movement.SetMovementDirection(movementInput);
        }
    }
}