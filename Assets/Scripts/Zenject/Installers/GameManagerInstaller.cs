using BallCollector.CoreSystem;
using UnityEngine;
using Zenject;


namespace BallCollector.Installers
{
    public class GameManagerInstaller : MonoInstaller
    {
        [SerializeField] private GameManager _gameManager;

        public override void InstallBindings()
        {
            var gameManagerInstance = Container.InstantiatePrefabForComponent<GameManager>(_gameManager);
            Container.Bind<GameManager>().FromInstance(gameManagerInstance).AsSingle();
        }
    }
}