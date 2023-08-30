using BallCollector.CoreSystem;
using UnityEngine;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayButton _playButton;
    [SerializeField] private LevelSelector _levelSelector;

    [Inject] private GameManager _gameManager;
    private void OnEnable()
    {
        _playButton.LevelChosen += ChoseLevel;
    }


    private void ChoseLevel()
    {
        Debug.Log(_levelSelector.GetSelectedLevel());
        _gameManager.ChoseLevel(_levelSelector.GetSelectedLevel());
        _gameManager.PlayChosenLevel();

    }
}
