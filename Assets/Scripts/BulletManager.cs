using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public List<Bullet> activeBullets;

    public List<Bullet> desactiveBullets;

    [SerializeField] private int _maxBulletsPool;
    [SerializeField] Bullet _bulletPrefab;

    public void DoShoot(Vector3 position, Quaternion rotation)
    {
        if(activeBullets.Count < _maxBulletsPool)
        {
            Bullet bullet = Instantiate(_bulletPrefab, position, rotation, transform);
            bullet.Setup(this);
            activeBullets.Add(bullet);
        }
        else
        {
            Bullet bulletTemp = desactiveBullets[0];
            bulletTemp.gameObject.SetActive(true);
            bulletTemp.transform.position = position;
            bulletTemp.transform.rotation = rotation;
            
            desactiveBullets.Remove(bulletTemp);
        }
    }
}
