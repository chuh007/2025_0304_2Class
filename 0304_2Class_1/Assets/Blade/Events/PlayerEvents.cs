using Blade.Core;

namespace Blade.Events
{
    public static class PlayerEvents
    {
        public static readonly PlayerDead PlayerDead = new PlayerDead();
        
        
    }
    
    public class PlayerDead : GameEvent {}
}