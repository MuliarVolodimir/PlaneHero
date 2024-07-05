using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsManager : MonoBehaviour
{
    [SerializeField] List<MissionItem> _coinMissions;
    [SerializeField] List<MissionItem> _crowbarMissions;

    [SerializeField] GameObject _missionItemIconInfo;
    [SerializeField] GameObject _missionsContent;

    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;
        InitializeItems(_coinMissions, MissionItem.RewardType.Coins);
        InitializeItems(_crowbarMissions, MissionItem.RewardType.Crowbars);
    }

    private void InitializeItems(List<MissionItem> items, MissionItem.RewardType rewardType)
    {
        foreach (var item in items)
        {
            if (item.CurProgress >= item.MaxProgress) continue;

            GameObject newItem = Instantiate(_missionItemIconInfo, _missionsContent.transform);
            var newMissionItemIconInfo = newItem.GetComponent<MissionItemIconInfo>();

            int currentProgress = rewardType == MissionItem.RewardType.Coins ? _appData.GetDefeatedEnemies() : _appData.GetDefeatedBosses();
            item.CurProgress = currentProgress;

            newMissionItemIconInfo.SetInfo(
                item.RewardSprite,
                item.MissionCondition,
                item.CurProgress,
                item.MaxProgress
            );

            newItem.GetComponent<Button>().onClick.AddListener(() => { OnItemInfoClick(item, newItem); });
        }
    }

    private void OnItemInfoClick(MissionItem item, GameObject missionObject)
    {
        if (item.CurProgress >= item.MaxProgress)
        {
            switch (item.Type)
            {
                case MissionItem.RewardType.Coins:
                    _appData.AddResourceCoin(item.Reward);
                    break;
                case MissionItem.RewardType.Crowbars:
                    _appData.AddResourceCrowbar(item.Reward);
                    break;
            }

            Destroy(missionObject);
            CheckAndRespawnMissions();
        }
    }

    private void CheckAndRespawnMissions()
    {
        if (AllMissionsCompleted(_coinMissions) && AllMissionsCompleted(_crowbarMissions))
        {
            ResetMissions(_coinMissions);
            ResetMissions(_crowbarMissions);
            InitializeItems(_coinMissions, MissionItem.RewardType.Coins);
            InitializeItems(_crowbarMissions, MissionItem.RewardType.Crowbars);
        }
    }

    private bool AllMissionsCompleted(List<MissionItem> missions)
    {
        foreach (var mission in missions)
        {
            if (mission.CurProgress < mission.MaxProgress) return false;
        }
        return true;
    }

    private void ResetMissions(List<MissionItem> missions)
    {
        foreach (var mission in missions)
        {
            mission.CurProgress = 0;
        }
    }
}


[Serializable]
public class MissionItem
{
    public string MissionCondition;
    public int CurProgress;
    public int MaxProgress;
    public Sprite RewardSprite;
    public int Reward;
    public RewardType Type;

    public enum RewardType
    {
        Coins,
        Crowbars
    }
}
