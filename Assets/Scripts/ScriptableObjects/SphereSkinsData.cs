using System;
using UnityEngine;

namespace BallCollector.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SphereSkinsData", menuName = "ScriptableObjects/SphereSkinsData", order = 2)]
    public class SphereSkinsData : ScriptableObject
	{
        [SerializeField] private SphereSkin[] _skins;
    }

    [Serializable]
    public class SphereSkin
    {
        [SerializeField] private Material _skinMaterial;
        [SerializeField] private bool _isUnlocked;

        public Material SkinMaterial => _skinMaterial;
        public bool IsUnlocked => _isUnlocked;

        public void Unlock()
        {
            _isUnlocked = true;
		}
    }
}