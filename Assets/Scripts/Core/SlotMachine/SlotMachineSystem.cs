using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineSystem : MonoBehaviour
{
    [SerializeField] int _slotCount;
    [SerializeField] int _spinPrice;
    [SerializeField] int _reward;

    [SerializeField] AudioClip _spinClip;

    public bool IsSpinning { get; private set; } = false;

    public delegate void SpinResultHandler(bool isWin, List<int> slotValues, int reward);
    public event SpinResultHandler OnSpinEnd;

    private float _spinDuration;
    private List<int> _currentSlotValues;
    private List<Item> _slotItems;

    private ApplicationData _appData;

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
            _spinDuration = _spinClip.length;
            StartCoroutine(SpinCoroutine());
        }
        else
        {
            OnSpinEnd?.Invoke(false, null, 0);
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

        int reward = isWin ? _reward : 0;
        if (isWin)
        {
            _appData.AddResourceCoin(_reward);
        }

        OnSpinEnd?.Invoke(isWin, _currentSlotValues, reward);
        IsSpinning = false;
    }
}
