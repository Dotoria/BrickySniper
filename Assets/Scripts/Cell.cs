using UnityEngine;

public class Cell : MonoBehaviour
{
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
        
        Debug.Log(" " + _paddle.cell.transform.position);

        // public AttackLogic attackLogic;
        // public MoveLogic moveLogic;
    }

    public void Shoot()
    {
        gameObject.SetActive(true);
        transform.position = _paddle.gameObject.transform.position;
        transform.position += new Vector3(0, 1f);
        _cellRB.velocity = cellSO.movePoint * transform.up;

        CellManager.Instance.ReloadCell(CellManager.Instance.cellSO.FindIndex(cell => cell == cellSO));
        GameManager.Instance.GainEnergy(-5);
    }
}