using Unity.AI.Navigation;
using UnityEngine;

namespace Blade.Test.NavMesh
{
    public class TestNavSurface : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface navMeshSurface;

        //기존의 네브메시를 삭제하고 새로 굽는다. 
        public void ReBakeNavMesh() => navMeshSurface.BuildNavMesh();
    }
}