using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("BULLET CONFIG")]
    [Range(4, 8)] [SerializeField] float _launchForce = 5f;
    [Range(1, 5)] [SerializeField] float _lifeTime = 5;
    [Range(0.1f, 0.5f)] [SerializeField] float _explosionTime = .5f;
    [SerializeField] GameObject _explosionObj;

    private float _lifeTimeBase;
    private PoolManager _bulletManager;
    private SpriteRenderer _spriteRenderer;
    private bool _stopMovement;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

    public void StartExplosion(Vector3 explosionPos)
    {
        if (_stopMovement)
            return;

        _stopMovement = true;
        StartCoroutine(ExecuteExplosion(explosionPos));
    }

    private IEnumerator ExecuteExplosion(Vector3 explosionPos)
    {
        _explosionObj.SetActive(true);
        _explosionObj.transform.position = explosionPos;
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(_explosionTime);
        _spriteRenderer.enabled = true;
        _explosionObj.SetActive(false);

        DisableBullet();
    }
}
