using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SlotMachineUI : MonoBehaviour
{
    [SerializeField] SlotMachineSystem _slotMachineSystem;
    [SerializeField] List<Image> _slotPlaces;
    [SerializeField] List<Item> _slotItems;
    [SerializeField] Button _spinButton;

    [SerializeField] PopupScreen _popupScreen;
    [SerializeField] Transform _particlePos;
    [SerializeField] GameObject _winParticle;
    [SerializeField] Button _backButton;

    [SerializeField] AudioClip _winClip;
    [SerializeField] AudioClip _loseClip;
    [SerializeField] AudioClip _applyClip;

    private void Start()
    {
        _slotMachineSystem.OnSpinEnd += HandleSpinResult;
        _spinButton.onClick.AddListener(Spin);
    }

    private void Spin()
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        _backButton.interactable = false;
        _slotMachineSystem.SpinSlots();
    }

    private void HandleSpinResult(bool isWin, List<int> slotValues, int reward)
    {
        if (slotValues != null)
        {
            for (int i = 0; i < slotValues.Count; i++)
            {
                _slotPlaces[i].sprite = _slotItems[slotValues[i]].Sprite;
            }
        }
        else
        {
            _popupScreen.ShowMessage("NOT ENOUGHT COINS!");
        }

        if (isWin)
        {
            AudioManager.Instance.PlayOneShotSound(_winClip);
            GameObject particle = Instantiate(_winParticle, _particlePos);
            Destroy(particle, _winClip.length);
        }
        else
        {
            AudioManager.Instance.PlayOneShotSound(_loseClip);
            _popupScreen.ShowMessage("TRY AGAIN!");
        }

        _backButton.interactable = true;
    }
}
