using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] private int _life;

    public void TakeDamage()
    {
        _life -= 1;

        if(_life <= 0)
        {
            Destroy(gameObject);
        }
    }

}
