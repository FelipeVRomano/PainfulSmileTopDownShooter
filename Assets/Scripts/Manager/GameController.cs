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
    PoolManager _bulletManager;

    private void Start()
    {
        _bulletManager = GetComponent<PoolManager>();
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
        
    }

    void SpawnEnemies()
    {
        _enemySpawnTime -= Time.deltaTime;

        if (_enemySpawnTime < 0)
        {
            int checkSpawnOffCamera = CheckSpawnPositionOffCamera();
            string enemyName = SetEnemyName();

            Debug.LogError("SPAWN POSITION: " + checkSpawnOffCamera + " : " + enemyName);
            _bulletManager.DoEnemySpawn(_enemySpawnPosition[checkSpawnOffCamera].position,
                                        _enemySpawnPosition[checkSpawnOffCamera].rotation,
                                        enemyName);

            _enemySpawnTime = _enemySpawnTimeBase;
        }
    }

    int CheckSpawnPositionOffCamera()
    {
        for(int i = 0; i < _enemySpawnPosition.Count; i++)
        {
            Vector3 spawnPos = Camera.main.WorldToViewportPoint(_enemySpawnPosition[i].position);

            if ((spawnPos.x <= 0f || spawnPos.x >= 1f) && (spawnPos.y <= 0f || spawnPos.y >= 1f))
            {
                Debug.LogError("FOUND INDEX OF: " + i);
                return i;
            }

            Debug.LogError("CHECKING EVERY INDEX: " + spawnPos);
        }

        Debug.LogError("NOT FOUND ANY SHIT");
        return 0;
    }

    string SetEnemyName()
    {
        bool canSpawnShooter = false;
        bool canSpawnChaser = false;

        if (_bulletManager.allEnemiesChaser.Count < 5 || _bulletManager.desactiveEnemyChaser.Count > 0) canSpawnChaser = true;
        if (_bulletManager.allEnemiesShooter.Count < 5 || _bulletManager.desactiveEnemyShooter.Count > 0) canSpawnShooter = true;

        Debug.LogError("CAN SPAWN: CHASER" + canSpawnChaser + " SHOOTER " + canSpawnShooter);

        if (canSpawnChaser && canSpawnShooter)
        {
            int i = UnityEngine.Random.Range(0, 2);

            Debug.LogError("INDEX RANDOM: " + i);

            if (i == 0) return "Chaser";
            else return "Shooter";
        }
        else if(!canSpawnShooter && canSpawnChaser)
            return "Chaser";
        else if(canSpawnShooter && !canSpawnChaser)
            return "Shooter";
        else
            return "";
    }
}
