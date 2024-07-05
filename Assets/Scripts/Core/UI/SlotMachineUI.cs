using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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

    [SerializeField] float resultDelay = 1.0f;  // Delay before showing the result

    private void Start()
    {
        _slotMachineSystem.InitializeSlots(_slotItems);
        _slotMachineSystem.OnSpinEnd += HandleSpinResult;
        _spinButton.onClick.AddListener(Spin);
        UpdateSlotVisuals(new List<int>() { 0, 2, 1 });
    }

    private void Spin()
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        _backButton.interactable = false;
        _spinButton.interactable = false;
        _slotMachineSystem.SpinSlots();
    }

    private void HandleSpinResult(bool isWin, List<int> slotValues, int reward, Sprite sprite)
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
            _popupScreen.ShowMessage("NOT ENOUGH COINS!");
            _spinButton.interactable = true;
            _backButton.interactable = true;
            return;
        }

        StartCoroutine(ShowResultAfterDelay(isWin, reward, sprite));
    }

    private IEnumerator ShowResultAfterDelay(bool isWin, int reward, Sprite sprite)
    {
        yield return new WaitForSeconds(resultDelay);

        if (isWin)
        {
            AudioManager.Instance.PlayOneShotSound(_winClip);
            GameObject particle = Instantiate(_winParticle, _particlePos);
            Destroy(particle, _winClip.length);
            _popupScreen.ShowReward(sprite, $"+{reward}!");
        }
        else
        {
            AudioManager.Instance.PlayOneShotSound(_loseClip);
            _popupScreen.ShowMessage("TRY AGAIN!");
        }

        _backButton.interactable = true;
        _spinButton.interactable = true;
    }

    public void UpdateSlotVisuals(List<int> slotValues)
    {
        for (int i = 0; i < slotValues.Count; i++)
        {
            _slotPlaces[i].sprite = _slotItems[slotValues[i]].Sprite;
        }
    }
}
