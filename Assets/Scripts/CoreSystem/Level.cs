using System;
using BallCollector.CameraControl;
using BallCollector.Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace BallCollector.CoreSystem
{
    public class Level : MonoBehaviour
    {
        [Header("Camera")] 
        [SerializeField] CameraController _cameraController;
[Space]

        [SerializeField] private CollectableItemsContainer _itemsContainer;

[Space] [Inject] private Player _player;
        [SerializeField] private Vector3 _ballStartPosition;
        [SerializeField] private float _ballStartDiameter;


        [SerializeField] private Timer _timer;
        [SerializeField] private int _baseTime;

        [SerializeField] private float _targetRadius;
        
        
        

        private void Awake()
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

            if (_targetRadius > _player.Collector.Collider.radius)
            {
                Debug.Log("you lose");
            }
            else
            {
                Debug.Log("you win");
            }
        }

        private void OnTimerSecondElapsed(int secondsLeft)
        {
            Debug.Log(secondsLeft);
        }


    }
}