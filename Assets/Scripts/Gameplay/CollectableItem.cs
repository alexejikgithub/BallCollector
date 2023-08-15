using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BallCollector.Gameplay
{
    public class CollectableItem : MonoBehaviour
    {
        public event Action Collected; 

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _mainCollider;
        [SerializeField] private GameObject _noPhisicsCollider;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _volume;
        [SerializeField] private Material _dissolveMaterial;
        [SerializeField] private Vector2 _disolveNoiseDispersion = new Vector2(10, 30); //TODO make universal.
        [SerializeField] private Vector2 _disolveTimeDispersion = new Vector2(1, 3); //TODO make universal.

        [Space] 
        [SerializeField] private CollectableItem[] _dependatnItems;
        
        private readonly int _noiseScale = Shader.PropertyToID("_NoiseScale");
        private readonly int _alphaClip = Shader.PropertyToID("_AlphaClip");

        public float Volume => _volume;
        
        [ContextMenu("Disable")]
        public void DisablePhysics()
        {
            if (_noPhisicsCollider != null)
            {
                _noPhisicsCollider.SetActive(true);
                _mainCollider.enabled = false;
            }

            _rigidbody.isKinematic = true;
        }
        [ContextMenu("Enable")]
        public void EnablePhysics()
        {
            foreach (var item in _dependatnItems)
            {
                item.DisablePhysics();
                item.DisableCollider();
                item.transform.parent = transform;
            }
            
            if (_noPhisicsCollider != null)
            {
                _noPhisicsCollider.SetActive(false);
                _mainCollider.enabled = true;
            }
            
            _rigidbody.isKinematic = false;
            
        }

        public void BecomeCollected(float radius)
        {
            _rigidbody.isKinematic = true;
            DisableCollider();
            
            transform.DOLocalMove(transform.localPosition.normalized * (radius - _mainCollider.bounds.extents.magnitude), 5f).OnComplete(Dissolve); 
            Collected?.Invoke();
            
        }

        private void DisableCollider()
        {
            _mainCollider.isTrigger = true;
        }

        private void Dissolve()
        {
            _meshRenderer.material = new Material(_dissolveMaterial);
            _meshRenderer.material.SetFloat(_noiseScale,
                Random.Range(_disolveNoiseDispersion.x, _disolveNoiseDispersion.y));
            _meshRenderer.material
                .DOFloat(1f, _alphaClip, Random.Range(_disolveTimeDispersion.x, _disolveTimeDispersion.y))
                .OnComplete(() => gameObject.SetActive(false));
            foreach (var item in _dependatnItems)
            {
                item.Dissolve();
            }
        }
#if UNITY_EDITOR
        public void SetVolume()
        {
            if (_mainCollider != null)
            { 
                _volume = CalculateApproximateVolume(_mainCollider) ;
            }
        }
        
        float CalculateApproximateVolume(Collider collider)
        {
            Bounds bounds = collider.bounds;
            Vector3 size = bounds.size;

            float approximateVolume = size.x * size.y * size.z;
            return approximateVolume;
        }

#endif
    }
}