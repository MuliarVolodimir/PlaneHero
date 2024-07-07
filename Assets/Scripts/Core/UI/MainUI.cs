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
    [SerializeField] Button _choosePlaneButton;
    [SerializeField] Button _backButton;

    [SerializeField] GameObject _mainScreen;
    [SerializeField] GameObject _missionsScreen;
    [SerializeField] GameObject _slotMachineScreen;
    [SerializeField] GameObject _settingsScreen;
    [SerializeField] GameObject _choosePlaneScreen;
    [SerializeField] GameObject _shopScreen;

    [SerializeField] AudioClip _applyButtonClip;
    [SerializeField] AudioClip _cancelButtonClip;
    [SerializeField] AudioClip _backgroundClip;

    [SerializeField] LoadingScreen _loadingScreen;

    private List<GameObject> _screens;
    private GameObject _currentActiveScreen;

    private void Start()
    {
        _screens = new List<GameObject> { _settingsScreen, _shopScreen, _slotMachineScreen, _missionsScreen, _choosePlaneScreen };

        foreach (var screen in _screens)
        {
            screen.SetActive(false);
        }

        _mainScreen.SetActive(true);
        _backButton.gameObject.SetActive(false);
        _currentActiveScreen = _mainScreen;

        _planeGameButton.onClick.AddListener(OnPlaneGame);
        _slotMachineButton.onClick.AddListener(OnSlotMachine);
        _settingsButton.onClick.AddListener(OnSettings);
        _shopButton.onClick.AddListener(OnShop);
        _missionsButton.onClick.AddListener(OnMissions);
        _choosePlaneButton.onClick.AddListener(OnChoosePlane);
        _backButton.onClick.AddListener(CloseActiveWindow);
        _loadingScreen.OnLoad += _loadingScreen_OnLoad;
    }

    private void _loadingScreen_OnLoad()
    {
        AudioManager.Instance.SetBackGroundMusic(_backgroundClip);
    }

    private void OnChoosePlane()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        ToggleWindow(_choosePlaneScreen);
    }

    private void OnMissions()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        ToggleWindow(_missionsScreen);
    }

    private void OnShop()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        ToggleWindow(_shopScreen);
    }

    private void OnSlotMachine()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        ToggleWindow(_slotMachineScreen);
    }

    public void OnSettings()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        ToggleWindow(_settingsScreen);
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

        if (_currentActiveScreen != _mainScreen)
        {
            _currentActiveScreen.SetActive(false);
            _mainScreen.SetActive(true);
            _currentActiveScreen = _mainScreen;
        }
    }

    private void ToggleWindow(GameObject screen)
    {
        bool isActive = screen.activeSelf;

        foreach (var activeScreen in _screens)
        {
            activeScreen.SetActive(false);
        }

        if (!isActive)
        {
            screen.SetActive(true);
            _currentActiveScreen = screen;
        }
        else
        {
            _mainScreen.SetActive(true);
            _currentActiveScreen = _mainScreen;
        }

        if (screen != _settingsScreen)
            _backButton.gameObject.SetActive(!isActive);
    }
}
