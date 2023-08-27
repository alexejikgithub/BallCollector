using System;
using System.Collections;
using BallCollector.CoreSystem;
using UnityEngine;
using Zenject;

namespace BallCollector.Gameplay
{
    public class SphereMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _accelerationTime = 0.05f;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _stopTime;
        [SerializeField] private Collector _collector; //TODO move higher in the hierarchy. 


        private float _movementForce;

        private bool _useGyro = false;

        [Inject] private InputFacade _inputFacade;


        //private float _forceFactor;
        private float _speedFactor;
        private float _density;

        private Transform _cameraTransform;
        private Vector3 _direction = Vector3.zero;
        private Vector3 _cameraForward;
        private Vector3 _cameraRight;

        private float _currentSpeed;

        private Coroutine _stopCoroutine;

        private bool _isMoving;

        private void Start()
        {
            var mass = _rigidbody.mass;
            _speedFactor = _maxSpeed / _collector.Collider.radius;
            _density = mass / _collector.CurrentVolume;


            _movementForce = (_rigidbody.mass * _maxSpeed) / _accelerationTime;
            // _forceFactor = _movementForce / (mass * _maxSpeed);
        }

        private void FixedUpdate()
        {
            if (_isMoving)
            {
                _rigidbody.AddForce(_direction * _movementForce);
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
                _rigidbody.AddForce(Physics.gravity * _rigidbody.mass * 100);
            }
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
            StopMovement();
        }

        private void AdjustMovementParameters(float targetRadius, float growTime)
        {
            _maxSpeed = _speedFactor * targetRadius;
            _rigidbody.mass = _density * _collector.CurrentVolume;
            _movementForce = (_rigidbody.mass * _maxSpeed) / _accelerationTime;
        }

        private void StartMovement()
        {
            if (_stopCoroutine != null)
            {
                StopCoroutine(_stopCoroutine);
            }

            _isMoving = true;
        }

        private void Move(Vector2 offset)
        {
            offset = offset.normalized;
            _cameraForward = _cameraTransform.forward;
            _cameraRight = _cameraTransform.right;

            _cameraForward.y = 0f;
            _cameraRight.y = 0f;

            _direction = _cameraForward * offset.y + _cameraRight * offset.x;
        }

        private void StopMovement()
        {
            if (_stopCoroutine != null)
            {
                StopCoroutine(_stopCoroutine);
            }

            _stopCoroutine = StartCoroutine(Stopping());
            _isMoving = false;
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