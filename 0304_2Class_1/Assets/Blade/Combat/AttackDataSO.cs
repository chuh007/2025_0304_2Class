using System;
using UnityEngine;

namespace Blade.Combat
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "SO/Combat/AttackData", order = 0)]
    public class AttackDataSO : ScriptableObject
    {
        public string attackName;
        public float movementPower;
        public float damageMultiplier = 1f; //증가데미지 - 곱연산
        public float damageIncrease = 0; //추가 데미지 - 합연산

        public bool isPowerAttack = false;

        private void OnEnable()
        {
            attackName = this.name; //파일이름으로 AttackName 을 지정한다.
        }
    }
}