using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IStopAction
{
    enum EnemyType
    {
        Chaser, 
        Shooter
    }

    [Header("ENEMY CONTROLLER")]
    [Range(.5f, 5)]
    [SerializeField] private float _movementSpeed;
    [Range(.5f, 5)]
    [SerializeField] private float _rotateSpeed;
    [Range(0, 5)] [Tooltip("Add value to avoid enemy trying to get inside player")]
    [SerializeField] private float _stopDistance = 1;

    private Pathfinding _pathfinding;
    private Transform _player;
    private Life _lifeManager;
    private Vector3 _moveForward;
    private Vector3 _pathTarget;

    private bool _playerIsClose;
    private bool _gameOver;
    private bool _characterIsDead;

    public bool PlayerIsClose => _playerIsClose;

    void Awake()
    {
        _pathfinding = GetComponent<Pathfinding>();
    }

    void Start()
    {
        _player = _pathfinding.TargetPos;
        _lifeManager = GetComponent<Life>();

        _lifeManager.CharacterDeath += CharacterDeath;
        GameController.gmController.GameOver += GameOver;
    }

    private void OnDestroy()
    {
        _lifeManager.CharacterDeath -= CharacterDeath;
        GameController.gmController.GameOver -= GameOver;
    }

    private void OnEnable()
    {
        _characterIsDead = false;
    }

    void Update()
    {
        if (_gameOver || _characterIsDead)
            return;

        if (!HasPathToGo())
            return;

        _pathTarget = _pathfinding.pathList[0].vPosition;

        RotateEnemy();
        CheckIfPlayerIsClose();
        Move();
    }

    private bool HasPathToGo()
    {
        return _pathfinding.pathList.Count != 0;
    }

    private void RotateEnemy()
    {
        transform.up = Vector3.Slerp(transform.up, _pathTarget - transform.position, Time.deltaTime * _rotateSpeed);
    }

    private void CheckIfPlayerIsClose()
    {
        if (Vector3.Distance(transform.position, _player.position) > _stopDistance)
        {
            _moveForward = Vector3.up * _movementSpeed;
            _playerIsClose = false;
        }
        else
        {
            _playerIsClose = true;
            _moveForward = Vector3.zero * _movementSpeed;
        }
    }

    void Move()
    {        
        Vector3 moveEnemy = _moveForward.y * Time.deltaTime * transform.up;
        Vector3 newPosition = transform.localPosition + moveEnemy;

        transform.localPosition = newPosition;
    }

    public void GameOver()
    {
        _gameOver = true;
    }

    public void CharacterDeath()
    {
        _characterIsDead = true;
    }
}

