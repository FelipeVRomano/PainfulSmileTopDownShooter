using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterManager : MonoBehaviour, IStopAction
{
    [Header("PLAYER SHOOTER MANAGER")]
    [SerializeField] Transform _frontalShoot;
    [SerializeField] List<Transform> _sideShoot;
    [Range(1, 5)] [SerializeField] float _cooldownShotFrontal;
    [Range(3, 6)] [SerializeField] float _cooldownComboSide;

    private PoolManager _bulletManager;
    private Life _playerLife;

    private float _cooldownComboSideBase;
    private float _cooldownShotFrontalBase;
    private bool _gameOver;
   
    private void Start()
    {
        _bulletManager = FindObjectOfType<PoolManager>();
        _playerLife = GetComponent<Life>();

        _playerLife.CharacterDeath += CharacterDeath;
        GameController.gmController.GameOver += GameOver;
    }

    private void OnDestroy()
    {
        _playerLife.CharacterDeath -= CharacterDeath;
        GameController.gmController.GameOver -= GameOver;
    }

    private void Update()
    {
        if (_gameOver)
            return;

        CheckCooldown();
        CheckPlayerInput();
    }

    bool CanComboSideShot()
    {
        return _cooldownComboSideBase <= 0;
    }

    bool CanShotFrontal()
    {
        return _cooldownShotFrontalBase <= 0;
    }

    void CheckCooldown()
    {
        if(_cooldownComboSideBase > 0)
        {
            _cooldownComboSideBase -= Time.deltaTime;
        }

        if(_cooldownShotFrontalBase > 0)
        {
            _cooldownShotFrontalBase -= Time.deltaTime;
        }
    }

    void CheckPlayerInput()
    {
        if(Input.GetKeyDown(KeyCode.C) && CanComboSideShot())
        {
            SideComboShot();
        }

        if(Input.GetKeyDown(KeyCode.V) && CanShotFrontal())
        {
            FrontalShot();
        }
    }

    private void FrontalShot()
    {
        _cooldownShotFrontalBase = _cooldownShotFrontal;
        _bulletManager.DoShootPlayer(_frontalShoot.position, transform.rotation);      
    }

    private void SideComboShot()
    {
        _cooldownComboSideBase = _cooldownComboSide;
        for (int i = 0; i < _sideShoot.Count; i++)
        {
            _bulletManager.DoShootPlayer(_sideShoot[i].position, _sideShoot[i].rotation);
        }
    }

    public void GameOver()
    {
        _gameOver = true;
    }

    public void CharacterDeath()
    {
        _gameOver = true;
    }
}
