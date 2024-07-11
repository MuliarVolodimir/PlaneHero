using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] List<ShopItem> _crowbarItems;
    [SerializeField] List<ShopItem> _chestItems;
    [SerializeField] List<ShopItem> _coinItems;

    [SerializeField] GameObject _crowbarsContent;
    [SerializeField] GameObject _chestContent;
    [SerializeField] GameObject _coinsContent;

    [SerializeField] GameObject _shopItemIconInfo;
    [SerializeField] PopupScreen _popupScreen;
    [SerializeField] GameObject _chestGameController;
    [SerializeField] GameObject _chestSpawnPos;
    [SerializeField] AudioClip _applyClip;
    [SerializeField] AudioClip _errorClip;

    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance; 
        InitializeItems(_crowbarItems, _crowbarsContent);
        InitializeItems(_chestItems, _chestContent);
        InitializeItems(_coinItems, _coinsContent);
    }

    private void InitializeItems(List<ShopItem> items, GameObject content)
    {
        for (int i = 0; i < items.Count; i++)
        {
            int index = i;
            GameObject newItem = Instantiate(_shopItemIconInfo, content.transform);
            var newShopItemIconInfo = newItem.GetComponent<ShopItemIconInfo>();

            newShopItemIconInfo.SetInfo(items[index].RewardIcon, items[index].PriceIcon, items[index].Reward, items[index].Price);
            newItem.GetComponent<Button>().onClick.AddListener(() => { OnItemInfoClick(items[index].Price, items[index].Reward, items[index].Type); });
        }
    }

    private void OnItemInfoClick(int price, int reward, ShopItem.RewardType rewardType)
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        switch (rewardType)
        {
            case ShopItem.RewardType.Coin:
                BuyCoins(price, reward);
                break;
            case ShopItem.RewardType.Crowbar:
                BuyCrowbars(price, reward);
                break;
            case ShopItem.RewardType.Chest:
                BuyChests(price, reward);
                break;
            default:
                break;
        }
    }

    private void BuyChests(int price, int reward)
    {
        if (price <= _appData.GetCrowbars())
        {
            _appData.AddResourceCrowbar(-price);
            GameObject chestController = Instantiate(_chestGameController, _chestSpawnPos.transform);
            chestController.GetComponent<ChestGameController>().Initialize(reward);
        }
        else
        {
            AudioManager.Instance.PlayOneShotSound(_errorClip);
            _popupScreen.ShowMessage($"NOT ENOUGHT CROWBARS!");
        }
    }

    private void BuyCrowbars(int price, int reward)
    {
        if (price <= _appData.GetCoins())
        {
            _appData.AddResourceCoin(-price);
            _appData.AddResourceCrowbar(reward);
            _popupScreen.ShowReward(_crowbarItems[0].RewardIcon, $"+{reward}");
        }
        else
        {
            AudioManager.Instance.PlayOneShotSound(_errorClip);
            _popupScreen.ShowMessage($"NOT ENOUGHT COINS!");
        }
    }

    private void BuyCoins(int price, int reward)
    {
        _appData.AddResourceCoin(reward); // this only for testing
        /*
        here must been purchase system logic
        if (something)
        {
            _appData.AddResourceCoin(reward);
            _popupScreen.ShowReward(_coinItems[0].RewardIcon, $"+{reward}");
        }
        else
        {
            AudioManager.Instance.PlayOneShotSound(_errorClip);
            _popupScreen.ShowMessage($"PURCESE ERROR!");
        }
        */
    }
}

[Serializable]
public class ShopItem
{
    public int Price;
    public Sprite PriceIcon;
    public int Reward;
    public Sprite RewardIcon;
    public RewardType Type;

    public enum RewardType
    {
        Coin,
        Crowbar,
        Chest
    }
}