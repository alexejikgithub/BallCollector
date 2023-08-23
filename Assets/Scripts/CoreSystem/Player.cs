using BallCollector.Gameplay;
using UnityEngine;

namespace BallCollector.CoreSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private SphereMovement _sphereMovement;
        [SerializeField] private Collector _collector;

        public SphereMovement SphereMovement => _sphereMovement;

        public Collector Collector => _collector;
    }
}
