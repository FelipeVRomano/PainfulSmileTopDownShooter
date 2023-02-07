using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterManager : MonoBehaviour, IGameOver
{
    [SerializeField] Transform _frontalShoot;
    [SerializeField] List<Transform> _sideShoot;
    [SerializeField] float _cooldownShotFrontal;
    [SerializeField] float _cooldownComboSide;

    private float _cooldownComboSideBase;
    private float _cooldownShotFrontalBase;
    private PoolManager _bulletManager;
    private GameController _gmController;
    private bool _gameOver;
    
    private void Start()
    {
        _bulletManager = FindObjectOfType<PoolManager>();
        _gmController = FindObjectOfType<GameController>();

        _gmController.GameOver += GameOver;
    }

    private void OnDestroy()
    {
        _gmController.GameOver -= GameOver;
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
}
