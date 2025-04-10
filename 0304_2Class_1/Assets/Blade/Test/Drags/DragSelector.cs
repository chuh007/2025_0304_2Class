using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.Test.Drags
{
    public class DragSelector : MonoBehaviour
    {
        [SerializeField] private SelectionIcon selectionIcon; //1
        [SerializeField] private LayerMask whatIsGround, whatIsUnit;
        private HashSet<TestUnit> _selectedUnits;

        private Vector2 _dragStartUIPosition;
        private Vector3 _dragStartWorldPosition;
        
        private Collider[] _results; //오버랩 박스를 위한 컬라이더셋

        private Vector2 _currentMousePosition;
        private bool _isMultiSelection = false; //2

        public Action<bool> OnMouseStatusChange;
        private void Awake()
        {
            _results = new Collider[10];
            _selectedUnits = new HashSet<TestUnit>();

            OnMouseStatusChange += HandleMouseStatusChange;
        }

        private void OnDestroy()
        {
            OnMouseStatusChange -= HandleMouseStatusChange;
        }

        //간단하게 마우스 처리
        private void Update()
        {
            if(Mouse.current.leftButton.wasPressedThisFrame)
                OnMouseStatusChange?.Invoke(true);
            else if(Mouse.current.leftButton.wasReleasedThisFrame)
                OnMouseStatusChange?.Invoke(false);

            _currentMousePosition = Mouse.current.position.ReadValue();

            if (_isMultiSelection)
            {
                Vector2 delta = _currentMousePosition - _dragStartUIPosition;
                selectionIcon.SetSize(delta);

                UpdateSelection();
            }
        }

        private void UpdateSelection()
        {
            GetRaycastInfo(out RaycastHit hit);
            Vector3 currentWorldPosition = hit.point;

            Vector3 boxSize = currentWorldPosition - _dragStartWorldPosition;
            Vector3 center = _dragStartWorldPosition + boxSize * 0.5f;
            float yAngle = Camera.main.transform.eulerAngles.y;
            Vector3 xAxis = Quaternion.Euler(0, yAngle, 0) * Vector3.right;
            Vector3 zAxis = Quaternion.Euler(0, yAngle, 0) * Vector3.forward;

            float xSize = Mathf.Abs(Vector3.Dot(xAxis, boxSize));
            float zSize = Mathf.Abs(Vector3.Dot(zAxis, boxSize));

            int cnt = Physics.OverlapBoxNonAlloc(center, new Vector3(xSize, 5f, zSize) * 0.5f, _results,
                Quaternion.Euler(0, yAngle, 0), whatIsUnit);

            foreach (var unit in _selectedUnits)
            {
                unit.SetSelected(false);
            }
            _selectedUnits.Clear();

            for (int i = 0; i < cnt; i++)
            {
                Collider target = _results[i];
                if (target.TryGetComponent(out TestUnit unit))
                {
                    _selectedUnits.Add(unit);
                    unit.SetSelected(true);
                }
            }
        }

        private void HandleMouseStatusChange(bool isPointerDown)
        {
            SetMultiSelection(isPointerDown);
        }

        private void SetMultiSelection(bool isPointerDown)
        {
            selectionIcon.SetActive(isPointerDown);
            _isMultiSelection = isPointerDown;

            if (_isMultiSelection)
            {
                selectionIcon.SetSize(Vector2.one); //시작할때 1칸 크기로 초기화
                _dragStartUIPosition = _currentMousePosition; //시작 UI좌표를 저장한다.
                GetRaycastInfo(out RaycastHit hit);
                _dragStartWorldPosition = hit.point; //시작 월드 좌표를 저장하고
                
                selectionIcon.SetPosition(_dragStartUIPosition);
            }
        }

        private bool GetRaycastInfo(out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(_currentMousePosition);
            bool isHit = Physics.Raycast(ray, out hit, Camera.main.farClipPlane, whatIsGround);
            return isHit;
        }
        
        private Vector3 _lastWorldPosition;
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                if (_isMultiSelection)
                {
                    GetRaycastInfo(out RaycastHit hit);
                    _lastWorldPosition = hit.point;
                    Debug.Log($"start : {_dragStartWorldPosition}, end : {_lastWorldPosition}");
                }
        
                Vector3 boxSize = _lastWorldPosition - _dragStartWorldPosition;
                Vector3 center = _dragStartWorldPosition + boxSize * 0.5f;
        
                //요기 값은 실제 카메라의 회전치에 맞추어서 회전해야한다.
                Vector3 xAxis = Quaternion.Euler(0, -45f, 0) * Vector3.right;
                Vector3 zAxis = Quaternion.Euler(0, -45f, 0) * Vector3.forward;
        
                float xSize = Vector3.Dot(xAxis, boxSize);
                float zSize = Vector3.Dot(zAxis, boxSize);
        
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_dragStartWorldPosition, 0.3f);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_lastWorldPosition, 0.3f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(center, 0.3f);
        
        
                // 새 변환 설정
                Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, -45f, 0), Vector3.one);
        
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(xSize, 4 , zSize));
        
                Gizmos.matrix = Matrix4x4.identity;
            }
        }

    }
}