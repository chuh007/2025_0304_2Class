using System;
using UnityEngine;

namespace Blade.Combat
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "SO/Combat/AttackData", order = 0)]
    public class AttackDataSO : ScriptableObject
    {
        public string attackName;
        public float movementPower;
        public float damageMultiplier = 0; // 증가 데미지 - 곱연산
        public float damageIncrease; // 추가 데미지 - 합연산

        public bool isPowerAttakc = false;

        private void OnEnable()
        {
            attackName = this.name; // 파일 이름으로 AttakcName 을 저장한다.
        }
    }
}