using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform parent;

    public bool isGround => controller.isGrounded;

    private Vector3 _velocity;
    public Vector3 Velocity => _velocity;
    private float _verticalVelocity;
    private Vector3 _movementDirection;

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
        _velocity = Quaternion.Euler(0, -45f, 0) * _movementDirection;
        _velocity *= moveSpeed * Time.fixedDeltaTime;

        if (_velocity.magnitude > 0)
        {
            Quaternion targetRot = Quaternion.LookRotation(_velocity);
            float rotateSpeed = 8f;
            parent.rotation = Quaternion.Lerp(parent.rotation, targetRot, Time.deltaTime * rotateSpeed);
        }
    }

    private void ApplyGravity()
    {
        if (isGround && _verticalVelocity < 0)
            _verticalVelocity = -0.03f;
        else
            _verticalVelocity += gravity * Time.fixedDeltaTime;

        _velocity.y = _verticalVelocity;
    }

    private void Move()
    {
        controller.Move(_velocity);
    }

}
