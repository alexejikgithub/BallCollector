using BallCollector.CoreSystem;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace BallCollector.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _distanceFactor = 20;
        [SerializeField] private float _maximumDistance = 40;
        [SerializeField] private  CinemachineVirtualCamera _virtualCamera;

        [Inject] private Player _player;

        private CinemachineFramingTransposer _virtualCameraFramingTransposer;

        private float _targetDistance;

        private void OnEnable()
        {
            _player.Collector.ColliderRadiusChanged += SetCameraDistance;
            CinemachineComponentBase componentBase = _virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is CinemachineFramingTransposer)
            {
                _virtualCameraFramingTransposer = componentBase as CinemachineFramingTransposer; // your value
            }

            SetPlayer();
        }

        private void OnDisable()
        {
            _player.Collector.ColliderRadiusChanged -= SetCameraDistance;
        }

        private void SetCameraDistance(float collectorRadius, float growTime)
        {
            _targetDistance = _distanceFactor * collectorRadius;
            _targetDistance = Mathf.Clamp(_targetDistance, 0, _maximumDistance);
            DOTween.To(() => _virtualCameraFramingTransposer.m_CameraDistance, x => _virtualCameraFramingTransposer.m_CameraDistance = x, _targetDistance, growTime);

        }

        public void SetPlayer()
        {
            var playerTransform = _player.SphereMovement.transform;
            _virtualCamera.Follow=playerTransform;
            _virtualCamera.LookAt=playerTransform;
        }
    }
}