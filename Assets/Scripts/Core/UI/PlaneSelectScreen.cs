using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaneSelectScreen : MonoBehaviour
{
    [SerializeField] Image _curPlaneImage;

    [SerializeField] Button _checkButton;
    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;

    [SerializeField] PopupScreen _popupScreen;

    [SerializeField] AudioClip _aplayClip;

    private List<Item> _planeItems;
    private int _itemIndex;
    private Item _curPlane;

    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;
        _planeItems = _appData.GetPlanes();

        _checkButton.onClick.AddListener(() => { CheckWeapon(); });
        _leftButton.onClick.AddListener(() => { MoveLeft(); });
        _rightButton.onClick.AddListener(() => { MoveRight(); });

        UpdateView(_appData.GetPlane());
    }

    private void OnEnable()
    {
        if (_appData != null)
        UpdateView(_appData.GetPlane());
    }

    private void UpdateView(string selectedPlane)
    {
        _curPlane = _planeItems.Find(plane => plane.Name == selectedPlane);
        _itemIndex = _planeItems.IndexOf(_curPlane);

        if (_curPlaneImage != null)
        {
            _curPlaneImage.sprite = _curPlane.Sprite;
        }

        UpdateGraphics();
    }

    private void CheckWeapon()
    {
        AudioManager.Instance?.PlayOneShotSound(_aplayClip);

        if (_appData.IsPlaneUnlocked(_curPlane.Name))
        {
            Select();
        }
        else
        {
            Buy();
        }
    }

    private void Select()
    {
        _appData.SetPlane(_curPlane.Name);
        UpdateView(_appData.GetPlane());
    }

    private void Buy()
    {
        if (_appData.GetCoins() >= _curPlane.Price)
        {
            _appData.AddResourceCoin(-_curPlane.Price);
            _appData.UnlockPlane(_curPlane.Name);
            UpdateView(_appData.GetPlane());
        }
        else
        {
            _popupScreen?.ShowMessage("NOT ENOUGH TICKETS!");
        }

        UpdateGraphics();
    }

    private void MoveLeft()
    {
        AudioManager.Instance?.PlayOneShotSound(_aplayClip);
        _itemIndex--;
        if (_itemIndex < 0) _itemIndex = _planeItems.Count - 1;
        UpdateGraphics();
    }

    private void MoveRight()
    {
        AudioManager.Instance?.PlayOneShotSound(_aplayClip);
        _itemIndex++;
        if (_itemIndex >= _planeItems.Count) _itemIndex = 0;
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        _curPlane = _planeItems[_itemIndex];
        if (_curPlaneImage != null)
        {
            _curPlaneImage.sprite = _planeItems[_itemIndex].Sprite;
        }
        if (_checkButton != null)
        {
            var textMesh = _checkButton.GetComponentInChildren<TextMeshProUGUI>();
            if (textMesh != null)
            {
                if (_appData.IsPlaneUnlocked(_curPlane.Name))
                {
                    textMesh.text = "SELECT";
                }
                else
                {
                    textMesh.text = _planeItems[_itemIndex].Price.ToString();
                }
            }
        }
    }
}
