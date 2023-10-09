using BallCollector.ScriptableObjects;
using System;
using UnityEngine;

namespace BallCollector.CoreSystem
{
    public class SkinManager : MonoBehaviour
    {
		public event Action<SphereSkin> SkinChanged = delegate { };


		[SerializeField] private SphereSkinsData _sphereSkinsData;

		private const string _selectedSkinIndex = "SelectedSkinIndex";

        public void SetSelectedSkinIndex(int index)
        {
            if(index<_selectedSkinIndex.Length)
            {
				PlayerPrefs.SetInt(_selectedSkinIndex, index);
				SkinChanged.Invoke(GetSkin());
			}
        }
		public int GetSelectedSkinIndex()
		{
			return PlayerPrefs.GetInt(_selectedSkinIndex);
		}

		public SphereSkin GetSkin()
		{
			return _sphereSkinsData.Skins[GetSelectedSkinIndex()];
		}
	}
}