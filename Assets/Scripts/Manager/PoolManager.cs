using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("ENEMY POOL CONFIG")]
    [SerializeField] GameObject _enemyChaserPrefab;
    [SerializeField] GameObject _enemyShooterPrefab;
    [SerializeField] GameObject _parentEnemyPool;
    [SerializeField] private int _maxEnemiesPool;

    [Header("ENEMY BULLET POOL CONFIG")]
    [SerializeField] private int _maxEnemiesBulletsPool;
    [SerializeField] Bullet _bulletEnemyPrefab;
    [SerializeField] GameObject _parentEnemyBulletsPool;

    [Header("PLAYER BULLET POOL CONFIG")]
    [SerializeField] private int _maxPlayersBulletsPool;
    [SerializeField] Bullet _bulletPlayerPrefab;
    [SerializeField] GameObject _parentPlayerBulletsPool;

    [HideInInspector] public List<Bullet> allPlayerBullets;
    [HideInInspector] public List<Bullet> desactivePlayerBullets;

    [HideInInspector] public List<Bullet> allEnemyBullets;
    [HideInInspector] public List<Bullet> desactiveEnemyBullets;

    [HideInInspector] public List<GameObject> allEnemiesChaser;
    [HideInInspector] public List<GameObject> desactiveEnemyChaser;

    [HideInInspector] public List<GameObject> allEnemiesShooter;
    [HideInInspector] public List<GameObject> desactiveEnemyShooter;

    public void DoShootPlayer(Vector3 position, Quaternion rotation)
    {
        if(allPlayerBullets.Count < _maxPlayersBulletsPool)
        {
            Bullet bullet = Instantiate(_bulletPlayerPrefab, position, rotation, _parentPlayerBulletsPool.transform);
            bullet.Setup(this);
            allPlayerBullets.Add(bullet);
        }
        else
        {
            Bullet bulletTemp = desactivePlayerBullets[0];
            bulletTemp.gameObject.SetActive(true);
            bulletTemp.transform.position = position;
            bulletTemp.transform.rotation = rotation;
            
            desactivePlayerBullets.Remove(bulletTemp);
        }
    }

    public void DoShootEnemy(Vector3 position, Quaternion rotation)
    {
        if (allEnemyBullets.Count < _maxEnemiesBulletsPool)
        {
            Bullet bullet = Instantiate(_bulletEnemyPrefab, position, rotation, _parentEnemyBulletsPool.transform);
            bullet.Setup(this);
            allEnemyBullets.Add(bullet);
        }
        else
        {
            Bullet bulletTemp = desactivePlayerBullets[0];
            bulletTemp.gameObject.SetActive(true);
            bulletTemp.transform.position = position;
            bulletTemp.transform.rotation = rotation;

            desactiveEnemyBullets.Remove(bulletTemp);
        }
    }

    public void DoEnemySpawn(Vector3 position, Quaternion rotation, string enemyType)
    {
        if (enemyType == "Chaser" && allEnemiesChaser.Count < _maxEnemiesPool)
        {
            GameObject newEnemyChaser = Instantiate(_enemyChaserPrefab, position, rotation, _parentEnemyPool.transform);
            allEnemiesChaser.Add(newEnemyChaser);
        }
        else if (enemyType == "Shooter" && allEnemiesShooter.Count < _maxEnemiesPool)
        {
            GameObject newEnemyShooter = Instantiate(_enemyShooterPrefab, position, rotation, _parentEnemyPool.transform);
            allEnemiesShooter.Add(newEnemyShooter);
        }
        else
        {
            GameObject enemyTemp = null;

            switch(enemyType)
            {
                case "Chaser":
                    enemyTemp = desactiveEnemyChaser[0];
                    desactiveEnemyChaser.Remove(enemyTemp);
                    break;
                case "Shooter":
                    enemyTemp = desactiveEnemyShooter[0];
                    desactiveEnemyShooter.Remove(enemyTemp);
                    break;
                default:
                    return;
            }

            enemyTemp.gameObject.SetActive(true);
            enemyTemp.transform.position = position;
            enemyTemp.transform.rotation = rotation;
        }
    }
}
