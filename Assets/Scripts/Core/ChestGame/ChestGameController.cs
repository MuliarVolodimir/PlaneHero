using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestGameController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _chestCountsText;
    [SerializeField] List<ChestItem> _chestItems;
    [SerializeField] Button _chestButton;
    [SerializeField] int _chestCount;
    [SerializeField] int _tapCount;

    [SerializeField] PopupScreen _popupScreen;
    [SerializeField] Animator _animator;

    [SerializeField] GameObject _particle;
    [SerializeField] List<AudioClip> _tapChestClips;
    [SerializeField] AudioClip _openChestClip;

    private ApplicationData _appData = ApplicationData.Instance;
    private bool _isInteractive = false;
    private int _maxTapCounts;

    public void Initialize(int chestCount)
    {
        _maxTapCounts = _tapCount;
        _chestButton.onClick.AddListener(OnTap);
        _chestCount = chestCount;
        _chestCountsText.text = $"x{_chestCount}";

        _chestCountsText.text = $"x{_chestCount}";
        _isInteractive = true;
    }

    public void OnTap()
    {
        var index = UnityEngine.Random.Range(0, _tapChestClips.Count);
        AudioManager.Instance.PlayOneShotSound(_tapChestClips[index]);
        _animator.ResetTrigger("Tap");
        _animator.SetTrigger("Tap");

        _tapCount--;

        if (_tapCount < 1 && _isInteractive)
        {
            _isInteractive = false;
            _chestButton.gameObject.SetActive(false);

            _chestCount--;
            
            _tapCount = _maxTapCounts;
            StartCoroutine(OpenChest());
        }
    }

    private IEnumerator OpenChest()
    {
        AudioManager.Instance.PlayOneShotSound(_openChestClip);
        GameObject particle = Instantiate(_particle, transform.position, transform.rotation);
        Destroy(particle, _openChestClip.length);
        yield return new WaitForSeconds(1f);

        var index = UnityEngine.Random.Range(0, _chestItems.Count);
        switch (_chestItems[index].Type)
        {
            case ChestItem.RewardType.Cion:
                _appData.AddResourceCoin(_chestItems[index].Reward);
                break;
            case ChestItem.RewardType.Crowbar:
                _appData.AddResourceCrowbar(_chestItems[index].Reward);
                break;
            default:
                break;
        }

        Debug.Log("chestOpen");
        _popupScreen.ShowReward(_chestItems[index].Sprite, $"+{ _chestItems[index].Reward}");
        _popupScreen.OnConfirm += CheckOtherChests;
    }

    private void CheckOtherChests()
    {
        _chestCountsText.text = $"x{_chestCount}";
        if (_chestCount >= 0)
        {
            _chestButton.gameObject.SetActive(true);
            _isInteractive = true;
        }
        else if (_chestCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}

[Serializable]
public class ChestItem
{
    public int Reward;
    public Sprite Sprite;
    public RewardType Type;

    public enum RewardType
    {
        Cion,
        Crowbar
    }
}