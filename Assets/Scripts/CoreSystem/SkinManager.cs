using BallCollector.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace BallCollector.CoreSystem
{
    public class SkinManager : MonoBehaviour
    {
        [SerializeField] private SphereSkinsData _sphereSkinsData;

		private const string _selectedSkinIndex = "SelectedSkinIndex";

        public void SetSelectedSkinIndex(int index)
        {
            if(index<_selectedSkinIndex.Length)
            {
				PlayerPrefs.SetInt(_selectedSkinIndex, index);
			}
        }
		public int GetSelectedSkinIndex()
		{
			return PlayerPrefs.GetInt(_selectedSkinIndex);
		}
	}
}