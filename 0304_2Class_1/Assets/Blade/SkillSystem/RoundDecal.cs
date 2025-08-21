using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Blade.SkillSystem
{
    public class RoundDecal : MonoBehaviour
    {
        [SerializeField] private DecalProjector decalProjector;
        [SerializeField] private float depth = 3f;
        
        public void SetProjectorActive(bool isActive)
        {
            decalProjector.enabled = isActive;
        }

        public void SetDecalSize(float radius)
        {
            decalProjector.size = new Vector3(2 * radius, 2 * radius, depth);
        }
    }
}