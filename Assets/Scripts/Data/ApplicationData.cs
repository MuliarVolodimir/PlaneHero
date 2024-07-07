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

    private int _defeatedEnemies = 200;
    private int _defeatedBosses = 10;
    private int _gameLevel = 1;
    private int _expCount = 0;
    private int _expToTheNextLvl = 100;
    private List<string> _comletedMissions = new List<string>();

    public event Action<int, int> OnExpChanged;
    public event Action<int, int> OnResourcesChanged;

    public void AddExp(int exp)
    {
        _expCount += exp;

        if (_expCount >= _expToTheNextLvl)
        {
            var expTemp = Math.Abs(_expToTheNextLvl - _expCount);
            _expCount = expTemp;
            _gameLevel++;
        }
        OnExpChanged?.Invoke(_expCount, _gameLevel);
    }

    public (int, int) GetExp()
    {
        return (_expCount, _gameLevel);
    }

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

    public bool IsPlaneUnlocked(string name)
    {
        return _unlockedPlanes.Contains(name);
    }

    public void SetPlane(string name)
    {
        _curSelectedPlane = name;
    }

    public void AddEnemiesBosses(int enemies, int bosses)
    {
        _defeatedEnemies += enemies;
        _defeatedBosses += bosses;
    }

    public int GetDefeatedEnemies()
    {
        return _defeatedEnemies;
    }

    public int GetDefeatedBosses()
    {
        return _defeatedBosses;
    }

    public void ResetProgress()
    {
        _defeatedBosses = 0;
        _defeatedEnemies = 0;
        _comletedMissions.Clear();
    }

    public bool IsMissionCompleted(string missionName)
    {
        bool isCompleted = _comletedMissions.Contains(missionName);
        return isCompleted;
    }

    public void CompleteMission(string missionName)
    {
        if (IsMissionCompleted(missionName) == false)
        {
            _comletedMissions.Add(missionName);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Coins", _resources[RESOURCE_COIN_ID].Count);
        PlayerPrefs.SetInt("Crowbars", _resources[RESOURCE_CROWBAR_ID].Count);

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
}

[Serializable]
public class EconomicResource
{
    public string Name;
    public int Count;
}
