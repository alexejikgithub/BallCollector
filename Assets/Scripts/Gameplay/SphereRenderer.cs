using BallCollector.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallCollector.Gameplay
{
	public class SphereRenderer : MonoBehaviour
	{
		[SerializeField] private MeshRenderer _renderer;


		public void SetSkin(SphereSkin skin)
		{
			_renderer.material = skin.SkinMaterial;
		}
	}
}