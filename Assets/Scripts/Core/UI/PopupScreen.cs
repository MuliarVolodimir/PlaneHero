using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupScreen : MonoBehaviour
{
    [SerializeField] CanvasGroup _screenVisibility;
    [SerializeField] Image _itemImage;
    [SerializeField] Button _confirmButton;
    [SerializeField] TextMeshProUGUI _messageText;
    [SerializeField] AudioClip _aplayClip;

    public event Action OnConfirm;

    private void Start()
    {
        _itemImage.gameObject.SetActive(false);
        _confirmButton.onClick.AddListener(Close);
        _screenVisibility.alpha = 0;
        _screenVisibility.blocksRaycasts = false;
    }

    private void Close()
    {
        _screenVisibility.alpha = 0;
        OnConfirm?.Invoke();
        AudioManager.Instance.PlayOneShotSound(_aplayClip);
        _itemImage.gameObject.SetActive(false);
        _screenVisibility.blocksRaycasts = false;
    }

    public void ShowMessage(string message)
    {
        _screenVisibility.alpha = 1;
        _screenVisibility.blocksRaycasts = true;
        _itemImage.gameObject.SetActive(false);
        _messageText.text = message;
    }

    public void ShowReward(Sprite sprite, string message)
    {
        _screenVisibility.alpha = 1;
        _screenVisibility.blocksRaycasts = true;
        _itemImage.gameObject.SetActive(true);
        _itemImage.sprite = sprite;
        _messageText.text = message;
    }
}
