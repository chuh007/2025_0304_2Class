using System;
using UnityEngine;

namespace Chuh007Lib.StatSystem
{
    [Serializable]
    public class StatOverride
    {
        [SerializeField] private StatSO stat;
        [SerializeField] private bool isUseOverride;
        [SerializeField] private float overrideValue;
        
        public StatOverride(StatSO stat) => this.stat = stat;

        // 기본 에셋인 SO를 클론해서 오버라이드 하거나 기본값으로 만들어주는 메서드
        public StatSO CreateStat()
        {
            StatSO newStat = stat.Clone() as StatSO;
            Debug.Assert(newStat != null, $"{nameof(stat)} stat cloning failed");

            if (isUseOverride)
            {
                newStat.BaseValue = overrideValue;
            }
            
            return newStat;
        }
    }
}