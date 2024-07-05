using TMPro;
using UnityEngine;

public class ResourcesInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _coinsText;
    [SerializeField] TextMeshProUGUI _crowbarsText;

    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;
        _appData.OnResourcesChanged += _appData_OnResourcesChanged;
        _appData_OnResourcesChanged(_appData.GetCoins(), _appData.GetCrowbars());
    }

    private void _appData_OnResourcesChanged(int coins, int crowbars)
    {
        _coinsText.text = coins.ToString();
        _crowbarsText.text = crowbars.ToString();
    }
}
