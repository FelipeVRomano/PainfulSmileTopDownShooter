using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("BULLET CONFIG")]
    [Range(4, 8)] [SerializeField] float _launchForce = 5f;
    [SerializeField] float _lifeTime = 5;
    [SerializeField] float _explosionTime = .5f;
    [SerializeField] GameObject _explosionObj;

    private float _lifeTimeBase;
    private PoolManager _bulletManager;
    private bool _stopMovement;

    public void Setup(PoolManager bulletManager)
    {
        _bulletManager = bulletManager;
        _lifeTimeBase = _lifeTime;
    }

    private void FixedUpdate()
    {
        if (_stopMovement)
            return;

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

        if (_lifeTimeBase <= 0)
        {
            DisableBullet();
        }
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (_stopMovement)
    //        return;

    //    Life damageable = collision.GetComponent<Life>();
    //    if (damageable != null)
    //    {
    //        damageable.TakeDamage(1);
    //        _stopMovement = true;
    //        StartCoroutine(ExecuteExplosion());
    //        return;
    //    }
    //}

    void DisableBullet()
    {
        _lifeTimeBase = _lifeTime;

        switch (gameObject.tag)
        {
            case "BulletEnemy":
                _bulletManager.desactiveEnemyBullets.Add(this);
                break;
            case "BulletPlayer":
                _bulletManager.desactivePlayerBullets.Add(this);
                break;
            default:
                Debug.LogWarning("NOT ASSIGNED BULLET TAG");
                break;
        }
        _stopMovement = false;
        gameObject.SetActive(false);
    }

    public void StartExplosion()
    {
        if (_stopMovement)
            return;

        _stopMovement = true;
        StartCoroutine(ExecuteExplosion());
    }

    private IEnumerator ExecuteExplosion()
    {
        _explosionObj.SetActive(true);
        yield return new WaitForSeconds(_explosionTime);
        _explosionObj.SetActive(false);

        DisableBullet();
    }
}
