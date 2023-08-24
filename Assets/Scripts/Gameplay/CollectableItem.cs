using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BallCollector.Gameplay
{
    public class CollectableItem : MonoBehaviour
    {
        public event Action Collected;

        [SerializeField] private bool _isAlwaysKinematic;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _mainCollider;
        [SerializeField] private GameObject _noPhisicsCollider;
        [SerializeField] private MeshRenderer[] _meshRenderers;
        [SerializeField] private float _volume;
        [SerializeField] private Material _dissolveMaterial;
        [SerializeField] private float _dissolveNoiseFactor = 50;
        [SerializeField] private Vector2 _dissolveNoiseDispersion = new Vector2(10, 30); //TODO make universal.
        [SerializeField] private Vector2 _disolveTimeDispersion = new Vector2(1, 3); //TODO make universal.

        [Space] [SerializeField] private CollectableItem[] _dependatnItems;

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

            if(_isAlwaysKinematic)
                return;
            _rigidbody.isKinematic = false;
        }

        public void BecomeCollected(float radius)
        {
            _rigidbody.isKinematic = true;
            DisableCollider();

            transform.DOLocalMove(
                    transform.localPosition.normalized * (radius - _mainCollider.bounds.extents.magnitude), 5f)
                .OnComplete(Dissolve);
            Collected?.Invoke();
        }

        private void DisableCollider()
        {
            _mainCollider.isTrigger = true;
        }

        private void Dissolve()
        {
            var dissolveMaterial = new Material(_dissolveMaterial);
            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.material = dissolveMaterial;
            }

            dissolveMaterial.SetFloat(_noiseScale,
                Random.Range(_dissolveNoiseDispersion.x, _dissolveNoiseDispersion.y) * _dissolveNoiseFactor);
            dissolveMaterial
                .DOFloat(1f, _alphaClip, Random.Range(_disolveTimeDispersion.x, _disolveTimeDispersion.y))
                .OnComplete(() => gameObject.SetActive(false));
            foreach (var item in _dependatnItems)
            {
                item.Dissolve();
            }
        }

        public void SetVolume(float volume)
        {
            _volume = volume;
        }
#if UNITY_EDITOR
        public void SetVolume()
        {
            if (_mainCollider != null)
            {
                _volume = CalculateExactVolume(_mainCollider);
            }
        }

         float CalculateExactVolume(Collider collider)
            {
                switch (collider)
                {
                    case BoxCollider boxCollider:
                        Vector3 size = boxCollider.size;
                        Vector3 scale = collider.transform.localScale;
                        float scaledVolume = size.x * size.y * size.z * scale.x * scale.y * scale.z;
                        return scaledVolume;
        
                    case SphereCollider sphereCollider:
                        float radius = sphereCollider.radius;
                        float scaledSphereVolume = (4f / 3f) * Mathf.PI * Mathf.Pow(radius, 3);
                        Vector3 sphereScale = collider.transform.localScale;
                        return scaledSphereVolume * sphereScale.x * sphereScale.y * sphereScale.z;
        
                    case CapsuleCollider capsuleCollider:
                        float radiusC = capsuleCollider.radius;
                        float height = capsuleCollider.height;
                        float scaledCapsuleVolume = Mathf.PI * Mathf.Pow(radiusC, 2) * (height - 2 * radiusC / 3);
                        Vector3 capsuleScale = collider.transform.localScale;
                        return scaledCapsuleVolume * capsuleScale.x * capsuleScale.y * capsuleScale.z;
        
                    // Add cases for other collider types if needed
                    
                    default:
                        Bounds bounds = collider.bounds;
                        Vector3 sizeDefault = bounds.size;
                        Vector3 defaultScale = collider.transform.localScale;
                        float scaledDefaultVolume = sizeDefault.x * sizeDefault.y * sizeDefault.z * defaultScale.x * defaultScale.y * defaultScale.z;
                        return scaledDefaultVolume;
                }
            }

#endif
    }
}