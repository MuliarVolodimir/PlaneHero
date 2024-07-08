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

    //start values
    private int _defeatedEnemies = 0;
    private int _defeatedBosses = 0;
    private int _gameLevel = 1;
    private int _bossLvlRate = 10;

    private List<string> _comletedMissions = new List<string>();

    public event Action<int> OnLvlChanged;
    public event Action<int, int> OnResourcesChanged;

    public void AddLevel()
    {
        _gameLevel++;

        OnLvlChanged?.Invoke(_gameLevel);
        SaveData();
    }

    public int GetExp()
    {
        return  _gameLevel;
    }

    public int GetBossLvlRate()
    {
        return _bossLvlRate;    
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
        SaveData();
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
            SaveData();
        }
    }

    public void AddResourceCrowbar(int amount)
    {
        if (_resources != null)
        {
            _resources[RESOURCE_CROWBAR_ID].Count += amount;
            OnResourcesChanged?.Invoke(_resources[RESOURCE_COIN_ID].Count, _resources[RESOURCE_CROWBAR_ID].Count);
            SaveData();
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
        SaveData();
    }

    public void AddEnemiesBosses(int enemies, int bosses)
    {
        _defeatedEnemies += enemies;
        _defeatedBosses += bosses;
        SaveData();
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
        SaveData();
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
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Coins", _resources[RESOURCE_COIN_ID].Count);
        PlayerPrefs.SetInt("Crowbars", _resources[RESOURCE_CROWBAR_ID].Count);
        PlayerPrefs.SetInt("DefeatedEnemies", _defeatedEnemies);
        PlayerPrefs.SetInt("DefeatedBosses", _defeatedBosses);
        PlayerPrefs.SetInt("GameLevel", _gameLevel);

        PlayerPrefs.SetString("CurSelectedPlane", _curSelectedPlane);
        PlayerPrefs.SetString("UnlockedPlanes", string.Join(",", _unlockedPlanes));
        PlayerPrefs.SetString("CompletedMissions", string.Join(",", _comletedMissions));

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
        if (PlayerPrefs.HasKey("DefeatedEnemies"))
        {
            _defeatedEnemies = PlayerPrefs.GetInt("DefeatedEnemies");
        }
        if (PlayerPrefs.HasKey("DefeatedBosses"))
        {
            _defeatedBosses = PlayerPrefs.GetInt("DefeatedBosses");
        }
        if (PlayerPrefs.HasKey("GameLevel"))
        {
            _gameLevel = PlayerPrefs.GetInt("GameLevel");
        }
        if (PlayerPrefs.HasKey("CurSelectedPlane"))
        {
            _curSelectedPlane = PlayerPrefs.GetString("CurSelectedPlane");
        }
        if (PlayerPrefs.HasKey("UnlockedPlanes"))
        {
            _unlockedPlanes = new List<string>(PlayerPrefs.GetString("UnlockedPlanes").Split(','));
        }
        if (PlayerPrefs.HasKey("CompletedMissions"))
        {
            _comletedMissions = new List<string>(PlayerPrefs.GetString("CompletedMissions").Split(','));
        }
    }
}

[Serializable]
public class EconomicResource
{
    public string Name;
    public int Count;
}
