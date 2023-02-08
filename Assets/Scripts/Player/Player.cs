using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IStopAction
{
    [Header("PLAYER SPEED")]
    [Range(.5f, 5f)] [SerializeField] private float _moveSpeed = 2.5f;
    [Range(20, 50)] [SerializeField] float _rotateSpeed = 40;

    private Life _playerLife;
    private float _moveForward, _rotatePlayer;
    private Vector2 _mapLimitStart, _mapLimitEnd;
    private bool _gameOver;

    void Start()
    {
        _playerLife = GetComponent<Life>();

        _playerLife.CharacterDeath += CharacterDeath;
        _mapLimitStart = GameController.gmController.MapBoundsStart;
        _mapLimitEnd = GameController.gmController.MapBoundsEnd;
        GameController.gmController.GameOver += GameOver;
    }

    private void OnDestroy()
    {
        _playerLife.CharacterDeath -= CharacterDeath;
        GameController.gmController.GameOver -= GameOver;
    }

    void Update()
    {
        if (_gameOver)
            return;

        ReadHorizontalInput();
        ReadVerticalInput();

        MoveForward();
        PlayerRotate();
    }

    private void MoveForward()
    {
        if (_moveForward < 0)
            _moveForward = 0;

        Vector3 movePlayer = _moveForward * Time.deltaTime * transform.up;
        Vector3 newPosition = transform.localPosition + movePlayer;

        newPosition.x = Mathf.Clamp(newPosition.x, _mapLimitStart.x + .5f, _mapLimitEnd.x - .5f);
        newPosition.y = Mathf.Clamp(newPosition.y, _mapLimitStart.y + .5f, _mapLimitEnd.y - .5f);

        transform.localPosition = newPosition;
    }

    private void ReadHorizontalInput()
    {
        _rotatePlayer = Input.GetAxis("Horizontal") * _rotateSpeed;
    }

    private void ReadVerticalInput()
    {
        _moveForward = Input.GetAxis("Vertical") * _moveSpeed;
    }

    private void PlayerRotate()
    {
        Vector3 rotatePlayer = _rotatePlayer * transform.forward * Time.deltaTime;
        transform.Rotate(rotatePlayer * -10);
    }

    public void CharacterDeath()
    {
        _gameOver = true;
    }

    public void GameOver()
    {
        gameObject.SetActive(false);
    }

}
