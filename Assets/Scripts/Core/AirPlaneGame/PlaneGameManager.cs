using System.Collections.Generic;
using UnityEngine;

public class PlaneGameManager : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] List<GameObject> _enemies;
    [SerializeField] List<GameObject> _bosses;


    private bool _gameEnd = false;
    private bool _gamePaused = false;

    private ApplicationData _appData;

    void Start()
    {
        _appData = ApplicationData.Instance;


    }

    void Update()
    {
        
    }
}
