using BallCollector.CoreSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _ShopButton;
    [Space]
    [SerializeField] private LevelSelector _levelSelector;
    [SerializeField] private Shop _shop;

    [Inject] private GameManager _gameManager;
    private void OnEnable()
    {
        _playButton.onClick.AddListener(ChoseLevel);
		_ShopButton.onClick.AddListener(OpenShop);

	}
	private void OnDisable()
	{
		_playButton.onClick.RemoveListener(ChoseLevel);
		_ShopButton.onClick.RemoveListener(OpenShop);
	}

	private void ChoseLevel()
    {
        Debug.Log(_levelSelector.GetSelectedLevel());
        _gameManager.ChoseLevel(_levelSelector.GetSelectedLevel());
        _gameManager.PlayChosenLevel();
    }

    private void OpenShop()
    {
		_shop.Open();
	}
}
