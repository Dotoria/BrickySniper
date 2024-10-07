using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Arrow : MonoBehaviour
{
    private Player player;
    private BoxCollider2D playerCollider;
    
    public GameObject ball;
    public GameObject tracer;
    public LayerMask wallLayer;
    private List<GameObject> tracerList = new();
    private float distanceBetweenTracers = 0.2f;
    private int numTracers = 20;
    
    public bool canDirect = true;
    public Vector2 direction;
    public float activeAngle = 80f;

    public int remainBall;
    private List<GameObject> ballList = new();
    public ObjectPool ballPool;

    private Rigidbody2D ballRB;
    private Rigidbody2D tracerRB;
    private float speed = 3f;
    private float xOffset = 0.1f;

    private void Awake()
    {
        ballPool = new ObjectPool(ball, remainBall);
    }

    void OnEnable()
    {
        player = GetComponentInParent<Player>();
        playerCollider = player.GetComponent<BoxCollider2D>();
        playerCollider.enabled = false;
        
        canDirect = true;
        player.canDrag = false;
        
        // List 생성
        if (ballList.Count == 0)
        {
            InitializeBalls();
        }

        // 첫 번째 Ball 장착
        if (ballList.Count > 0)
        {
            ShiftBalls();
        }
        
        // tracer
        tracerList = new List<GameObject>();
        for (int i = 0; i < numTracers; i++)
        {
            tracerList.Add(Instantiate(tracer, transform.position, Quaternion.identity, transform));
        }
    }

    void Update()
    {
        if (canDirect)
        {
            Time.timeScale = 0.2f;
            float angle = 0f;
            
            if (Input.GetMouseButton(0))
            {
                // UI와 겹칩 방지
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                
                // arrow
                direction = (mousePosition - transform.position).normalized;
                angle = RestrictAngle(direction);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                
                // tracer
                Vector2 currentDirection = direction;
                Vector3 currentPosition = transform.position;
                int j = 1;
                for (int i = 0; i < numTracers; i++)
                {
                    tracerList[i].transform.position = currentPosition + (Vector3)(currentDirection * (distanceBetweenTracers * j));
                    tracerList[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg - 90));

                    RaycastHit2D hit = Physics2D.CircleCast(tracerList[i].transform.position, 0.2f, currentDirection, distanceBetweenTracers, wallLayer);
                    if (hit.collider)
                    {
                        Vector2 normal = hit.normal;
                        currentDirection = Vector2.Reflect(currentDirection, normal);
                        float remainingDistance = distanceBetweenTracers - hit.distance;
                        currentPosition = hit.point + currentDirection * remainingDistance;
                        if (i < numTracers - 1) j = 1;
                    }
                    else
                    {
                        currentPosition += (Vector3)currentDirection * distanceBetweenTracers;
                    }

                    j++;
                }
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                
                Time.timeScale = 1f;
                canDirect = !canDirect;
                player.canDrag = !player.canDrag;
                remainBall--;

                ballRB.rotation = angle;
                ballRB.velocity = speed * transform.up;
                ballRB.transform.SetParent(null);
                StartCoroutine(MakeCollider());
                
                ballList.RemoveAt(0);
            }
        }
    }

    private float RestrictAngle(Vector2 vector2)
    {
        float angle = Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg - 90;

        if ((activeAngle < angle && angle <= 180) || (-(360 - activeAngle) < angle && angle <= -180))
        {
            angle = activeAngle;
        }
        else if (-180 < angle && angle < -activeAngle)
        {
            angle = -activeAngle;
        }

        return angle;
    }

    IEnumerator MakeCollider()
    {
        yield return new WaitForSeconds(0.2f);
        playerCollider.enabled = true;
        gameObject.SetActive(false);
    }

    void InitializeBalls()
    {
        Vector3 pos = new Vector3(transform.position.x - 0.3f, transform.position.y - 0.3f);
        for (int i = 0; i < remainBall; i++)
        {
            GameObject newBall = ballPool.GetFromPool();
            newBall.transform.position = pos;
            newBall.transform.SetParent(player.transform, true);
            newBall.GetComponent<CircleCollider2D>().enabled = false;
            ballList.Add(newBall);
            pos.x += xOffset;
        }
    }
    
    void ShiftBalls()
    {
        if (ballList.Count > 1)
        {
            for (int i = ballList.Count - 1; i > 0; i--)
            {
                Vector3 newPos = ballList[i - 1].transform.position;
                ballList[i].transform.position = newPos;
            }
        }

        GameObject firstBall = ballList[0];
        firstBall.transform.position = transform.position;
        firstBall.GetComponent<CircleCollider2D>().enabled = true;
        ballRB = firstBall.GetComponent<Rigidbody2D>();
    }

    void AddBall()
    {
        Vector3 newPos = ballList[^1].transform.position;
        newPos.x += xOffset;
        GameObject newBall = ballPool.GetFromPool();
        newBall.transform.position = newPos;
        newBall.GetComponent<CircleCollider2D>().enabled = false;
        ballList.Add(newBall);
    }
}
