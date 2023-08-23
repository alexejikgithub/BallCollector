using BallCollector.CoreSystem;
using UnityEngine;
using Zenject;



namespace BallCollector.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Player _player;

        public override void InstallBindings()
        {
            Container.Bind<Player>().FromInstance(_player).AsSingle().NonLazy();
        }
    }
}