using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionItemIconInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _missionText;

    [SerializeField] Image _rewardImage;
    [SerializeField] TextMeshProUGUI _rewardText;

    [SerializeField] Image _missionBab;
    [SerializeField] TextMeshProUGUI _missionProgressText;

    public void SetInfo(Sprite rewardSprite, string missionState, int curProgress, int maxProgress)
    {
        _rewardImage.sprite = rewardSprite;
        _missionText.text = missionState;
        if (curProgress >= maxProgress)
        {
            _missionBab.fillAmount = 1f;
        }
        else
        {
            _missionBab.fillAmount = (float)curProgress / maxProgress;
        }    
        _missionProgressText.text = $"{curProgress}/{maxProgress}";
    }

}
