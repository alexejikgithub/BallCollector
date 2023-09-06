using BallCollector.Gameplay;
using UnityEngine;

namespace BallCollector.CoreSystem
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private SphereMovement _sphereMovement;
		[SerializeField] private Collector _collector;
		[SerializeField] private SphereRenderer _sphereRenderer;

		public SphereMovement SphereMovement => _sphereMovement;
		public Collector Collector => _collector;
		public SphereRenderer SphereRenderer => _sphereRenderer;
	}
}
