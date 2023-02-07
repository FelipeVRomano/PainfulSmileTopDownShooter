using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour, IGameOver
{
    [SerializeField] Transform _frontalShoot;
    [SerializeField] float _cooldownShotFrontal;
    [SerializeField] float _distanceToShoot;

    private float _cooldownShotFrontalBase;
    private PoolManager _bulletManager;
    Transform _playerPosition;
    EnemyMovement _enemyMovement;
    GameController _gmController;
    private bool _gameOver;

    private void Start()
    {
        _bulletManager = FindObjectOfType<PoolManager>();
        _playerPosition = FindObjectOfType<Player>().transform;
        _enemyMovement = GetComponent<EnemyMovement>();
        _gmController = FindObjectOfType<GameController>();
        _gmController.GameOver += GameOver;
    }

    private void OnDestroy()
    {
        _gmController.GameOver -= GameOver;
    }

    private void OnEnable()
    {
        ResetEnemyStats();
    }

    private void Update()
    {
        if (_gameOver)
            return;

        CheckCooldown();

        if (CanShotFrontal())
        {
            DoEnemyAim();
            FrontalShot();
        }
    }

    private void ResetEnemyStats()
    {
        _cooldownShotFrontalBase = _cooldownShotFrontal;
    }

    private void DoEnemyAim()
    {
        _frontalShoot.up = _playerPosition.position - transform.position;
    }

    bool CanShotFrontal()
    {
        return _cooldownShotFrontalBase <= 0 && _enemyMovement.PlayerIsClose;
    }

    void CheckCooldown()
    {
        if (_cooldownShotFrontalBase > 0)
        {
            _cooldownShotFrontalBase -= Time.deltaTime;
        }
    }

    private void FrontalShot()
    {
        _cooldownShotFrontalBase = _cooldownShotFrontal;


        _bulletManager.DoShootEnemy(_frontalShoot.position, _frontalShoot.rotation);
    }

    public void GameOver()
    {
        _gameOver = true;
    }
}
