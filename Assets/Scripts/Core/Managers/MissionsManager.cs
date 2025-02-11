using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsManager : MonoBehaviour
{
    [SerializeField] List<MissionItem> _coinMissions;
    [SerializeField] List<MissionItem> _crowbarMissions;
    [SerializeField] GameObject _missionItemIconInfo;
    [SerializeField] GameObject _missionsContent;

    [SerializeField] AudioClip _applyClip;

    private float _completedMissions = 0f;
    private float _totalMissions = 0f;
    private List<GameObject> _curMissionsObjects;
    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;
        _curMissionsObjects = new List<GameObject>();
        InitializeItems(_coinMissions);
        InitializeItems(_crowbarMissions);
    }

    // spawn mission objects
    private void InitializeItems(List<MissionItem> missionItems)
    {
        foreach (var missionItem in missionItems)
        {
            GameObject itemObj = Instantiate(_missionItemIconInfo, _missionsContent.transform);
            var newMissionItemIconInfo = itemObj.GetComponent<MissionItemIconInfo>();

            int currentProgress = missionItem.Type == MissionItem.RewardType.Coins ? _appData.GetDefeatedEnemies() : _appData.GetDefeatedBosses();

            newMissionItemIconInfo.SetInfo( missionItem, currentProgress);

            var missionButton = itemObj.GetComponent<Button>();
            if (!_appData.IsMissionCompleted(missionItem.MissionCondition))
            {
                missionButton.onClick.AddListener(() => { OnItemInfoClick(missionItem, missionButton, currentProgress); });
            }
            else
            {
                missionButton.interactable = false;
            }
            _totalMissions++;
            _curMissionsObjects.Add(itemObj);
        }
    }

    // claim reward and complite mission
    private void OnItemInfoClick(MissionItem missionItem, Button missionButton, int progress)
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
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
            _completedMissions++;
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
        if (_completedMissions == _totalMissions)
        {
            _completedMissions = 0;
            _totalMissions = 0;
        }

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
        foreach (var item in _curMissionsObjects)
        {
            Destroy(item);
        }
        _curMissionsObjects.Clear();
        _appData.ResetProgress();
    }
}
