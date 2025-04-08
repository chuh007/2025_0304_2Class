using Unity.AI.Navigation;
using UnityEngine;

namespace Blade.Test.NavMesh
{
    public class TestNavSurface : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface navMeshSurface;
        
        public void ReBakeNavMesh() => navMeshSurface.BuildNavMesh();
    }
}