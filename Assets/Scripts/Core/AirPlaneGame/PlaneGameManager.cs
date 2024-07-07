using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGameManager : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Transform _palyerSpawnPos;
    [SerializeField] List<Transform> _enemySpawnPos;
    [SerializeField] List<GameObject> _enemies;
    [SerializeField] List<GameObject> _bosses;

    [SerializeField] float _enemySpawnRate;

    private bool _gameEnd = false;
    private bool _gamePaused = false;

    private ApplicationData _appData;

    void Start()
    {
        _appData = ApplicationData.Instance;

        GameObject player = Instantiate(_playerPrefab, _palyerSpawnPos);
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        var index = Random.Range(0, _enemies.Count);
        var enemiesCount = Random.Range(0, _enemySpawnPos.Count + 1);

        for (int i = 0; i < enemiesCount; i++)
        {
            GameObject newEnemy = Instantiate(_enemies[index], _enemySpawnPos[i]);
            newEnemy.GetComponent<EnemyController>().OnDie += PlaneGameManager_OnDie;
            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }

    private void PlaneGameManager_OnDie()
    {
        if (!_gameEnd)
        {
            _appData.AddEnemiesBosses(1, 0);
        }
    }
}
