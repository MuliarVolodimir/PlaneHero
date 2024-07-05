using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationData
{ 
    private const int RESOURCE_COIN_ID = 0;
    private const int RESOURCE_CROWBAR_ID = 1;

    private static ApplicationData _instance;

    public static ApplicationData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ApplicationData();
            }
            return _instance;
        }
    }

    

    // Settings
    public bool IsMusicMute = false;
    public bool IsEffectsMute = false;

    // Data
    private List<Item> _planes;
    private List<EconomicResource> _resources = new List<EconomicResource>();
    private string _curSelectedPlane = "StartPlane";
    private List<string> _unlockedPlanes = new List<string> { "StartPlane" };

    private int _gameLevel = 1;
    private int _expCount = 0;

    public event Action<int, int> OnExpChanged;
    public event Action<int, int> OnResourcesChanged;

    public void InitPlanes(List<Item> plane)
    {
        _planes = plane;
    }

    public List<Item> GetPlanes()
    {
        return _planes;
    }

    public void UnlockPlane(string name)
    {
        _unlockedPlanes.Add(name);
        _curSelectedPlane = name; 
    }

    public string GetPlane()
    {
        return _curSelectedPlane;
    }

    public void InitResources(List<EconomicResource> resources)
    {
        _resources = new List<EconomicResource>(resources);
    }

    public void AddResourceCoin(int amount)
    {
        if (_resources != null)
        {
            _resources[RESOURCE_COIN_ID].Count += amount;
            OnResourcesChanged?.Invoke(_resources[RESOURCE_COIN_ID].Count, _resources[RESOURCE_CROWBAR_ID].Count);
        }
    }

    public void AddResourceCrowbar(int amount)
    {
        if (_resources != null)
        {
            _resources[RESOURCE_CROWBAR_ID].Count += amount;
            OnResourcesChanged?.Invoke(_resources[RESOURCE_COIN_ID].Count, _resources[RESOURCE_CROWBAR_ID].Count);
        }
    }

    public int GetCoins()
    {
        return _resources.Count > RESOURCE_COIN_ID ? _resources[RESOURCE_COIN_ID].Count : 0;
    }

    public int GetCrowbars()
    {
        return _resources.Count > RESOURCE_CROWBAR_ID ? _resources[RESOURCE_CROWBAR_ID].Count : 0;
    }

    public void AddLevel(int expCount)
    {
        _expCount += expCount;
        if (_expCount >= 100) // 100 - exp to next lvl
        {
            _gameLevel++;
        }

        OnExpChanged?.Invoke(_gameLevel, _expCount);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Coins", _resources[RESOURCE_COIN_ID].Count);
        PlayerPrefs.SetInt("Crowbar", _resources[RESOURCE_CROWBAR_ID].Count);

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            _resources[RESOURCE_COIN_ID].Count = PlayerPrefs.GetInt("Coins");
        }
        if (PlayerPrefs.HasKey("Crowbars"))
        {
            _resources[RESOURCE_CROWBAR_ID].Count = PlayerPrefs.GetInt("Crowbars");
        }
    }

    public bool IsPlaneUnlocked(string name)
    {
        if (_unlockedPlanes.Contains(name))
            return true;
        else
            return false;
    }

    public void SetPlane(string name)
    {
        _curSelectedPlane = name;
    }
}

[Serializable]
public class EconomicResource
{
    public string Name;
    public int Count;
}