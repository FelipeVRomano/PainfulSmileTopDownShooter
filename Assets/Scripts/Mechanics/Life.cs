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
    [Range(2,10)] [SerializeField] private int _life;
    [SerializeField] CharacterType _characterType;
    [SerializeField] Image _lifeBarImage;
    [SerializeField] GameObject _deathAnimObj;

    [Header("CHARACTER HEALTH STATE")]
    [SerializeField] Sprite _fullHealthImg;
    [SerializeField] List<LifeSpriteDamage> _lifeSpriteDamage;
    [SerializeField] GameObject _fireEffectObj;

    SpriteRenderer _spriteRenderer;
    PoolManager _poolManager;

    private int _lifeBase;
    private bool _characterIsDead;

    public System.Action CharacterDeath;
    public int ActualLife => _life;

    private void Start()
    {
        if (_characterType == CharacterType.EnemyChaser || _characterType == CharacterType.EnemyShooter)
            _poolManager = FindObjectOfType<PoolManager>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _fullHealthImg;
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
        else
        {
            DoCharacterSpriteChange();
        }
    }

    void DoCharacterSpriteChange()
    {
        int getCurrentValue = _life;
        int indexOfSprite = 0;
        bool needToChangeSprite = false;

        for(int i = 0; i < _lifeSpriteDamage.Count; i++)
        {
            if (_lifeSpriteDamage[i].spriteLifeIndex >= _life)
            {
                if(!needToChangeSprite)
                {
                    getCurrentValue = _lifeSpriteDamage[i].spriteLifeIndex;
                    indexOfSprite = i;
                    needToChangeSprite = true;
                }
                else
                {
                    if (Mathf.Min(getCurrentValue, _lifeSpriteDamage[i].spriteLifeIndex) == _lifeSpriteDamage[i].spriteLifeIndex)
                    {
                        getCurrentValue = _lifeSpriteDamage[i].spriteLifeIndex;
                        indexOfSprite = i;
                    }
                }
            }
        }

        if (needToChangeSprite)
        {
            _spriteRenderer.sprite = _lifeSpriteDamage[indexOfSprite].spriteLifeImage;
        }

        if(_life == 1)
        {
            _fireEffectObj.SetActive(true);
        }   
    }

    void DoCharacterDeath()
    {
        StartCoroutine(ExecuteCharacterDeath(.5f, _characterType));
    }

    IEnumerator ExecuteCharacterDeath(float waitTime, CharacterType characterType)
    {
        _deathAnimObj.SetActive(true);

        CharacterDeath?.Invoke();

        yield return new WaitForSeconds(waitTime);

        if (characterType == CharacterType.Player)
        {
            GameController.gmController.PlayerDied();
        }

        _deathAnimObj.SetActive(false);
        _fireEffectObj.SetActive(false);
        _spriteRenderer.sprite = _fullHealthImg;

        if (_characterType == CharacterType.EnemyChaser || _characterType == CharacterType.EnemyShooter)
        {
            if(_characterType == CharacterType.EnemyChaser) _poolManager.desactiveEnemyChaser.Add(gameObject);
            else _poolManager.desactiveEnemyChaser.Add(gameObject);
            GameController.gmController.AddScore();
            _life = _lifeBase;
            _lifeBarImage.fillAmount = (float)_life / _lifeBase;

            gameObject.SetActive(false);
        }
    }
}
