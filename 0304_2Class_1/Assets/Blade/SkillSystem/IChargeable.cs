namespace Blade.SkillSystem
{
    public interface IChargeable
    {
        bool IsCharging { get; }
        void StartCharging();
        void ReleaseCharging();
        void CancelCharging();
    }
}