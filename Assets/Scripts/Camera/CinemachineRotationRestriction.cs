using Cinemachine;
using UnityEngine;

namespace BallCollector.CameraControl
{
    [ExecuteAlways]
    [SaveDuringPlay]
    public class CinemachineRotationRestriction : CinemachineExtension
    {
        [SerializeField] private float _minXValue, _maxXValue;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            var euler = state.RawOrientation.eulerAngles;


            euler.x = Mathf.Clamp(euler.x, _minXValue, _maxXValue);

            state.RawOrientation = Quaternion.Euler(euler);
        }
    }
}