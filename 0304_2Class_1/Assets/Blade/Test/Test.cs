using System;
using Blade.Players;
using UnityEngine;

namespace Blade.Test
{
    public class Test : MonoBehaviour
    {
        public PlayerInputSO playerInputSO;
        private Rigidbody _rb;

        private Vector3 movement;

        private Vector3 pos;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            playerInputSO.OnAttackPressed += HandeleAttackPressd;
        }

        private void OnDisable()
        {
            playerInputSO.OnAttackPressed -= HandeleAttackPressd;
        }

        private void HandeleAttackPressd()
        {
            movement = GetPlayerDirection().normalized;
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, pos) < 0.1f) _rb.linearVelocity = Vector3.zero;
            else _rb.linearVelocity = movement * 3f;
        }

        private Vector3 GetPlayerDirection()
        {
            pos = playerInputSO.GetWorldPosition();
            pos.y = transform.position.y;
            Debug.Log(pos);
            Vector3 direction = pos - transform.position;
            direction.y = 0;
            return direction;
        }
    }
}
