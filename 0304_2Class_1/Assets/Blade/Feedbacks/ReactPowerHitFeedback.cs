using System;
using Blade.Enemies;
using Blade.Enemies.BT.Events;
using Blade.Enemies.Skeletons;
using Blade.Entities;
using UnityEngine;

namespace Blade.Feedbacks
{
    public class ReactPowerHitFeedback : Feedback
    {
        [SerializeField] private EntityActionData actionData;
        [SerializeField] private CommonEnemy enemy;

        private StateChange _stateChannel;
        
        private void Start()
        {
            _stateChannel = enemy.GetBlackboardVariable<StateChange>("StateChannel");
        }

        public override void CreateFeedback()
        {
            if (actionData.HitByPowerAttack)
            {
                _stateChannel.SendEventMessage(EnemyState.HIT);
            }
        }
        
        public override void StopFeedback()
        {
            
        }
    }
}