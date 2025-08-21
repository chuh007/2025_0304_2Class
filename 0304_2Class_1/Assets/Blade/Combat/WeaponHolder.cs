using System;
using Blade.Core;
using Blade.Entities;
using Blade.Events;
using UnityEngine;

namespace Blade.Combat
{
    public class WeaponHolder : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;
        [SerializeField] private Weapon[] weapons;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void HandleDead(PlayerDead obj)
        {
            DropWeapons();
        }

        public void DropWeapons()
        {
            foreach (var weapon in weapons)
            {
                weapon.Drop();
            }
        }
    }
}