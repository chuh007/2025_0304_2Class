using System;
using Blade.Players;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Blade.Test.NavMesh
{
    public class SplinePointer : MonoBehaviour
    {
        [SerializeField] private Transform playerPosition;
        [SerializeField] private PlayerInputSO playerInput;

        public float pointerBendAmount = 2;

        private Spline _spline;
        private SplineInstantiate _splineInstantiate;
        private BezierKnot _playerKnot, _objectKnot;

        private void Awake()
        {
            _spline = GetComponent<SplineContainer>().Spline;
            _splineInstantiate = GetComponent<SplineInstantiate>();

            playerInput.OnAttackPressed += HandleClick;
        }

        private void OnDestroy()
        {
            playerInput.OnAttackPressed -= HandleClick;
        }

        private void HandleClick()
        {
            _splineInstantiate.enabled = false;
            Vector3 worldPosition = playerInput.GetWorldPosition();
            FindSelectedTarget(worldPosition);
        }

        private void Start()
        {
            _spline.Add(_playerKnot);
            _spline.Insert(1, _objectKnot);
        }

        private void FindSelectedTarget(Vector3 worldPosition)
        {
            _playerKnot.Position = playerPosition.position + new Vector3(0, 0.2f, 0); //플레이어보다 살짝 위로
            _objectKnot.Position = worldPosition + new Vector3(0, 0.2f, 0);

            _playerKnot.TangentOut = new float3(0, pointerBendAmount, 1f);
            _objectKnot.TangentIn = new float3(0, pointerBendAmount, -1f); //일부러 살짝 휘게 y 조절
            
            _spline.SetKnot(0, _playerKnot);
            _spline.SetKnot(1, _objectKnot);
            
            _spline.SetTangentMode(0, TangentMode.Mirrored, BezierTangent.Out);
            _spline.SetTangentMode(1, TangentMode.Mirrored, BezierTangent.In);

            _splineInstantiate.enabled = true;
        }
    }
}