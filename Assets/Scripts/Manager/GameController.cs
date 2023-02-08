using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("MAP BOUNDS")]
    [SerializeField] Vector2 _mapBoundsStart;
    [SerializeField] Vector2 _mapBoundsEnd;

    [Header("SET TRUE TO IGNORE SAVE DATA")]
    [SerializeField] bool _ignoreLoadData;
    [Header("GAME SESSION INFO")] 
    [Range(1,3)] [SerializeField] float _gameSessionTime;

    [Header("ENEMY MANAGER")]
    [Range(5, 20)] [SerializeField] float _enemySpawnTime;
    [SerializeField] GameObject _chaserEnemy;
    [SerializeField] GameObject _shooterEnemy;
    [SerializeField] List<Transform> _enemySpawnPosition;

    [Header("GAME OVER")]
    [SerializeField] GameObject _gameOverObject;
    [SerializeField] TextMeshProUGUI _text;

    [Header("GAME UI")]
    [SerializeField] TextMeshProUGUI _scoreTxt;
    [SerializeField] TextMeshProUGUI _gameSessionTimeTxt;

    public Vector2 MapBoundsStart => _mapBoundsStart;
    public Vector2 MapBoundsEnd => _mapBoundsEnd;
    public int PlayerScore { get; set; }

    public System.Action GameOver;
    public static GameController gmController;

    private float _enemySpawnTimeBase;
    private float _gameSessionTimeBase;
    private bool _playerIsDead;
    PoolManager _bulletManager;

    private void Awake()
    {
        if (gmController != null)
            Destroy(gmController.gameObject);

        gmController = this;
    }

    private void Start()
    {
        _bulletManager = FindObjectOfType<PoolManager>();

        if (!_ignoreLoadData)
        {
            LoadGameData();
        }

        StartCoroutine(GameSessionRun());
    }

    public void PlayerDied()
    {
        _playerIsDead = true;
    }

    private void LoadGameData()
    {
        _gameSessionTime = PlayerPrefs.GetInt("GameSessionTime", Mathf.RoundToInt(_gameSessionTime));
        _enemySpawnTime = PlayerPrefs.GetInt("EnemySpawnRate", Mathf.RoundToInt(_enemySpawnTime));
    }

    IEnumerator GameSessionRun()
    {
        _enemySpawnTimeBase = 1f;
        _gameSessionTimeBase = _gameSessionTime * 60;
        while (_gameSessionTimeBase > 0)
        {
            _gameSessionTimeBase -= Time.deltaTime;

            SpawnEnemies();
            RefreshUIGame();

            if(_playerIsDead)
            {
                DoGameOver(false);
                yield break;
            }
            yield return null;
        }
        DoGameOver(true);       
    }

    private void RefreshUIGame()
    {
        _scoreTxt.text = "SCORE: " + PlayerScore.ToString();
        int gameSessionTime = Mathf.RoundToInt(_gameSessionTimeBase);
        _gameSessionTimeTxt.text = gameSessionTime.ToString();
    }

    public void AddScore()
    {
        PlayerScore += 1;
    }

    private void DoGameOver(bool resultPlayerVictory)
    {
        GameOver?.Invoke();

        if(SaveManager.Instance != null)
        {
            SaveManager.Instance.AddChangeKeyToSave("PlayerScore", PlayerScore);

            int bestScore = PlayerPrefs.GetInt("BestPlayerScore", 0);

            if(bestScore < PlayerScore)
                SaveManager.Instance.AddChangeKeyToSave("BestPlayerScore", PlayerScore);

            SaveManager.Instance.SaveGame();
        }
        else
        {
            Debug.LogWarning("Save Manager is null, enabled it to save Player score");
        }

        _gameOverObject.SetActive(true);

        if (resultPlayerVictory)
        {
            _text.text = "YOU SURVIVED!";
        }
        else
        {
            _text.text = "YOU LOST!";
        }
    }

    #region EnemySpawnManager
    void SpawnEnemies()
    {
        _enemySpawnTimeBase -= Time.deltaTime;

        if (_enemySpawnTimeBase < 0)
        {
            int checkSpawnOffCamera = CheckSpawnPositionOffCamera();
            string enemyName = SetEnemyName();

            _bulletManager.DoEnemySpawn(_enemySpawnPosition[checkSpawnOffCamera].position,
                                        _enemySpawnPosition[checkSpawnOffCamera].rotation,
                                        enemyName);

            _enemySpawnTimeBase = _enemySpawnTime;
        }
    }

    int CheckSpawnPositionOffCamera()
    {
        int randomIndex = UnityEngine.Random.Range(0, _enemySpawnPosition.Count);
        return randomIndex;
    }

    string SetEnemyName()
    {
        bool canSpawnShooter = false;
        bool canSpawnChaser = false;

        if (_bulletManager.allEnemiesChaser.Count < 5 || _bulletManager.desactiveEnemyChaser.Count > 0) canSpawnChaser = true;
        if (_bulletManager.allEnemiesShooter.Count < 5 || _bulletManager.desactiveEnemyShooter.Count > 0) canSpawnShooter = true;

        if (canSpawnChaser && canSpawnShooter)
        {
            int i = UnityEngine.Random.Range(0, 2);

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
    #endregion
}
