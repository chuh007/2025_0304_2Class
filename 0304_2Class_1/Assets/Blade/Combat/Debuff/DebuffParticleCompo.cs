using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blade.Combat.Debuff
{
    // 콜라이더 있는애한테 넣는거 추천
    public class DebuffParticleCompo : MonoBehaviour, IDebuffable
    {
        [SerializeField] private DebuffParticleSO[] particles;

        private Dictionary<DebuffType, ParticleSystem> particleDict;
        private Dictionary<DebuffType, ParticleSystem> realParticles;

        private void Awake()
        {
            particleDict = new Dictionary<DebuffType, ParticleSystem>();
            realParticles = new Dictionary<DebuffType, ParticleSystem>();
            foreach (var particleSO in particles)
            {
                particleDict.Add(particleSO.debuff, particleSO.particle);
            }
        }

        public int CurrentDebuff { get; private set; }
        public void PlusDebuff(DebuffType debuff)
        {
            CurrentDebuff |= (int)debuff;
            ParticleSystem particle = Instantiate(particleDict[debuff], transform).GetComponent<ParticleSystem>();
            realParticles[debuff] = particle;
            particle.Play();
        }

        public void MinusDebuff(DebuffType debuff)
        {
            CurrentDebuff &= ~(int)debuff;
            ParticleSystem particle = realParticles[debuff];
            realParticles.Remove(debuff);
            Destroy(particle.gameObject);
        }

        public bool CheckDebuff(DebuffType debuff)
        {
            return (CurrentDebuff & (int)debuff) == 0;
        }
    }
}