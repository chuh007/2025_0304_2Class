using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blade.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Collider weaponCollider;
        [SerializeField] private Rigidbody rbCompo;


        private void Awake()
        {
            weaponCollider.enabled = false;
            rbCompo.isKinematic = true;
        }

        public void Drop()
        {
            weaponCollider.enabled = true;
            rbCompo.isKinematic = false;
            transform.SetParent(null);
            DOVirtual.DelayedCall(3f, () => Destroy(gameObject));
        }
    }
}