using System;
using Blade.Core;
using Blade.Events;
using Unity.Cinemachine;
using UnityEngine;

namespace Blade.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO cameraChannel;
        [SerializeField] private CinemachineImpulseSource impulseSource;

        private void Awake()
        {
            cameraChannel.AddListener<ImpulseEvent>(HandleCameraImpulse);
        }

        private void OnDestroy()
        {
            cameraChannel.RemoveListener<ImpulseEvent>(HandleCameraImpulse);
        }

        private void HandleCameraImpulse(ImpulseEvent evt)
        {
            impulseSource.GenerateImpulse(evt.power);
        }
    }
}