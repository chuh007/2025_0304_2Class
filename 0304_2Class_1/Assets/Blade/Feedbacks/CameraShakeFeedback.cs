using Blade.Entities;
using Unity.Cinemachine;
using UnityEngine;

namespace Blade.Feedbacks
{
    public class CameraShakeFeedback : Feedback
    {
        [SerializeField] private float impulseForce = 0.6f;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private EntityActionData actionData;
        
        public override void CreateFeedback()
        {
            if(actionData.HitByPowerAttack == false) return;
            
            impulseSource.GenerateImpulse(impulseForce);
        }

        public override void StopFeedback()
        {
            //do nothing
        }
    }
}