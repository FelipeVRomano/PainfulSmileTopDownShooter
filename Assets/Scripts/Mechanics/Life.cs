using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public enum CharacterType
    {
        EnemyChaser,
        EnemyShooter,
        Player,
    }

    [SerializeField] private int _life;
    CharacterType _characterType;

    public void TakeDamage()
    {
        _life -= 1;

        if(_life <= 0)
        {
            switch(_characterType)
            {
                case CharacterType.EnemyChaser:
                    FindObjectOfType<PoolManager>().desactiveEnemyChaser.Add(gameObject);
                    break;
                case CharacterType.EnemyShooter:
                    FindObjectOfType<PoolManager>().desactiveEnemyChaser.Add(gameObject);
                    break;
                case CharacterType.Player:
                    break;
            }
        }
    }
}
