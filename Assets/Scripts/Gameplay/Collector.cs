using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace BallCollector.Gameplay
{
    public class Collector : MonoBehaviour
    {
        public event Action<float,float> ColliderRadiusChanged;
        
        [SerializeField] private float _collectionFactor;
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private float _currentVolume;
        [SerializeField] private Transform _meshTransform;
        
        [SerializeField] private Material _sphereMaterial; //TODO Move this field to separate class.
        [SerializeField] private float _materialDepthRatio; //TODO Move this field to separate class.
        [SerializeField] private float _materialStrengthRatio = 60;//TODO Move this field to separate class.
        
        public SphereCollider Collider => _collider;
        public float CurrentVolume => _currentVolume;
        
        private List<CollectableItem> _collectableItems = new List<CollectableItem>();

        private float _targetRadius;
        private float _growTime = 0.5f;

        private float _colliderToMeshRatio;
        float _distanceToItemOuterPoint;
        private Vector3 _targetMeshScale;
        
        
        // Constants for optimisation of radius calculation.
        private const float _3div4Pi = 3.0f / (4 * Mathf.PI);
        private const float _1div4 = 1.0f / 3.0f;

        private Vector3 _sphereCenter;
        private readonly int _depthDistance = Shader.PropertyToID("_DepthDistance");
        private readonly int _normalStrength = Shader.PropertyToID("_NormalStrength");


        private void Awake()
        {
            DOTween.SetTweensCapacity(2000, 50); //TODO move somewhere else.
            
            _targetRadius = _collider.radius;
            _currentVolume = (4.0f / 3.0f) * Mathf.PI * Mathf.Pow(_targetRadius, 3);

            _targetMeshScale = _meshTransform.localScale;
            _colliderToMeshRatio = _targetMeshScale.x / _collider.radius;
            
           _sphereMaterial.SetFloat(_depthDistance,_materialDepthRatio*_targetRadius);
           _sphereMaterial.SetFloat(_normalStrength,_materialStrengthRatio*_targetRadius );
           ColliderRadiusChanged?.Invoke(_targetRadius, 0);
        }

        private void TryCollectItem(CollectableItem item)
        {
            if(_currentVolume/item.Volume< _collectionFactor)
                return;
            
            _collectableItems.Add(item);
            item.transform.parent = _itemsContainer;
            item.BecomeCollected(_collider.radius);

            IncreaseRadius(item.Volume);
            ColliderRadiusChanged?.Invoke(_targetRadius, _growTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out CollectableItem item))
            {
                TryCollectItem(item);
            }
        }

        private void IncreaseRadius(float additionalVolume)
        {
            _currentVolume += additionalVolume;
            _targetRadius = Mathf.Pow(_currentVolume * _3div4Pi, _1div4);
            
            _collider.radius = _targetRadius;
            DOTween.To(() => _collider.radius, x => _collider.radius = x, _targetRadius, _growTime);
            ColliderRadiusChanged?.Invoke(_targetRadius, _growTime);

            _targetMeshScale = Vector3.one * _collider.radius * _colliderToMeshRatio;
            _meshTransform.DOScale(_targetMeshScale,_growTime); //TODO remake as sequence
            
            _sphereMaterial.SetFloat(_depthDistance,_materialDepthRatio*_collider.radius); // TODO Add Interpolation.
            _sphereMaterial.SetFloat(_normalStrength,_materialStrengthRatio*_collider.radius); // TODO Add Interpolation.
        }
        
    }
}