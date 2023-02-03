using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [Header("MAP BOUNDS")]
    [SerializeField] Vector2 _mapBoundsStart;
    [SerializeField] Vector2 _mapBoundsEnd;

    public Vector2 MapBoundsStart => _mapBoundsStart;
    public Vector2 MapBoundsEnd => _mapBoundsEnd;

    [Header("GAME SESSION INFO")]
    [SerializeField] float _gameSessionTime;

    [Header("ENEMY MANAGER")]
    [SerializeField] float _enemySpawnTime;
    [SerializeField] GameObject _chaserEnemy;
    [SerializeField] GameObject _shooterEnemy;
    [SerializeField] List<Transform> _enemySpawnPosition;

    float _enemySpawnTimeBase;

    private void Start()
    {
        StartCoroutine(GameSessionRun());
    }

    IEnumerator GameSessionRun()
    {
        _enemySpawnTimeBase = _enemySpawnTime;
        while (_gameSessionTime > 0)
        {
            _gameSessionTime -= Time.deltaTime;

            SpawnEnemies();
            yield return null;
        }
        
        Debug.LogError("GAME SESSION ENDED");
    }

    void SpawnEnemies()
    {
        _enemySpawnTime -= Time.deltaTime;

        if (_enemySpawnTime < 0)
        {
            Debug.Log("SPAWNING ENEMY");
            _enemySpawnTime = _enemySpawnTimeBase;
        }
    }
}
