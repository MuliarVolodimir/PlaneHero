using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaneGameUI : MonoBehaviour
{
    [SerializeField] Button _pauseButton;
    [SerializeField] Button _mainMenuButton;

    [SerializeField] GameObject _postScreen;
    [SerializeField] GameObject _pauseScreen;

    [SerializeField] TextMeshProUGUI _gameActionText;
    [SerializeField] TextMeshProUGUI _gameResultsText;
    [SerializeField] TextMeshProUGUI _enemiesLeftText;
    [SerializeField] PlaneGameManager _planeManager;

    [SerializeField] AudioClip _applyClip;
    [SerializeField] AudioClip _backgroundClip;

    private void Start()
    {
        AudioManager.Instance.SetBackGroundMusic(_backgroundClip);

        _mainMenuButton.onClick.AddListener(() => { OnMainMenu(); });
        _pauseButton.onClick.AddListener(() => { OnPause(); });

        _planeManager.OnGameEnd += _planeManager_OnGameEnd;
        _planeManager.OnEnemiesCountChanged += _planeManager_OnEnemiesCountChanged;
        _planeManager.OnBossSpawned += _planeManager_OnBossSpawned;

        _gameActionText.gameObject.SetActive(false);
        _postScreen.SetActive(false);
        _pauseScreen.SetActive(false);
    }

    private void _planeManager_OnBossSpawned()
    {
        _gameActionText.color = Color.red;
        StartCoroutine(DisplayActionText("WARNING!!! \n BOSS", 2f));
        _enemiesLeftText.text = $"BOSS";
    }

    private void OnPause()
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        _planeManager.SetPause();
        _pauseScreen.SetActive(!_pauseScreen.activeSelf);
    }

    private void _planeManager_OnEnemiesCountChanged(int enemiesCount)
    {
        _enemiesLeftText.text = $"Enemies left: {enemiesCount}";
    }

    private void _planeManager_OnGameEnd(bool isWin, int defeatedEnemies)
    {
        if (isWin)
        {
            _gameActionText.color = Color.green;
            StartCoroutine(DisplayActionText($"CONGRATULATION!!! \n LEVEL COMPLETED!", 0f)); 
        }
        else
        {
            _gameActionText.color = Color.red;
            StartCoroutine(DisplayActionText($"LOSE! \n GOOD LUCK NEXT TIME!", 0f));
        }
        _gameResultsText.text = $"Enemies defeated: {defeatedEnemies}";
        _postScreen.SetActive(true);
    }

    private void OnMainMenu()
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        AudioManager.Instance.ResetBackgroundMusic();
        SceneLoader.Instance.LoadScene(SceneLoader.Scene.MainScene);
    }

    private IEnumerator DisplayActionText(string message, float duration)
    {
        _gameActionText.text = message;
        _gameActionText.gameObject.SetActive(true);
 
        yield return new WaitForSeconds(duration);

        if (duration > 0)
        {
            _gameActionText.gameObject.SetActive(false);
        }
    }
}
