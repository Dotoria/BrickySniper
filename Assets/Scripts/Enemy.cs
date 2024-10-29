using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyScriptableObject enemySO;

    private SpriteRenderer _prefabSprite;
    private Animator _animator;
    
    public int healthPoint;
    public int currentHealthPoint;
    public int attackPoint;

    private Rigidbody2D _enemyRB;
    
    private void Awake()
    {
        
        _prefabSprite = gameObject.GetComponent<SpriteRenderer>();
        _enemyRB = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void Install()
    {
        gameObject.name = enemySO.prefabName;
        _prefabSprite.sprite = enemySO.prefabSprite;
        _animator.runtimeAnimatorController = enemySO.prefabAnimation;
        
        healthPoint = enemySO.healthPoint;
        currentHealthPoint = healthPoint;
        attackPoint = enemySO.attackPoint;
        
        gameObject.AddComponent<PolygonCollider2D>();
    }

    public void Shoot(Vector3 spawnPos, Vector3 spawnDir)
    {
        Install();
        transform.position = spawnPos;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, spawnDir);
        Debug.Log("pos " + transform.position);
        Debug.Log("dir " + spawnDir);
        _enemyRB.velocity = enemySO.movePoint * transform.up;
    }
}
