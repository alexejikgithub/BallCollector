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
    [Space]
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Sprite _levelDoneImage;
    [SerializeField] private Sprite _levelInProgressImage;
    [SerializeField] private Sprite _levelBlockedImage;

	[Space]
	[SerializeField] private int _index;
#if UNITY_EDITOR
	[SerializeField] private GameObject _leftPathEditorOnly;
    [SerializeField] private GameObject _rightPathEditorOnly;
#endif
	public int Index => _index;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonPressed);
    }

   

    public void SetButtonImage(int value)
    {
        Debug.Log(value);
        switch (value)
        {
            case < 0:
                _buttonImage.sprite = _levelDoneImage;
                break;
            case > 0:
                _buttonImage.sprite = _levelBlockedImage;
                break;
            default:
                _buttonImage.sprite = _levelInProgressImage;
                break;
        }
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


#if UNITY_EDITOR


    public void InitializeButton(int index, int maxIndex)
    {
        SetIndex(index);
        var indexDifference = maxIndex - _index;
        if(indexDifference < maxIndex)
        {
			_leftPathEditorOnly.SetActive(true);
		}
        if(indexDifference > 1) 
        {
			_rightPathEditorOnly.SetActive(true);
		}
	}
	public void SetIndex(int index)
	{
		_index = index;
		_text.text = (index + 1).ToString();
	}

#endif
}

