using DG.Tweening;
using UnityEngine;

namespace Blade.Enemies
{
    public class RagDollPart : MonoBehaviour
    {
        private Rigidbody _rbCompo;
        private Collider _colliderCompo;

        public void Initialize()
        {
            _rbCompo = GetComponent<Rigidbody>();
            _colliderCompo = GetComponent<Collider>();
        }

        public void SetRagDollActive(bool isActive)
            => _rbCompo.isKinematic = !isActive;

        public void SetCollider(bool isActive)
            => _colliderCompo.enabled = isActive;

        public void KnockBack(Vector3 force, Vector3 point)
        {
            DOVirtual.DelayedCall(0.1f, () =>
            {
                _rbCompo.AddForceAtPosition(force, point, ForceMode.Impulse);
            });
        }
    }
}