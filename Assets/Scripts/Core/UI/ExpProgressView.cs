using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpProgressView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _curLvlText;
    [SerializeField] Image _currExpProgress;

    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;
        _appData.OnExpChanged += _appData_OnExpChanged;

        (int exp, int toTheNextlvl, int lvl) = _appData.GetExp();
        _appData_OnExpChanged(exp, toTheNextlvl, lvl);
    }

    private void _appData_OnExpChanged(int expCount, int expToTheNextLvl, int lvl)
    {
        Debug.Log($"expCount: {expCount}, expToTheNextLvl: {expToTheNextLvl}, lvl: {lvl}");

        _currExpProgress.fillAmount = (float)expCount / expToTheNextLvl;
        _curLvlText.text = $"LVL: {lvl}";
    }
}