using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineSystem : MonoBehaviour
{
    [SerializeField] int _slotCount;
    [SerializeField] int _spinPrice;
    [SerializeField] int _rewardCoin;
    [SerializeField] int _rewardCrowbar;

    [SerializeField] AudioClip _spinClip;

    public bool IsSpinning { get; private set; } = false;

    public delegate void SpinResultHandler(bool isWin, List<int> slotValues, int reward, Sprite sprite);
    public event SpinResultHandler OnSpinEnd;

    private float _spinDuration;
    private List<int> _currentSlotValues;
    private List<Item> _slotItems;

    private ApplicationData _appData;
    private SlotMachineUI _slotMachineUI;

    private void Awake()
    {
        _slotMachineUI = FindObjectOfType<SlotMachineUI>();
    }

    public void InitializeSlots(List<Item> items)
    {
        _appData = ApplicationData.Instance;
        _slotItems = items;
        _currentSlotValues = new List<int>();

        for (int i = 0; i < _slotCount; i++)
        {
            int randomIndex = Random.Range(0, _slotItems.Count);
            _currentSlotValues.Add(randomIndex);
        }
    }

    public void SpinSlots()
    {
        if (IsSpinning) return;

        var coins = _appData.GetCoins();

        if (coins >= _spinPrice)
        {
            _appData.AddResourceCoin(-_spinPrice);
            IsSpinning = true;
            _spinDuration = 3;
            StartCoroutine(SpinCoroutine());
        }
        else
        {
            OnSpinEnd?.Invoke(false, null, 0, null);
        }
    }

    private IEnumerator SpinCoroutine()
    {
        AudioManager.Instance.PlayOneShotSound(_spinClip);
        float elapsedTime = 0f;
        float interval = 0.1f;

        while (elapsedTime < _spinDuration)
        {
            for (int i = 0; i < _slotCount; i++)
            {
                int randomIndex = Random.Range(0, _slotItems.Count);
                _currentSlotValues[i] = randomIndex;
            }

            _slotMachineUI.UpdateSlotVisuals(_currentSlotValues);

            elapsedTime += interval;
            yield return new WaitForSeconds(interval);
        }

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        bool isWin = true;

        for (int i = 1; i < _slotCount; i++)
        {
            if (_currentSlotValues[i] != _currentSlotValues[0])
            {
                isWin = false;
                break;
            }
        }

        int reward = 0;
        Sprite rewardSprite = null;

        if (isWin)
        {
            var winningItem = _slotItems[_currentSlotValues[0]];

            if (winningItem.Name == "Crowbar")
            {
                reward = _rewardCrowbar;
                _appData.AddResourceCrowbar(reward);
                rewardSprite = winningItem.Sprite;
            }
            else
            {
                reward = _rewardCoin;
                _appData.AddResourceCoin(reward);
                rewardSprite = winningItem.Sprite;
            }
        }

        OnSpinEnd?.Invoke(isWin, _currentSlotValues, reward, rewardSprite);
        IsSpinning = false;
    }
}
