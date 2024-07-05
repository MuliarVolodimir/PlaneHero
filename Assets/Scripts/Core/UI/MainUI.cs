using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] Button _planeGameButton;
    [SerializeField] Button _slotMachineButton;
    [SerializeField] Button _missionsButton;
    [SerializeField] Button _settingsButton;
    [SerializeField] Button _shopButton;
    [SerializeField] Button _onChoosePlane;
    [SerializeField] Button _backButton;

    [SerializeField] GameObject _mainScreen;
    [SerializeField] GameObject _missionsScreen;
    [SerializeField] GameObject _slotMachineScreen;
    [SerializeField] GameObject _settingsScreen;
    [SerializeField] GameObject _choosePlaneScreen;
    [SerializeField] GameObject _shopScreen;
    [SerializeField] GameObject _popupScreen;
    [SerializeField] Image _selectedWeaponImage;

    [SerializeField] AudioClip _applyButtonClip;
    [SerializeField] AudioClip _cancelButtonClip;
    [SerializeField] AudioClip _backgroundClip;

    [SerializeField] LoadingScreen _loadingScreen;

    private List<GameObject> _activeScreens;
    private int _screenIndex;

    private ApplicationData _appData;

    private void Start()
    {
        _activeScreens = new List<GameObject> { _popupScreen, _mainScreen, _settingsScreen, _shopScreen, _slotMachineScreen, _missionsScreen, _choosePlaneScreen };

        foreach (var screen in _activeScreens)
        {
            screen.SetActive(false);
        }

        _mainScreen.SetActive(true);

        _planeGameButton.onClick.AddListener(() => { OnPlaneGame(); });
        _slotMachineButton.onClick.AddListener(() => { OnSlotMachine(); });
        _settingsButton.onClick.AddListener(() => { OnSettings(); });
        _shopButton.onClick.AddListener(() => { OnShop(); });
        _missionsButton.onClick.AddListener(() => { OnMissions(); });
        _onChoosePlane.onClick.AddListener(() => { OnChoosePlane(); });
        _backButton.onClick.AddListener(() => { CloseActiveWindow(); });
        _loadingScreen.OnLoad += _loadingScreen_OnLoad;
    }

    private void _loadingScreen_OnLoad()
    {
        AudioManager.Instance.SetBackGroundMusic(_backgroundClip);
    }

    private void OnChoosePlane()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        _backButton.gameObject.SetActive(true);
        SwitchActive(_choosePlaneScreen);
    }

    private void OnMissions()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        _backButton.gameObject.SetActive(true);
        SwitchActive(_missionsScreen);
    }

    private void OnShop()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        _backButton.gameObject.SetActive(true);
        SwitchActive(_shopScreen);
    }

    private void OnSlotMachine()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        _backButton.gameObject.SetActive(true);
        SwitchActive(_slotMachineScreen);
    }

    public void OnSettings()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        _settingsScreen.SetActive(!_settingsScreen.activeSelf);
    }

    private void OnPlaneGame()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        SceneLoader.Instance.LoadScene(SceneLoader.Scene.PlaneGameScene);
    }

    private void CloseActiveWindow()
    {
        AudioManager.Instance.PlayOneShotSound(_cancelButtonClip);
        _backButton.gameObject.SetActive(false);

        if (_activeScreens[_screenIndex] != _mainScreen)
        {
            _activeScreens[_screenIndex].SetActive(false);
            _mainScreen.SetActive(true);
            _screenIndex = _activeScreens.IndexOf(_mainScreen);
        }
    }

    private void SwitchActive(GameObject screen)
    {
        if (screen != null)
        {
            foreach (var activeScreen in _activeScreens)
            {
                activeScreen.SetActive(false);
            }
            screen.SetActive(true);
            _screenIndex = _activeScreens.IndexOf(screen);
        }
    }
}
