using UnityEngine;
using Zenject;

namespace BallCollector.Installers
{
    public class InputInstaller : MonoInstaller
    {
        [SerializeField] private InputFacade _inputFacadePrefab;
        public override void InstallBindings()
        {
            var inputFacadeInstance = Container.InstantiatePrefabForComponent<InputFacade>(_inputFacadePrefab);
            Container.Bind<InputFacade>().FromInstance(inputFacadeInstance).AsSingle();
        }
    }
}