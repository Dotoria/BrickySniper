using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private CellManager cm;
    
    public CellScriptableObject cellSO;

    private Paddle _paddle;
    
    private SpriteRenderer _prefabSprite;
    private SpriteRenderer _paddleSprite;
    private Animator _animator;
    private PolygonCollider2D _collider;
    
    public int healthPoint;
    public int currentHealthPoint;
    public int attackPoint;

    private Rigidbody2D _cellRB;
    
    private void Awake()
    {
        cm = GameScene.Instance.cellManager;
        
        _prefabSprite = gameObject.GetComponent<SpriteRenderer>();

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (var entry in playerObjects)
        {
            if (entry.GetComponent<Paddle>())
            {
                _paddle = entry.GetComponent<Paddle>();
                _paddleSprite = _paddle.gameObject.GetComponent<SpriteRenderer>();
                break;
            }
        }

        _cellRB = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<PolygonCollider2D>();
    }

    private void FixedUpdate()
    {
        _cellRB.velocity = _cellRB.velocity.normalized * cellSO.movePoint;
        _cellRB.angularVelocity = _cellRB.angularVelocity > 1 ? 1 : _cellRB.angularVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _animator.SetTrigger("Attack");
        }
    }
    
    public void Install()
    {
        gameObject.name = cellSO.prefabName;
        _prefabSprite.sprite = cellSO.prefabSprite;
        _paddleSprite.sprite = cellSO.paddleSprite;
        _animator.runtimeAnimatorController = cellSO.prefabAnimation;
        
        healthPoint = cellSO.healthPoint;
        currentHealthPoint = healthPoint;
        attackPoint = cellSO.attackPoint;
        
        // collider 업데이트
        if (_collider)
        {
            Destroy(_collider);
        }
        _collider = gameObject.AddComponent<PolygonCollider2D>();
        
        gameObject.SetActive(false);

        _paddle.isSetting = true;
        _paddle.cell = this;

        // public AttackLogic attackLogic;
        // public MoveLogic moveLogic;
    }

    public void Shoot()
    {
        gameObject.SetActive(true);
        transform.position = _paddle.gameObject.transform.position + new Vector3(0, 1f, 0);
        _cellRB.velocity = cellSO.movePoint * transform.up;

        // ?? 왜 안돼
        cm.ReloadCell(cm.cellSO.FindIndex(cell => cell == cellSO));
        GameScene.Instance.GainEnergy(-5);
    }
}