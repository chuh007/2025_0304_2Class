using UnityEngine;

namespace Blade.Combat.Debuff
{
    [CreateAssetMenu(fileName = "DebuffParticle", menuName = "SO/Debuff/Particle", order = 0)]
    public class DebuffParticleSO : ScriptableObject
    {
        public DebuffType debuff;
        public ParticleSystem particle;
    }
}