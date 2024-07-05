using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupScreen : MonoBehaviour
{
    [SerializeField] Image _itemImage;
    [SerializeField] Button _confirmButton;
    [SerializeField] TextMeshProUGUI _messageText;
    [SerializeField] AudioClip _aplayClip;

    private void Start()
    {
        _itemImage.gameObject.SetActive(false);
        gameObject.SetActive(false);
        _confirmButton.onClick.AddListener(() => { Close(); });
    }

    private void Close()
    {
        AudioManager.Instance.PlayOneShotSound(_aplayClip);
        gameObject.SetActive(false);
        _itemImage.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        gameObject.SetActive(true);
        _itemImage.gameObject.SetActive(false);
        _messageText.text = message;
    }

    public void ShowReward(Sprite sprite, string message)
    {
        _itemImage.gameObject.SetActive(true);
        _itemImage.sprite = sprite;
        _messageText.text = message;
    }
}