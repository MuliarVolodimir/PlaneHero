using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGameManager : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Transform _palyerSpawnPos;
    [SerializeField] Transform _bossSpawnPos;
    [SerializeField] List<Transform> _enemySpawnPos;
    [SerializeField] List<GameObject> _enemies;
    [SerializeField] List<GameObject> _bosses;

    [SerializeField] int _maxEnemiesWaves;
    [SerializeField] float _enemySpawnRate;
    [SerializeField] LoadingScreen _loadingScreen; 
    private GameObject _playerObject;
    private List<GameObject> _enemiesObjects;

    private int _waveCount = 0;
    private int _enemiesLeft = 0;
    private bool _gameEnd = false;
    private bool _gamePaused = false;
    private bool _bossLevel = false;
    private ApplicationData _appData;
    
    public event Action<bool> OnGameEnd;
    public event Action<int> OnEnemiesCountChanged;

    void Start()
    {
        _appData = ApplicationData.Instance;

        int lvl = _appData.GetLevel();
        _bossLevel = (float)lvl % _appData.GetBossLvlRate() == 0 ? true : false;
        Debug.Log(_bossLevel.ToString());

        _waveCount = UnityEngine.Random.Range(2, _maxEnemiesWaves);
        _loadingScreen.OnLoad += OnLoad;
    }

    private void OnLoad()
    {
        SpawnPlayer();
        StartCoroutine(SpawnEnemy());
    }

    private void SpawnPlayer()
    {
        if (_playerObject != null) Destroy(_playerObject);  

        _playerObject = Instantiate(_playerPrefab, _palyerSpawnPos);
        _playerObject.GetComponent<PlaneController>().OnDie += PlaneGameManager_OnPlaneDie;
    }

    private void PlaneGameManager_OnPlaneDie()
    {
        if (!_gameEnd)
        {
            GameEnd(false);
        }
    }

    private IEnumerator SpawnEnemy()
    {
        _enemiesObjects = new List<GameObject>();

        var index = UnityEngine.Random.Range(0, _enemies.Count);
        var enemiesCount = UnityEngine.Random.Range(3, _enemySpawnPos.Count + 1);
        _enemiesLeft = enemiesCount;

        for (int i = 0; i < enemiesCount; i++)
        {
            GameObject newEnemy = Instantiate(_enemies[index], _enemySpawnPos[i]);
            newEnemy.GetComponent<EnemyController>().OnDie += PlaneGameManager_OnTargetDie;
            _enemiesObjects.Add(newEnemy);
            yield return new WaitForSeconds(_enemySpawnRate);
        }
        _waveCount--;
        OnEnemiesCountChanged?.Invoke(_enemiesLeft);
    }

    private void PlaneGameManager_OnTargetDie()
    {
        if (!_gameEnd)
        {
            _appData.AddEnemiesBosses(1, 0);
            _enemiesLeft--;
            OnEnemiesCountChanged?.Invoke(_enemiesLeft);
            CheckWin();
        }
    }

    private void CheckWin()
    {
        if (_enemiesLeft <= 0)
        {
            if (_waveCount <= 0)
            {
                if (_bossLevel)
                {
                    SpawnBoss();
                }
                else
                {
                    GameEnd(true);
                }
            }
            else
            {
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    private void SpawnBoss()
    {
        var index = UnityEngine.Random.Range(0, _bosses.Count);
        GameObject boss = Instantiate(_bosses[index], _bossSpawnPos);
        var bossController = boss.GetComponent<BossController>();
        bossController.OnDie += BossController_OnDie;
        _enemiesObjects.Add(boss);

        Debug.Log("boss spawned");
    }

    private void BossController_OnDie()
    {
        GameEnd(true);
        _appData.AddEnemiesBosses(0, 1);
        Debug.Log("boss defeated");
    }

    public void SetPause()
    {
        _gamePaused = !_gamePaused;
        if (_gamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void GameEnd(bool isWin)
    {
        if (isWin)
        {
            _appData.AddLevel();
        }
        _gameEnd = true;
        OnGameEnd?.Invoke(isWin);

        if (_playerObject != null) Destroy(_playerObject);
        foreach (var item in _enemiesObjects)
        {
            if (item != null) Destroy(item);
        }
        _enemiesObjects.Clear();
    }
}
