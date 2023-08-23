using System;
using TMPro;
using UnityEngine;

namespace BallCollector.CoreSystem
{
    public class InputFacade : MonoBehaviour
    {
        public event Action DownTouched;
        public event Action UpTouched;
        public event Action<Vector2> MouseDeltaChanged;
        public event Action<Vector2> MouseOffsetChanged;
        public event Action<Vector2> GyroOffsetChanged;
        
        [SerializeField] private float _sensitivity = 1f;

        private Vector3 _startPoint;
        private Vector3 _lastPosition;
        private Vector2 _delta;
        private Vector2 _offset;

        private Vector3 _startGyroAltitude;
        private Vector3 _gyroOffset;

        public bool IsTouch => Input.GetMouseButton(0);
        private Vector2 MouseOffset() => _offset * _sensitivity;
        private Vector2 MouseDelta() => _delta * _sensitivity;
        private Vector2 GyroDelta() => new Vector2(_gyroOffset.x, _gyroOffset.y
        ) * _sensitivity;
        

        private void Start()
        {
            Input.gyro.enabled = true;
        }

        private void Update()
        {
            GyroModifyCamera();
            var mousePositionScreen = Input.mousePosition;
            var gyroAltitude = Input.acceleration;

            if (Input.GetMouseButtonDown(0))
            {
                _startPoint = mousePositionScreen;
                _startGyroAltitude = gyroAltitude;
                
                _lastPosition = mousePositionScreen;
                DownTouched?.Invoke();
            }

            if (Input.GetMouseButton(0))
            {
                _offset = mousePositionScreen - _startPoint;
                _delta = (mousePositionScreen - _lastPosition);

                _gyroOffset = gyroAltitude - _startGyroAltitude; 
                
                _lastPosition = mousePositionScreen;

                MouseDeltaChanged?.Invoke(MouseDelta());
                MouseOffsetChanged?.Invoke(MouseOffset());
                GyroOffsetChanged?.Invoke(GyroDelta());
                
            }

            if (Input.GetMouseButtonUp(0))
            {
                _delta = Vector2.zero;
                _offset = Vector2.zero;

                UpTouched?.Invoke();
            }
        }
        
        void GyroModifyCamera()
        {
            transform.rotation = GyroToUnity(Input.gyro.attitude);
        }
        
        private static Quaternion GyroToUnity(Quaternion q)
        {
            return new Quaternion(q.x, q.y, -q.z, -q.w);
        }
    }
}