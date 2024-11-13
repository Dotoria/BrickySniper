using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyScriptableObject enemySO;

    private SpriteRenderer _prefabSprite;
    private Animator _animator;
    private PolygonCollider2D _collider;
    
    public int healthPoint;
    public int currentHealthPoint;
    public int attackPoint;

    private Rigidbody2D _enemyRB;
    
    private void Awake()
    {
        _prefabSprite = gameObject.GetComponent<SpriteRenderer>();
        _enemyRB = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Cell"))
        {
            currentHealthPoint -= other.gameObject.GetComponent<Cell>().attackPoint;
            if (currentHealthPoint <= 0)
            {
                EnemyManager.Instance.DestroyEnemy(gameObject);
            }
            Debug.Log("enemy health? " + currentHealthPoint);
        }
    }

    public void Install()
    {
        gameObject.name = enemySO.prefabName;
        _prefabSprite.sprite = enemySO.prefabSprite;
        _animator.runtimeAnimatorController = enemySO.prefabAnimation;
        
        healthPoint = enemySO.healthPoint;
        currentHealthPoint = healthPoint;
        attackPoint = enemySO.attackPoint;

        // collider 업데이트
        if (_collider)
        {
            Destroy(_collider);
        }
        _collider = gameObject.AddComponent<PolygonCollider2D>();
    }

    public void Shoot(Vector3 spawnPos, Vector3 spawnDir)
    {
        Install();
        transform.position = spawnPos;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, spawnDir);
        _enemyRB.velocity = enemySO.movePoint * transform.up;
    }
}
