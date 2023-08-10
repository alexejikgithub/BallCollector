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

        private void Awake()
        {
            var radius = _collider.radius;
            _targetRadius = radius;
            _currentVolume = (4.0f / 3.0f) * Mathf.PI * Mathf.Pow(radius, 3);

            _targetMeshScale = _meshTransform.localScale;
            _colliderToMeshRatio = _targetMeshScale.x / _collider.radius;
        }

        private void CollectItem(CollectableItem item)
        {
            if(_currentVolume/item.Volume< _collectionFactor)
                return;
            
            _collectableItems.Add(item);
            item.BecomeCollected();
            item.transform.parent = _itemsContainer;
            IncreaseRadius(item.Volume);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out CollectableItem item))
            {
                CollectItem(item);
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
            _meshTransform.DOScale(_targetMeshScale,_growTime);
            DisableHiddenItems();
        }

        private void DisableHiddenItems()
        {
            
            foreach (var item in _collectableItems)
            {
                _distanceToItemOuterPoint = (item.Collider.bounds.center - _collider.transform.position).magnitude;

                if (_distanceToItemOuterPoint + item.Collider.bounds.extents.magnitude <= _collider.radius)
                {
                    item.gameObject.SetActive(false);
                }
            }
            _collectableItems.RemoveAll(item => !item.gameObject.activeSelf);
        }
    }
}