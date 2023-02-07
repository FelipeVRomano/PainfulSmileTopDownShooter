using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    public enum CharacterType
    {
        EnemyChaser,
        EnemyShooter,
        Player,
    }

    [Header("CHARACTER LIFE INFO")]
    [SerializeField] private int _life;
    [SerializeField] CharacterType _characterType;
    [SerializeField] Image _lifeBarImage;

    int _lifeBase;
    PoolManager _poolManager;
    GameController _gmController;

    private void Start()
    {
        if(_characterType == CharacterType.EnemyChaser || _characterType == CharacterType.EnemyShooter)
        _poolManager = FindObjectOfType<PoolManager>();
        _gmController = FindObjectOfType<GameController>();

        _lifeBase = _life;
    }

    public void TakeDamage(int damage)
    {
        _life -= damage;

        _lifeBarImage.fillAmount = (float) _life / _lifeBase;

        if (_life <= 0)
        {
            DoCharacterDeath();
        }
    }

    void DoCharacterDeath()
    {
        switch (_characterType)
        {
            case CharacterType.EnemyChaser:
                _poolManager.desactiveEnemyChaser.Add(gameObject);
                _gmController.AddScore();
                _life = _lifeBase;
                _lifeBarImage.fillAmount = (float)_life / _lifeBase;
                gameObject.SetActive(false);
                break;
            case CharacterType.EnemyShooter:
                _poolManager.desactiveEnemyChaser.Add(gameObject);
                _gmController.AddScore();
                _life = _lifeBase;
                _lifeBarImage.fillAmount = (float)_life / _lifeBase;
                gameObject.SetActive(false);
                break;
            case CharacterType.Player:
                _gmController.PlayerDied();
                break;
        }
    }
}
