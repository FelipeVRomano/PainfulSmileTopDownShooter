using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour, IStopAction
{
    [Header("ENEMY SHOOTER")]
    [SerializeField] Transform _frontalShoot;
    [SerializeField] float _cooldownShotFrontal;

    private PoolManager _bulletManager;
    private Transform _playerPosition;
    private EnemyMovement _enemyMovement;
    private Life _lifeManager;

    private float _cooldownShotFrontalBase;
    private bool _gameOver;
    private bool _characterIsDead;

    private void Start()
    {
        _bulletManager = FindObjectOfType<PoolManager>();
        _playerPosition = FindObjectOfType<Player>().transform;
        _enemyMovement = GetComponent<EnemyMovement>();
        _lifeManager = GetComponent<Life>();

        _lifeManager.CharacterDeath += CharacterDeath;
        GameController.gmController.GameOver += GameOver;
    }

    private void OnDestroy()
    {
        _lifeManager.CharacterDeath -= CharacterDeath;
        GameController.gmController.GameOver -= GameOver;
    }

    private void OnEnable()
    {
        ResetEnemyStats();
    }

    private void Update()
    {
        if (_gameOver || _characterIsDead)
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
        _characterIsDead = false;
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

    public void CharacterDeath()
    {
        _characterIsDead = true;
    }
}
