using System;
using Blade.Entities;
using Chuh007Lib.StatSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.Test
{
    public class StatTester : MonoBehaviour
    {
        [SerializeField] private EntityStat statCompo;
        [SerializeField] private StatSO targetStat;
        [SerializeField] private float modifyValue;

        private void Update()
        {
            if (Keyboard.current.oKey.wasPressedThisFrame)
            {
                Debug.Log("o");
                statCompo.AddModifier(targetStat, this, modifyValue);
            }

            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                Debug.Log("p");
                statCompo.RemoveModifer(targetStat, this);
            }
        }
    }
}