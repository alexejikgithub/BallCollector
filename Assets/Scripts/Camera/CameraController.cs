using System;
using BallCollector.Gameplay;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace BallCollector.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _distanceFactor = 20;

        //TODO add dependancy injection for sphere size
        [SerializeField] private Collector _collector;
        [SerializeField] private  CinemachineVirtualCamera _virtualCamera;
        
        private CinemachineFramingTransposer _virtualCameraFramingTransposer;

        private float _targetDistance;

        private void Awake()
        {
            _collector.ColliderRadiusChanged += SetCameraDistance;
            CinemachineComponentBase componentBase = _virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is CinemachineFramingTransposer)
            {
                _virtualCameraFramingTransposer = componentBase as CinemachineFramingTransposer; // your value
            }
        }

        private void SetCameraDistance(float collectorRadius, float growTime)
        {
            _targetDistance = _distanceFactor * collectorRadius;
            
            DOTween.To(() => _virtualCameraFramingTransposer.m_CameraDistance, x => _virtualCameraFramingTransposer.m_CameraDistance = x, _targetDistance, growTime);

        }
    }
}