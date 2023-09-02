using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private List<LevelButton> _levelButtons;

    private int _selectedLevel;

    private void OnEnable()
    {
        for (var i = 0; i < _levelButtons.Count; i++)
        {
            _levelButtons[i].LevelSelected += OnLevelSelected;
            _levelButtons[i].SetIndex(i);
            _levelButtons[i].Unselect();
        }
    }

    private void OnDisable()
    {
        for (var i = 0; i < _levelButtons.Count; i++)
        {
            _levelButtons[i].LevelSelected -= OnLevelSelected;
            _levelButtons[i].SetIndex(i);
        }
    }
    
    public int GetSelectedLevel()
    {
        return _selectedLevel;
    }

    private void OnLevelSelected(LevelButton button)
    {
        _levelButtons[_selectedLevel].Unselect();
        button.Select();
        _selectedLevel = button.Index;
    }
}