using System.Collections;
using UnityEngine;
using Zenject;

namespace BallCollector.Gameplay
{
    public class SphereMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _movementForce;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _stopTime;
        [SerializeField] private Collector _collector; //TODO move higher in the hierarchy. 

        private bool _useGyro=false;

        [Inject] private InputFacade _inputFacade;


        private float _forceFactor;
        private float _speedFactor;
        private float _massFactor;
        
        
        
        private Transform _cameraTransform;
        private Vector3 _direction = Vector3.zero;
        private Vector3 _cameraForward;
        private Vector3 _cameraRight;

        private float _currentSpeed;

        private Coroutine _stopCoroutine;

        private void Start()
        {
            var radius = _collector.Collider.radius;
            _forceFactor = _movementForce / radius;
            _speedFactor = _maxSpeed / radius;
            _massFactor = _rigidbody.mass / radius;
            
            EnableMovement();
        }


        public void EnableMovement()
        {
            _cameraTransform = Camera.main.transform; //TODO remake dependency 
            _inputFacade.DownTouched += StartMovement;
            if (_useGyro)
            {
                _inputFacade.GyroOffsetChanged += Move;
            }
            else
            {
                _inputFacade.MouseOffsetChanged += Move;
            }
            
            
            _inputFacade.UpTouched += StopMovement;
            _collector.ColliderRadiusChanged += AdjustMovementParameters;
        }

        public void DisableMovement()
        {
            _inputFacade.DownTouched -= StartMovement;
            if (_useGyro)
            {
                _inputFacade.GyroOffsetChanged -= Move;
            }
            else
            {
                _inputFacade.MouseOffsetChanged -= Move;
            }
            _inputFacade.UpTouched -= StopMovement;
            _collector.ColliderRadiusChanged -= AdjustMovementParameters;
        }

        private void AdjustMovementParameters(float targetRadius, float growTime)
        {
            _movementForce = _forceFactor * targetRadius;
            _maxSpeed = _speedFactor * targetRadius;
            _rigidbody.mass = _massFactor * targetRadius;
        }

        private void StartMovement()
        {
            if (_stopCoroutine != null)
            {
                StopCoroutine(_stopCoroutine);
            }
        }

        private void Move(Vector2 offset)
        {
            offset = offset.normalized;
            _cameraForward = _cameraTransform.forward;
            _cameraRight = _cameraTransform.right;

            _cameraForward.y = 0f;
            _cameraRight.y = 0f;

            _direction = _cameraForward * offset.y + _cameraRight * offset.x;

            _rigidbody.AddForce(_direction * _movementForce);
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
        }

        private void StopMovement()
        {
            _stopCoroutine = StartCoroutine(Stopping());
        }

        private IEnumerator Stopping()
        {
            var startVelocity = _rigidbody.velocity;

            float elapsedTime = 0;

            while (elapsedTime < _stopTime)
            {
                _rigidbody.velocity = Vector3.Lerp(startVelocity, Vector3.zero, (elapsedTime / _stopTime));
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _rigidbody.velocity = Vector3.zero;
        }
    }
}