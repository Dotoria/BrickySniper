using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakWall : MonoBehaviour
{
    private Animator _animator;

    public int wallMaxHealth;
    public int _wallCurrentHealth;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _wallCurrentHealth = wallMaxHealth;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cell"))
        {
            if (wallMaxHealth * 1 / 2 - 1 < _wallCurrentHealth && _wallCurrentHealth < wallMaxHealth * 1 / 2)
            {
                _animator.SetTrigger("Break1");
            }
            else if (_wallCurrentHealth < 0)
            {
                _animator.SetTrigger("Break2");
            }

            _wallCurrentHealth -= 1;
            Debug.Log("currentHealth? " + _wallCurrentHealth);
        }
    }
}
