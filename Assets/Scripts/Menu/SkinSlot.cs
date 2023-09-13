using BallCollector.CoreSystem;
using BallCollector.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkinSlot : MonoBehaviour
{
	
	[SerializeField] int _skinIndex;
	[SerializeField] Button _skinButton;

	[Inject] SkinManager _manager;

	private void OnEnable()
	{
		_skinButton.onClick.AddListener(SetSkin);
	}

	private void OnDisable()
	{
		_skinButton.onClick.RemoveListener(SetSkin);
	}

	private void SetSkin()
	{
		_manager.SetSelectedSkinIndex(_skinIndex);
	}


}
