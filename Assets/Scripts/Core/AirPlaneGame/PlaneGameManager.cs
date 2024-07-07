using System.Collections.Generic;
using UnityEngine;

public class PlaneGameManager : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Transform _palyerSpawnPos;
    [SerializeField] List<Transform> _enemySpawnPos;
    [SerializeField] List<GameObject> _enemies;
    [SerializeField] List<GameObject> _bosses;

    private bool _gameEnd = false;
    private bool _gamePaused = false;

    private ApplicationData _appData;

    void Start()
    {
        _appData = ApplicationData.Instance;

        GameObject player = Instantiate(_playerPrefab, _palyerSpawnPos);
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        var index = UnityEngine.Random.Range(0, _enemies.Count);

        for (int i = 0; i < _enemySpawnPos.Count; i++)
        {
            GameObject newEnemy = Instantiate(_enemies[index], _enemySpawnPos[i]);
        }
        
    }
}
