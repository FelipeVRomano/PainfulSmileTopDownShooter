using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    enum DamageType
    {
        Bullet,
        EnemyChaser
    }

    [Header("DAMAGE CONFIG")]
    [SerializeField] private int _damage;
    [SerializeField] DamageType _damageType;
    
    private bool _oneDamage;

    private void OnEnable()
    {
        _oneDamage = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Life damageable = collision.GetComponent<Life>();
        if (damageable != null)
        {
            if (_oneDamage)
                return;

            _oneDamage = true;

            damageable.TakeDamage(_damage);

            if (_damageType == DamageType.Bullet)
            {
                Bullet myBullet = gameObject.GetComponent<Bullet>();
                myBullet.StartExplosion(collision.gameObject.transform.position);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Life damageable = collision.gameObject.GetComponent<Life>();
        if (damageable != null)
        {
            if (_oneDamage)
                return;

            _oneDamage = true;

            damageable.TakeDamage(_damage);

            if(_damageType == DamageType.EnemyChaser)
            {
                Life myLife = gameObject.GetComponent<Life>();
                myLife.TakeDamage(myLife.ActualLife);
            }
        }
    }
}
