using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Button _closeButton;

	private void OnEnable()
	{
        _closeButton.onClick.AddListener(Close);
	}

	private void OnDisable()
	{
		_closeButton.onClick.RemoveListener(Close);
	}

	public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
