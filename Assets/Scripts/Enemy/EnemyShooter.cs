using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] Transform _frontalShoot;
    [SerializeField] float _cooldownShotFrontal;
    [SerializeField] float _distanceToShoot;

    private float _cooldownShotFrontalBase;
    private PoolManager _bulletManager;
    Transform _playerPosition;
    EnemyMovement _enemyMovement;
    private void Start()
    {
        _bulletManager = FindObjectOfType<PoolManager>();
        _playerPosition = FindObjectOfType<Player>().transform;
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        CheckCooldown();

        if (CanShotFrontal())
        {
            DoEnemyAim();
            FrontalShot();
        }
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
}
