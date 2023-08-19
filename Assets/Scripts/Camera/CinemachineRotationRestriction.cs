using Cinemachine;
using UnityEngine;

namespace BallCollector.CameraControl
{
    [ExecuteAlways]
    [SaveDuringPlay]
    public class CinemachineRotationRestriction : CinemachineExtension
    {
        [SerializeField] private float _minXValue, _maxXValue;
        [Space]
        [SerializeField] private float _minYValue, _maxYValue;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            var euler = state.RawOrientation.eulerAngles;


            euler.x = Mathf.Clamp(euler.x, _minXValue, _maxXValue);
            euler.y = Mathf.Clamp(euler.y, _minYValue, _maxYValue);

            state.RawOrientation = Quaternion.Euler(euler);
        }
    }
}