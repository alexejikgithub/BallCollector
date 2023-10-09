using BallCollector.CoreSystem;
using UnityEngine;
using Zenject;
namespace BallCollector.Installers
{
    public class SkinManagerInstaller : MonoInstaller
    {
		[SerializeField] private SkinManager _skinManager;

		public override void InstallBindings()
		{
			var skinManagerInstance = Container.InstantiatePrefabForComponent<SkinManager>(_skinManager);
			Container.Bind<SkinManager>().FromInstance(skinManagerInstance).AsSingle();
		}
	}
}