using System.Runtime.InteropServices;
using BallCollector.CameraControl;
using BallCollector.Gameplay;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace BallCollector.CoreSystem
{
    public class Level : MonoBehaviour
    {
        [Header("Camera")] [SerializeField] CameraController _cameraController;
        [Space] [SerializeField] private CollectableItemsContainer _itemsContainer;
        [Space] [SerializeField] private Timer _timer;
        [SerializeField] private int _baseTime;
        [SerializeField] private float _targetRadius;

        [Inject] private Player _player;
        [Inject] private LevelManager _levelManager;


        private void Start()
        {
            StartGameplay();
        }

        private void InitLevel()
        {
        }

        private void StartGameplay()
        {
            _timer.SecondElapsed += OnTimerSecondElapsed;
            _timer.TimerComplete += StopGameplay;
            _timer.StartTimer(_baseTime);
            _player.SphereMovement.EnableMovement();
        }

        private void StopGameplay()
        {
            Debug.Log("StopGameplay");

            _timer.SecondElapsed -= OnTimerSecondElapsed;
            _timer.TimerComplete -= StopGameplay;
            _player.SphereMovement.DisableMovement();
            DOVirtual.DelayedCall(3f, () =>

                {
                    if (_targetRadius > _player.Collector.Collider.radius)
                    {
                        Debug.Log("you lose");
                        _levelManager.ReloadLevel();
                    }
                    else
                    {
                        Debug.Log("you win");
                        _levelManager.LoadNextLevel();
                    }
                }
            );
        }

        private void OnTimerSecondElapsed(int secondsLeft)
        {
            Debug.Log(secondsLeft);
        }
    }
}