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
    public int movePoint;

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

    void Install()
    {
        gameObject.name = enemySO.prefabName;
        _prefabSprite.sprite = enemySO.prefabSprite;
        _animator.runtimeAnimatorController = enemySO.prefabAnimation;
        
        healthPoint = enemySO.healthPoint;
        currentHealthPoint = healthPoint;
        attackPoint = enemySO.attackPoint;
        movePoint = enemySO.movePoint;
    }
}
