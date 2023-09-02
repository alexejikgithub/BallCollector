using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public event Action<LevelButton> LevelSelected = delegate { };

    [SerializeField] private Button _button;
    [SerializeField] private GameObject _backLight;
    [SerializeField] private TextMeshProUGUI _text;

    private int _index;

    public int Index => _index;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonPressed);
    }

    public void SetIndex(int index)
    {
        _index = index;
        _text.text = (index + 1).ToString();
    }

    public void Select()
    {
        _backLight.SetActive(true);
    }

    public void Unselect()
    {
        _backLight.SetActive(false);
    }

    private void OnButtonPressed()
    {
        LevelSelected.Invoke(this);
    }
}