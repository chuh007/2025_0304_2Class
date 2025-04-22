using System;
using Blade.Core.Dependencies;
using Blade.Enemies;
using Blade.Players;
using UnityEngine;

namespace Blade.Managers
{
    [DefaultExecutionOrder(-1)]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField, Inject] private Player player;
        [SerializeField] private EntityFinderSO playerFinder;

        private void Awake()
        {
            playerFinder.SetTarget(player);
        }
    }
}