using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionItemIconInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _missionText;

    [SerializeField] Image _rewardImage;
    [SerializeField] TextMeshProUGUI _rewardText;
    [SerializeField] Image _missionBab;

    public void SetInfo(MissionItem missionItem, int curProgress)
    {
        _rewardImage.sprite = missionItem.RewardSprite;
        _rewardText.text = $"x{missionItem.Reward}";
        _missionText.text = missionItem.MissionCondition;
        if (curProgress >= missionItem.MaxProgress)
        {
            _missionBab.fillAmount = 1f;
        }
        else
        {
            _missionBab.fillAmount = (float)curProgress / missionItem.MaxProgress;
        }    
    }
}
