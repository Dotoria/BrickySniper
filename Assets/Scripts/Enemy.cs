using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyManager em;
    
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
        em = GameScene.Instance.enemyManager;
        
        _prefabSprite = gameObject.GetComponent<SpriteRenderer>();
        _enemyRB = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<PolygonCollider2D>();
    }

    void FixedUpdate()
    {
        _enemyRB.velocity = _enemyRB.velocity.normalized * enemySO.movePoint;
        _enemyRB.angularVelocity = _enemyRB.angularVelocity > 1 ? 1 : _enemyRB.angularVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Cell"))
        {
            Cell attackCell = other.gameObject.GetComponent<Cell>();
            currentHealthPoint -= attackCell.attackPoint;
            
            // bigCell 한테 죽은 것일 때
            if (attackCell.attackLogic == AttackLogic.Phagocytosis)
            {
                em.DestroyEnemy(gameObject);
            }
            // bigCell 한테 죽은 것이 아닐 때
            else if (currentHealthPoint <= 0)
            {
                StartCoroutine(DeadByCell());
            }
            else
            {
                _animator.SetTrigger("Damage");
            }
            
            GameScene.Instance.GainScore(500);
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
        transform.rotation = Quaternion.LookRotation(Vector3.forward, -spawnDir);
        _enemyRB.velocity = enemySO.movePoint * Vector3.down;
    }

    IEnumerator DeadByCell()
    {
        _animator.SetTrigger("Death");
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        em.DestroyEnemy(gameObject);
    }
}
