using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaneGameUI : MonoBehaviour
{
    [SerializeField] Button _pauseButton;
    [SerializeField] Button _mainMenuButton;

    [SerializeField] GameObject _postScreen;
    [SerializeField] GameObject _pauseScreen;

    [SerializeField] TextMeshProUGUI _enemiesLeftText;
    [SerializeField] PlaneGameManager _planeManager;

    private void Start()
    {
        _mainMenuButton.onClick.AddListener(() => { OnMainMenu(); });
        _pauseButton.onClick.AddListener(() => { OnPause(); } );

        _planeManager.OnGameEnd += _planeManager_OnGameEnd;
        _planeManager.OnEnemiesCountChanged += _planeManager_OnEnemiesCountChanged;

        _postScreen.SetActive(false);
        _pauseScreen.SetActive(false);
    }

    private void OnPause()
    {
        _planeManager.SetPause();
        _pauseScreen.SetActive(!_pauseScreen.activeSelf);
    }

    private void _planeManager_OnEnemiesCountChanged(int enemiesCount)
    {
        _enemiesLeftText.text = $"Enemies left: {enemiesCount}";
    }

    private void _planeManager_OnGameEnd(bool isWin)
    {
        if (isWin)
        {
            Debug.Log("win");
        }
        else
        {
            Debug.Log("Lose");
        }
        _postScreen.SetActive(true);
    }

    private void OnMainMenu()
    {
        SceneLoader.Instance.LoadScene(SceneLoader.Scene.MainScene);
    }
}
