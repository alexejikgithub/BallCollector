using System.Collections.Generic;
using System.Linq;
using BallCollector.CoreSystem;
using UnityEngine;
using Zenject;

public class LevelSelector : MonoBehaviour
{
	[SerializeField] private List<LevelButton> _levelButtons;

	[Inject] private GameManager _gameManager;

	private int _selectedLevel;
	private const string _unlockedLevelIndex = "UnlockedLevelIndex";

#if UNITY_EDITOR
	[SerializeField] private LevelButton _levelButtonPrefabEditorOnly;
	[SerializeField] private GameObject _spacerPrefabEditorOnly;
	[SerializeField] private Transform _levelButtonParentEditorOnly;
	[SerializeField] private GameManager _gameManagerEditorOnly;

#endif

	private void OnEnable()
	{
		_selectedLevel = PlayerPrefs.GetInt(_unlockedLevelIndex);
		for (var i = 0; i < _levelButtons.Count; i++)
		{
			_levelButtons[i].LevelSelected += OnLevelSelected;
			_levelButtons[i].SetButtonImage(i - _selectedLevel);
			_levelButtons[i].Unselect();
		}

		_levelButtons[_selectedLevel].Select();
	}

	private void OnDisable()
	{
		for (var i = 0; i < _levelButtons.Count; i++)
		{
			_levelButtons[i].LevelSelected -= OnLevelSelected;
		}
	}

	public int GetSelectedLevel()
	{
		return _selectedLevel;
	}

	private void OnLevelSelected(LevelButton button)
	{
		if (button.Index > PlayerPrefs.GetInt(_unlockedLevelIndex))
			return;

		_levelButtons[_selectedLevel].Unselect();
		button.Select();
		_selectedLevel = button.Index;
	}

#if UNITY_EDITOR

	[ContextMenu("GenerateLevelSelector")]
	private void GenerateLevelSelector()
	{
		_levelButtons=new List<LevelButton>();
		var levelCount = _gameManagerEditorOnly.LevelsCount;

		List<Transform> childrenToDelete = new List<Transform>();
		foreach (Transform child in _levelButtonParentEditorOnly)
		{
			childrenToDelete.Add(child);
		}
		foreach (Transform childToDelete in childrenToDelete)
		{
			DestroyImmediate(childToDelete.gameObject);
		}

		Instantiate(_spacerPrefabEditorOnly,_levelButtonParentEditorOnly);
		for (var i = 0; levelCount > i; i++)
		{
			var button = Instantiate(_levelButtonPrefabEditorOnly, _levelButtonParentEditorOnly);
			button.InitializeButton(i, levelCount);
			_levelButtons.Add(button);
		}
		Instantiate(_spacerPrefabEditorOnly, _levelButtonParentEditorOnly);
	}


#endif

}