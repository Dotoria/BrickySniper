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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Cell"))
        {
            _wallCurrentHealth -= 1;
            
            if (wallMaxHealth / 2 - 1 < _wallCurrentHealth && _wallCurrentHealth <= wallMaxHealth / 2)
            {
                _animator.Play("wallbreak1Animation", -1, 0);
            }
            else if (_wallCurrentHealth <= 0)
            {
                _animator.Play("wallbreak2Animation", -1, 0);
                
                Debug.Log("?? " + WallManager.NoSpawnWallList[0].name);
                if (!WallManager.NoSpawnWallList.Contains(gameObject))
                {
                    WallManager.DestroyWallList.Add(gameObject);
                }
                gameObject.SetActive(false);
            }

            Debug.Log("currentHealth? " + _wallCurrentHealth);
        }
    }
}
