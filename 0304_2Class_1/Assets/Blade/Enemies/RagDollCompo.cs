using System.Collections.Generic;
using System.Linq;
using Blade.Entities;
using UnityEngine;

namespace Blade.Enemies
{
    public class RagDollCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Transform ragDollParentTrm;
        [SerializeField] private LayerMask whatIsBody;

        private List<RagDollPart> _partList;
        private Collider[] _results;
        private RagDollPart _defaultPart;
        private EntityActionData _actionData;
        
        public void Initialize(Entity entity)
        {
            _actionData = entity.GetCompo<EntityActionData>();
            _results = new Collider[1];
            _partList = ragDollParentTrm.GetComponentsInChildren<RagDollPart>().ToList();
            foreach (RagDollPart part in _partList)
            {
                part.Initialize(); //각 파츠 초기화
            }
            Debug.Assert(_partList.Count > 0, $"No ragdoll part found in {gameObject.name}");
            _defaultPart = _partList[0]; //첫번째 파츠를 디폴트 값으로 넣어준다.

            entity.OnDeadEvent.AddListener(HandleDeatEvent);
            
            SetRagDollActive(false);
            SetColliderActive(false);
        }

        private void HandleDeatEvent()
        {
            SetRagDollActive(true);
            SetColliderActive(true);

            const float force = -30f;
            AddForceToRagDoll(_actionData.HitNormal * force, _actionData.HitPoint);
        }

        private void SetRagDollActive(bool isActive)
        {
            foreach (RagDollPart part in _partList)
            {
                part.SetRagDollActive(isActive);
            }
        }

        private void SetColliderActive(bool isActive)
        {
            foreach (RagDollPart part in _partList)
            {
                part.SetCollider(isActive);
            }
        }

        public void AddForceToRagDoll(Vector3 force, Vector3 point)
        {
            const float radius = 0.5f;
            int count = Physics.OverlapSphereNonAlloc(point, radius, _results, whatIsBody);
            if (count > 0)
            {
                _results[0].GetComponent<RagDollPart>().KnockBack(force, point);
            }
            else
            {
                _defaultPart.KnockBack(force, point);
            }
        }
    }
}