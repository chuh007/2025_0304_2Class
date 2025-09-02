namespace Blade.Combat.Debuff
{
    public interface IDebuffable
    {
        public int CurrentDebuff { get; }
        
        public void PlusDebuff(DebuffType debuff);
        
        public void MinusDebuff(DebuffType debuff);
        
        public bool CheckDebuff(DebuffType debuff);
    }
}