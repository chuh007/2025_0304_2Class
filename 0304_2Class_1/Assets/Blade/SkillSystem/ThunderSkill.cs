using Blade.Combat;
using Blade.Effects;
using Blade.Entities;
using Blade.Events;
using Chuh007Lib.Dependencies;
using Chuh007Lib.ObjectPool.RunTime;
using Chuh007Lib.StatSystem;
using UnityEngine;

namespace Blade.SkillSystem
{
    public class ThunderSkill : Skill, IChargeable
    {
        [SerializeField] private RoundDecal decal;
        [SerializeField] private float chargeSpeed = 2f;
        [SerializeField] private float maxRadius = 3f;

        [SerializeField] private StatSO damageStat;
        [SerializeField] private AttackDataSO thunderAttackData;
        [SerializeField] private float damageMultiplier = 1.2f;
        
        [SerializeField] private PoolItemSO thunderEffect;
        [Inject] private PoolManagerMono _poolManager;
        
        public bool IsCharging { get; set; }
        private float _currentRadius;
        private DamageCalcCompo _damageCompo;
        
        public override void InitializeSkill(Entity owner, SkillComponent skillComponent)
        {
            base.InitializeSkill(owner, skillComponent);
            _damageCompo = _owner.GetCompo<DamageCalcCompo>();
            decal.SetProjectorActive(false);
        }

        public override void UseSkill()
        {
            base.UseSkill(); // 쿨타임
            int enemyCount = _skillComponent.GetEnemiesInRange(decal.transform.position, _currentRadius);

            for (int i = 0; i < enemyCount; ++i)
            {
                Collider target = _skillComponent.Colliders[i];
                PoolingEffect effect = _poolManager.Pop<PoolingEffect>(thunderEffect);
                effect.PlayVFX(target.transform.position, Quaternion.identity);
                DelayPooling(effect, 2f);
                
                if (target.TryGetComponent(out IDamageable damageable))
                {
                    DamageData damageData = _damageCompo.CalculateDamage(damageStat, thunderAttackData, damageMultiplier);
                    damageable.ApplyDamage(damageData, target.transform.position, Vector3.up, thunderAttackData, _owner);
                }
            }

            if (enemyCount > 0)
            {
                ImpulseEvent evt = CameraEvents.ImpulseEvent.Initializer(thunderAttackData.impulseForce);
                _skillComponent.CameraChannel.RaiseEvent(evt);
            }
        }

        private async void DelayPooling(PoolingEffect effect, float duration)
        {
            await Awaitable.WaitForSecondsAsync(duration);
            _poolManager.Push(effect);
        }

        public void StartCharging()
        {
            _currentRadius = 0.1f;
            SetChargingStatus(true);
        }

        private void SetChargingStatus(bool isCharging)
        {
            decal.SetProjectorActive(isCharging);
            IsCharging = isCharging;
        }

        public void ReleaseCharging()
        {
            SetChargingStatus(false);
            UseSkill();
        }

        public void CancelCharging()
        {
            SetChargingStatus(false);
        }

        protected override void Update()
        {
            base.Update();
            if (IsCharging)
            {
                _currentRadius += Time.deltaTime * chargeSpeed;
                _currentRadius = Mathf.Clamp(_currentRadius, 0, maxRadius);
                decal.SetDecalSize(_currentRadius);
            }
        }
    }
}