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


        [Inject] private InputFacade _inputFacade;

        private Transform _cameraTransform;
        private Vector3 _direction = Vector3.zero;
        private Vector3 _cameraForward;
        private Vector3 _cameraRight;

        private float _currentSpeed;

        private Coroutine _stopCoroutine;

        private void Start()
        {
            EnableMovement();
        }

        public void EnableMovement()
        {
            _cameraTransform = Camera.main.transform; //TODO remake dependency 
            _inputFacade.DownTouched += StartMovement;
            _inputFacade.MouseOffsetChanged += Move;
            _inputFacade.UpTouched += StopMovement;
        }

        public void DisableMovement()
        {
            _inputFacade.DownTouched -= StartMovement;
            _inputFacade.MouseOffsetChanged -= Move;
            _inputFacade.UpTouched -= StopMovement;
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