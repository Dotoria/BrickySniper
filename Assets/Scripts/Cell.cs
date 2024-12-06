using UnityEngine;

public class Cell : MonoBehaviour
{
    public CellScriptableObject cellSO;

    private Paddle _paddle;
    private Arrow _arrow;
    
    private SpriteRenderer _prefabSprite;
    private SpriteRenderer _paddleSprite;
    private SpriteRenderer _arrowSprite;
    private Animator _animator;
    private PolygonCollider2D _collider;
    
    public int healthPoint;
    public int currentHealthPoint;
    public int attackPoint;

    private Rigidbody2D _cellRB;
    
    private void Awake()
    {
        _prefabSprite = gameObject.GetComponent<SpriteRenderer>();

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (var entry in playerObjects)
        {
            if (entry.GetComponent<Paddle>())
            {
                _paddle = entry.GetComponent<Paddle>();
                _paddleSprite = _paddle.gameObject.GetComponent<SpriteRenderer>();
            }
            else if (entry.GetComponent<Arrow>())
            {
                _arrow = entry.GetComponent<Arrow>();
                _arrowSprite = _arrow.gameObject.GetComponent<SpriteRenderer>();
            }
        }

        _cellRB = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<PolygonCollider2D>();
    }
    
    public void Install()
    {
        gameObject.name = cellSO.prefabName;
        _prefabSprite.sprite = cellSO.prefabSprite;
        _paddleSprite.sprite = cellSO.paddleSprite;
        _arrowSprite.sprite = cellSO.arrowSprite;
        _animator.runtimeAnimatorController = cellSO.prefabAnimation;
        
        healthPoint = cellSO.healthPoint;
        currentHealthPoint = healthPoint;
        attackPoint = cellSO.attackPoint;
        
        _arrow.gameObject.SetActive(true);
        _arrow.cellPrefab = gameObject;
        _paddle.canDrag = false;
        
        // collider 업데이트
        if (_collider)
        {
            Destroy(_collider);
        }
        _collider = gameObject.AddComponent<PolygonCollider2D>();
        
        gameObject.SetActive(false);
        
        // public AttackLogic attackLogic;
        // public MoveLogic moveLogic;
    }

    public void Shoot(float angle)
    {
        gameObject.SetActive(true);
        transform.position = _paddle.gameObject.transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _cellRB.velocity = cellSO.movePoint * transform.up;
        _arrow.gameObject.SetActive(false);
        
        GameManager.Instance.GainEnergy(-5);
    }
}