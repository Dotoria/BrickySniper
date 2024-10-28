using UnityEngine;

public class Cell : MonoBehaviour
{
    public CellScriptableObject cellSO;

    private Paddle _paddle;
    private Arrow _arrow;
    
    private SpriteRenderer _prefabSprite;
    private SpriteRenderer _paddleSprite;
    private SpriteRenderer _arrowSprite;
    
    public int healthPoint;
    public int currentHealthPoint;
    public int attackPoint;
    public int movePoint;

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
    }
    
    public void Install()
    {
        _paddle.canDrag = false;
        
        gameObject.name = cellSO.prefabName;
        _prefabSprite.sprite = cellSO.prefabSprite;
        _paddleSprite.sprite = cellSO.paddleSprite;
        _arrowSprite.sprite = cellSO.arrowSprite;
        
        healthPoint = cellSO.healthPoint;
        attackPoint = cellSO.attackPoint;
        movePoint = cellSO.movePoint;
        
        _arrow.gameObject.SetActive(true);
        _arrow.cell = gameObject;
        _paddle.canDrag = false;
        
        gameObject.SetActive(false);
        
        // public AttackLogic attackLogic;
        // public MoveLogic moveLogic;
    }

    public void Shoot(float angle)
    {
        gameObject.SetActive(true);
        _cellRB.rotation = angle;
        _cellRB.velocity = cellSO.movePoint * transform.up;
        _arrow.gameObject.SetActive(false);
    }
}