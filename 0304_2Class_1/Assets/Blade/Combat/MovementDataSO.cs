using UnityEngine;

namespace Blade.Combat
{
    [CreateAssetMenu(fileName = "Movement data", menuName = "SO/Combat/Movement data", order = 0)]
    public class MovementDataSO : ScriptableObject
    {
        public float maxSpeed;
        public float duration;
        public AnimationCurve moveCurve;
    }
}