using System;
using Blade.Players;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
namespace Blade.Test
{
    public class NavMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private NavMeshAgent navAgent;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            playerInput.OnAttackPressed += HandlePointerClink;
        }

        private void OnDestroy()
        {
            playerInput.OnAttackPressed -= HandlePointerClink;
        }

        private void HandlePointerClink()
        {
            Vector3 position = playerInput.GetWorldPosition();
            navAgent.SetDestination(position);
        }
    }
}