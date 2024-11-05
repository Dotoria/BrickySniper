using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    public int maxHealthPoint = 100;
    public int currentHealthPoint;
    private BoxCollider2D _collider;
    
    void Awake()
    {
        currentHealthPoint = maxHealthPoint;
        _collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyManager.Instance.DestroyEnemy(other.gameObject);
            currentHealthPoint -= other.GetComponent<Enemy>().attackPoint;
            Debug.Log("current: " + currentHealthPoint + " ap: " + other.GetComponent<Enemy>().attackPoint);
        }
    }
}
