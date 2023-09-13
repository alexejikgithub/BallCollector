using BallCollector.CoreSystem;
using BallCollector.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace BallCollector.Gameplay
{
	public class SphereRenderer : MonoBehaviour
	{
		[SerializeField] private MeshRenderer _renderer;

		[Inject] private SkinManager _skinManager;
		private void OnEnable()
		{
			_skinManager.SkinChanged += SetSkin;
			SetSkin(_skinManager.GetSkin());

		}
		private void OnDisable()
		{
			_skinManager.SkinChanged -= SetSkin;
		}

		public void SetSkin(SphereSkin skin)
		{
			_renderer.material = skin.SkinMaterial;
		}
	}
}