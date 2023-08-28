using BallCollector.CoreSystem;
using UnityEngine;
using Zenject;


namespace BallCollector.Installers
{
    public class GameManagerInstaller : MonoInstaller
    {
        [SerializeField] private GameManager _GameManager;

        public override void InstallBindings()
        {
            var gameManagerInstance = Container.InstantiatePrefabForComponent<GameManager>(_GameManager);
            Container.Bind<GameManager>().FromInstance(gameManagerInstance).AsSingle();
        }
    }
}