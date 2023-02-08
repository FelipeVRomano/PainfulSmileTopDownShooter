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

    void OnTriggerEnter2D(Collider2D collision)
    {
        Life damageable = collision.GetComponent<Life>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);

            if (_damageType == DamageType.Bullet)
            {
                Bullet myBullet = gameObject.GetComponent<Bullet>();
                myBullet.StartExplosion();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Life damageable = collision.gameObject.GetComponent<Life>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);

            if(_damageType == DamageType.EnemyChaser)
            {
                Life myLife = gameObject.GetComponent<Life>();
                myLife.TakeDamage(myLife.ActualLife);
            }
        }
    }
}
