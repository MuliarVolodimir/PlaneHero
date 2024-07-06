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
        InitializeItems(_coinMissions);
        InitializeItems(_crowbarMissions);
    }

    private void InitializeItems(List<MissionItem> missionItems)
    {
        foreach (var missionItem in missionItems)
        {
            if (_appData.IsMissionCompleted(missionItem.MissionCondition) == false) continue;
            else Debug.Log($"{missionItem.MissionCondition} comleted");

            GameObject itemObj = Instantiate(_missionItemIconInfo, _missionsContent.transform);
            var newMissionItemIconInfo = itemObj.GetComponent<MissionItemIconInfo>();

            int currentProgress = missionItem.Type == MissionItem.RewardType.Coins ? _appData.GetDefeatedEnemies() : _appData.GetDefeatedBosses();

            newMissionItemIconInfo.SetInfo( missionItem, currentProgress);

            var missionButton = itemObj.GetComponent<Button>();
            if (_appData.IsMissionCompleted(missionItem.MissionCondition))
            {
                missionButton.onClick.AddListener(() => { OnItemInfoClick(missionItem, missionButton, currentProgress); });
            }
            else
            {
                missionButton.interactable = false;
            }
        }
    }

    private void OnItemInfoClick(MissionItem missionItem, Button missionButton, int progress)
    {
        if (progress >= missionItem.MaxProgress)
        {
            switch (missionItem.Type)
            {
                case MissionItem.RewardType.Coins:
                    _appData.AddResourceCoin(missionItem.Reward);
                    break;
                case MissionItem.RewardType.Crowbars:
                    _appData.AddResourceCrowbar(missionItem.Reward);
                    break;
            }

            _appData.CompleteMission(missionItem.MissionCondition);
            missionButton.interactable = false;
            Debug.Log($"{missionItem.MissionCondition} claimed");
            CheckAndRespawnMissions();
        }
    }

    private void CheckAndRespawnMissions()
    {
        if (AllMissionsCompleted(_coinMissions) && AllMissionsCompleted(_crowbarMissions))
        {
            ResetMissions();
            InitializeItems(_coinMissions);
            InitializeItems(_crowbarMissions);
        }
    }

    private bool AllMissionsCompleted(List<MissionItem> missions)
    {
        foreach (var mission in missions)
        {
            if (_appData.IsMissionCompleted(mission.MissionCondition) == false)
            {
                return false;
            }   
        }
        return true;
    }

    private void ResetMissions()
    {
        _appData.ResetProgress();
    }
}
