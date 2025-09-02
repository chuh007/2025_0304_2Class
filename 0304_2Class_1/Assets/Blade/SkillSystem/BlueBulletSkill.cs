using Blade.Combat;
using Blade.Entities;
using Blade.Players;
using Chuh007Lib.Dependencies;
using Chuh007Lib.ObjectPool.RunTime;
using Chuh007Lib.StatSystem;
using UnityEngine;

namespace Blade.SkillSystem
{
    public class BlueBulletSkill : Skill
    {
        [SerializeField] private ParticleSystem[] muzzleEffects;
        [SerializeField] private PlayerInputSO playerInput;
        
        [SerializeField] private PoolItemSO blueBullet;
        [SerializeField] private AttackDataSO skillAttackData;
        [SerializeField] private StatSO damageStat;
        [SerializeField] private float damageMultiplier = 1.2f;
        [SerializeField] private float bulletSpeed = 25f;
        
        [Inject] private PoolManagerMono _poolManager;
        private EntityAnimatorTrigger _trigger;
        private DamageCalcCompo _damageCompo;
        private int _muzzleIndex;
        
        public override void InitializeSkill(Entity owner, SkillComponent skillComponent)
        {
            base.InitializeSkill(owner, skillComponent);
            _trigger = owner.GetCompo<EntityAnimatorTrigger>();
            _damageCompo = owner.GetCompo<DamageCalcCompo>();
        }

        public override void UseSkill()
        {
            Vector3 worldPosition = playerInput.GetWorldPosition();
            _owner.RotateToTarget(worldPosition);
            
            _muzzleIndex = 0;
            _trigger.OnCastSkillTrigger += HandleDamageCastTrigger;
        }

        private void HandleDamageCastTrigger()
        {
            if(_muzzleIndex >= muzzleEffects.Length) return;
            ParticleSystem firePosEffect = muzzleEffects[_muzzleIndex];
            firePosEffect.Play();

            Projectile bullet = _poolManager.Pop<Projectile>(blueBullet);
            DamageData damageData = _damageCompo.CalculateDamage(damageStat, skillAttackData, damageMultiplier);
            
            Quaternion forwardRotation = Quaternion.LookRotation(firePosEffect.transform.forward);
            bullet.SetUpProjectile(_owner, damageData,
                firePosEffect.transform.position, forwardRotation,
                firePosEffect.transform.forward * bulletSpeed);
            
            _muzzleIndex++;
            if (_muzzleIndex >= muzzleEffects.Length)
            {
                base.UseSkill();
                _trigger.OnCastSkillTrigger -= HandleDamageCastTrigger;
            }
        }
    }
}