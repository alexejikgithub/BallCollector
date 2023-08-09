using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace BallCollector.Gameplay
{
    public class Collector : MonoBehaviour
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private SphereCollider _collider;
        private List<CollectableItem> _collectableItems = new List<CollectableItem>();

        private float _targetRadius;
        private readonly float _increaseFactor = 0.1f;
        private float _growTime = 0.5f;

        private void Awake()
        {
            _targetRadius = _collider.radius;
        }

        private void CollectItem(CollectableItem item)
        {
            _collectableItems.Add(item);
            item.BecomeCollected();
            item.transform.parent = _itemsContainer;
            IncreaseColliderVolume(item.Volume);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out CollectableItem item))
            {
                CollectItem(item);
            }
        }


        private void IncreaseColliderVolume(float additionalVolume)
        {
            float term = additionalVolume / ((4.0f / 3.0f) * Mathf.PI) * _increaseFactor;
            float newRadiusCubed = term + Mathf.Pow(_collider.radius, 3);
            _targetRadius = Mathf.Pow(newRadiusCubed, 1.0f / 3.0f);
            DOTween.To(() => _collider.radius, x => _collider.radius = x, _targetRadius, _growTime);
        }
    }
}