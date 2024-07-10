using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpProgressView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _curLvlText;
    [SerializeField] TextMeshProUGUI _curLvlStateText;
    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;
        _appData.OnLvlChanged += _appData_OnExpChanged;

        int lvl = _appData.GetLevel();
        _appData_OnExpChanged(lvl);
    }

    private void _appData_OnExpChanged(int lvl)
    {
        if (lvl % _appData.GetBossLvlRate() == 0)
        {
            _curLvlStateText.text = $"WARNING!!! \n BOSS DETECTED!!!";
        }
        else
        {
            _curLvlStateText.text = $"ENEMY DETECTED!";
        }

        _curLvlText.text = $"LVL: {lvl}";
    }
}