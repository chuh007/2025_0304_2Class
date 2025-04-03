using System;
using System.Collections.Generic;
using Blade.Players;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Blade.Test
{
    public class RoadGridSystem : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private Grid mapGrid;
        [SerializeField] private GameObject roadBlockPrefab;

        public UnityEvent<bool> ConstructionModeChange;
        public UnityEvent UpdateNavigation;
        
        private bool _constructionMode = false;

        private HashSet<Vector3Int> _roadPoints;

        public bool ConstructionMode
        {
            get => _constructionMode;
            set
            {
                _constructionMode = value;
                ConstructionModeChange?.Invoke(_constructionMode);
            }
        }

        private void Awake()
        {
            _roadPoints = new HashSet<Vector3Int>();
            playerInput.OnAttackPressed += HandleClick;
        }

        private void OnDestroy()
        {
            playerInput.OnAttackPressed -= HandleClick;
        }

        private void HandleClick()
        {
            if (ConstructionMode == false) return;
            
            Vector3 worldPosition = playerInput.GetWorldPosition();
            Vector3Int cellPoint = mapGrid.WorldToCell(worldPosition);

            if (_roadPoints.Add(cellPoint))
            {
                Vector3 center = mapGrid.GetCellCenterWorld(cellPoint);
                GameObject road = Instantiate(roadBlockPrefab, center, Quaternion.identity);
                road.transform.SetParent(transform);
            }
        }

        private void Update()
        {
            if(Keyboard.current.tabKey.wasPressedThisFrame)
                ConstructionMode = !ConstructionMode;
        }
    }
}