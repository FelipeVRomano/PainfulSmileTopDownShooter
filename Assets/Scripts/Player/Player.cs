using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IGameOver
{
    [Header("PLAYER SPEED")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] float _rotateSpeed;

    private float _moveForward, _rotatePlayer;
    [SerializeField] private Vector2 _mapLimitStart, _mapLimitEnd;
    private GameController _gmController;

    bool _gameOver;
    void Start()
    {
        _gmController = FindObjectOfType<GameController>();

        _mapLimitStart = _gmController.MapBoundsStart;
        _mapLimitEnd = _gmController.MapBoundsEnd;

        _gmController.GameOver += GameOver;
    }

    private void OnDestroy()
    {
        _gmController.GameOver -= GameOver;
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

    public void GameOver()
    {
        _gameOver = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Life damageable = collision.gameObject.GetComponent<Life>();
        if (damageable != null)
        {
            damageable.TakeDamage(2);
            return;
        }
    }
}
