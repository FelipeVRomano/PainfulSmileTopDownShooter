using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] Transform _frontalShoot;
    [SerializeField] float _cooldownShotFrontal;
    private float _cooldownShotFrontalBase;
    private BulletManager _bulletManager;
    Transform _playerPosition;

    private void Start()
    {
        _bulletManager = FindObjectOfType<BulletManager>();
        _playerPosition = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        CheckCooldown();
        _frontalShoot.up = _playerPosition.position - transform.position;
        if (CanShotFrontal())
        {
            FrontalShot();
        }
    }

    bool CanShotFrontal()
    {
        return _cooldownShotFrontalBase <= 0;
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
