using BallCollector.CoreSystem;
using UnityEngine;
using Zenject;

namespace BallCollector.Installers
{
    public class LevelManagerInstaller : MonoInstaller
    {
        [SerializeField] private LevelManager _levelManager;
        public override void InstallBindings()
        {
            var inputFacadeInstance = Container.InstantiatePrefabForComponent<LevelManager>(_levelManager);
            Container.Bind<LevelManager>().FromInstance(inputFacadeInstance).AsSingle();
        }
    }
}