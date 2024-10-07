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
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                
                direction = (mousePosition - transform.position).normalized;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

                if ((activeAngle < angle && angle <= 180) || (-(360 - activeAngle) < angle && angle <= -180))
                {
                    angle = activeAngle;
                }
                else if (-180 < angle && angle < -activeAngle)
                {
                    angle = -activeAngle;
                }
                
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                
                // tracer
                Vector2 currentDirection = direction; // 반사된 방향을 저장할 변수
                Vector3 currentPosition = transform.position; // 트레이서의 현재 위치
                int j = 1;
                for (int i = 0; i < numTracers; i++)
                {
                    // 현재 트레이서의 위치 및 방향 업데이트
                    tracerList[i].transform.position = currentPosition + new Vector3(currentDirection.x, currentDirection.y) * (distanceBetweenTracers * j);
                    tracerList[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg - 90));

                    // Raycast로 벽 충돌 감지
                    RaycastHit2D hit = Physics2D.CircleCast(tracerList[i].transform.position, 0.2f, currentDirection, distanceBetweenTracers, wallLayer);
                    if (hit.collider)
                    {
                        Vector2 normal = hit.normal;
                        currentDirection = Vector2.Reflect(currentDirection, normal); // 방향을 반사

                        // 남은 거리를 계산하여 다음 트레이서 위치 설정
                        float remainingDistance = distanceBetweenTracers - hit.distance;
                        currentPosition = hit.point + currentDirection * remainingDistance;

                        // 다음 트레이서에 반사된 방향을 적용
                        if (i < numTracers - 1)
                        {
                            j = 1;
                            tracerList[i + 1].transform.position = currentPosition;
                            tracerList[i + 1].transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg - 90));
                        }
                    }
                    else
                    {
                        // 충돌하지 않았을 때는 정상적으로 방향과 위치를 계속 유지
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
