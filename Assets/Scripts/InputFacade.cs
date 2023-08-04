using System;
using UnityEngine;

public class InputFacade : MonoBehaviour
{
    public event Action DownTouched;
    public event Action UpTouched;
    public event Action<Vector2> MouseDeltaChanged; 
    public event Action<Vector2> MouseOffsetChanged;

    [SerializeField] private float _sensitivity = 1f;

    private Vector3 _startPoint;
    private Vector3 _lastPosition;
    private Vector2 _delta;
    private Vector2 _offset;
    
    public bool IsTouch => Input.GetMouseButton(0);
    private Vector2 MouseOffset() => _offset * _sensitivity;
    private Vector2 MouseDelta() => _delta * _sensitivity;

    private void Update()
    {
        var mousePositionScreen = UnityEngine.Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            _startPoint = mousePositionScreen;

            _lastPosition = mousePositionScreen;
            DownTouched?.Invoke();
        }

        if (Input.GetMouseButton(0))
        {
            _offset = mousePositionScreen - _startPoint;
            _delta = (mousePositionScreen - _lastPosition);
            
            _lastPosition = mousePositionScreen;
            
            MouseDeltaChanged?.Invoke(MouseDelta());
            MouseOffsetChanged?.Invoke(MouseOffset());
        }

        if (Input.GetMouseButtonUp(0))
        {
            _delta = Vector2.zero;
            _offset = Vector2.zero;
                
            UpTouched?.Invoke();
        }
    }
}
