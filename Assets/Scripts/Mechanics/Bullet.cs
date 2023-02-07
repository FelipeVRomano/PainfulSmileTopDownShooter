using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _launchForce = 5f;
    [SerializeField] float _lifeTime = 5;

    private float _lifeTimeBase;
    private PoolManager _bulletManager;

    public void Setup(PoolManager bulletManager)
    {
        _bulletManager = bulletManager;
        _lifeTimeBase = _lifeTime;
    }

    private void FixedUpdate()
    {
        Shoot();
        CooldownBullet();
    }
    void Shoot()
    {
        Vector3 moveBullet = _launchForce * Time.deltaTime * transform.up;
        Vector3 newPosition = transform.localPosition + moveBullet;
        transform.localPosition = newPosition;
    }

    private void CooldownBullet()
    {
        _lifeTimeBase -= Time.deltaTime;

        if(_lifeTimeBase <= 0)
        {
            DisableBullet();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Life damageable = collision.GetComponent<Life>();
        if (damageable != null)
        {
            damageable.TakeDamage(1);
            DisableBullet();
            return;
        }
    }

    void DisableBullet()
    {
        _lifeTimeBase = _lifeTime;

        switch(gameObject.tag)
        {
            case "BulletEnemy":
                _bulletManager.desactiveEnemyBullets.Add(this);
                break;
            case "BulletPlayer":
                _bulletManager.desactivePlayerBullets.Add(this);
                break;
            default:
                Debug.LogError("NOT ASSIGNED BULLET TAG");
                break;
        }
       gameObject.SetActive(false);
    }
}
