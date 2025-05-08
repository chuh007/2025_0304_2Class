using System.Collections.Generic;
using System.Linq;
using Blade.Players;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

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
        
        [SerializeField] private bool canCombineMesh;
        private MeshFilter _parentMeshFilter;
        
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
            _parentMeshFilter = GetComponent<MeshFilter>();
            _parentMeshFilter.mesh = new Mesh(); //비어있는 메시를 하나 추가한다.
            
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

                if (canCombineMesh)
                {
                    CombineMesh();
                }
                
                UpdateNavigation?.Invoke();
            }
        }

        private void CombineMesh()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(false);
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int vertexCnt = 0;
            for (int i = 0; i < meshFilters.Length; i++)
            {
                if (meshFilters[i].sharedMesh == null) continue;
                
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                
                meshFilters[i].gameObject.SetActive(false);
                
                vertexCnt += meshFilters[i].sharedMesh.vertexCount;
            }

            _parentMeshFilter.mesh = new Mesh(); //새로운 메시를 만들어주고
            if (vertexCnt > 65535)
                _parentMeshFilter.mesh.indexFormat = IndexFormat.UInt32;
            
            _parentMeshFilter.mesh.CombineMeshes(combine);
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if(Keyboard.current.tabKey.wasPressedThisFrame)
                ConstructionMode = !ConstructionMode;
        }

        [SerializeField] private GameObject agentPrefab;

        public void DeployAgent()
        {
            Vector3Int cellPoint = _roadPoints.Skip(Random.Range(0, _roadPoints.Count)).First();
            Vector3 position = mapGrid.GetCellCenterWorld(cellPoint);
            Instantiate(agentPrefab, position, Quaternion.identity);
        }

        
        
        
    }
}