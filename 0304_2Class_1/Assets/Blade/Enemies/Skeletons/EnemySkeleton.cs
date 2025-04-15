using System;
using UnityEngine;

namespace Blade.Enemies.Skeletons
{
    public class EnemySkeleton : Enemy
    {
        [SerializeField] private Vector3[] positions;

        private int _currentIndex = 0;
        private NavMovement _navMovement;

        protected override void Awake()
        {
            base.Awake();
            _navMovement = GetCompo<NavMovement>();
        }

        private void Start()
        {
            _navMovement.SetDestination(positions[_currentIndex]);
        }

        private void Update()
        {
            if (_navMovement.IsArrived)
            {
                _currentIndex = (_currentIndex + 1) % positions.Length;
                _navMovement.SetDestination(positions[_currentIndex]);
            }
        }
    }
}