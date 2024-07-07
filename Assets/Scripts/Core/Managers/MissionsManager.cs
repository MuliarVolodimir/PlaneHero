using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionsManager : MonoBehaviour
{
    [SerializeField] Image _expProgressBar;
    [SerializeField] TextMeshProUGUI _expRewardText;
    [SerializeField] int _expReward;

    [SerializeField] List<MissionItem> _coinMissions;
    [SerializeField] List<MissionItem> _crowbarMissions;
    [SerializeField] GameObject _missionItemIconInfo;
    [SerializeField] GameObject _missionsContent;

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
        UpdateExpMissionProgress();
        Debug.Log(_totalMissions);
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
            _appData.AddExp(_expReward);
            ResetMissions();
            InitializeItems(_coinMissions);
            InitializeItems(_crowbarMissions);
        }
        UpdateExpMissionProgress();
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

    private void UpdateExpMissionProgress()
    {
        if (_completedMissions <= _totalMissions)
        {
            _expProgressBar.fillAmount = _completedMissions / _totalMissions;
            _expRewardText.text = $"x{_expReward}";
        }
        else
        {
            _expProgressBar.fillAmount = 1f;
            _expRewardText.text = $"CLAIMED";
        }
    }
}
