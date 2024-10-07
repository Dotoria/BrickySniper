using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ShootMode
{
    Ball,
    Supply,
}

public class Arrow : MonoBehaviour
{
    // player
    private Player player;
    private BoxCollider2D playerCollider;
    
    // tracer
    public GameObject tracer;
    public LayerMask wallLayer;
    private List<GameObject> tracerList = new();
    private ObjectPool tracerPool;
    private float distanceBetweenTracers = 0.2f;
    private int numTracers = 20;
    
    // arrow
    public bool canDirect = true;
    public Vector2 direction;
    public float activeAngle = 80f;
    public ShootMode shootMode = ShootMode.Ball;

    // GameObject - ball
    public GameObject ball;
    public int remainBall;
    private List<GameObject> ballList = new();
    public ObjectPool ballPool;
    private Rigidbody2D ballRB;
    private float speed = 3f;
    private float xOffset = 0.1f;
    
    // GameObject - supply
    public GameObject supply;
    public int remainSupply;
    private List<GameObject> supplyList = new();
    public ObjectPool supplyPool;
    private Rigidbody2D supplyRB;

    private void Awake()
    {
        tracerPool = new ObjectPool(tracer, numTracers);
        ballPool = new ObjectPool(ball, remainBall);
        supplyPool = new ObjectPool(supply, remainSupply);
    }

    void OnEnable()
    {
        player = GetComponentInParent<Player>();
        playerCollider = player.GetComponent<BoxCollider2D>();
        playerCollider.enabled = false;
        
        canDirect = true;
        player.canDrag = false;
        
        // BallList 생성
        if (ballList.Count == 0)
        {
            InitializeObjects(ballPool, ballList, remainBall);
        }

        // SupplyList 생성
        if (supplyList.Count == 0)
        {
            InitializeObjects(supplyPool, supplyList, remainSupply);
        }

        // 첫 번째 Object 장착
        if (shootMode == ShootMode.Ball && ballList.Count > 0)
        {
            ShiftObjects(ballList);
            ballRB = ballList[0].GetComponent<Rigidbody2D>();
        }
        else if (shootMode == ShootMode.Supply && supplyList.Count > 0)
        {
            ShiftObjects(supplyList);
            supplyRB = supplyList[0].GetComponent<Rigidbody2D>();
        }
        
        // tracer
        tracerList = new List<GameObject>();
        for (int i = 0; i < numTracers; i++)
        {
            GameObject tracerObject = tracerPool.GetFromPool();
            tracerObject.transform.position = transform.position;
            tracerObject.transform.SetParent(transform);
            tracerList.Add(tracerObject);
        }
        // Supply 장착 위치
        if (shootMode == ShootMode.Supply)
        {
            GameObject supplyObject = supplyPool.GetFromPool();
            supplyList[0] = supplyObject;
            supplyObject.transform.position = transform.position;
            supplyObject.transform.SetParent(null);
            tracerList.Add(supplyObject);
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
                
                // mouse 입력
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
                for (int i = 0; i < tracerList.Count; i++)
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
                        if (i < tracerList.Count - 1) j = 1;
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
                
                tracerPool.ReturnToPool(tracerPool);
                tracerList.Clear();

                if (shootMode == ShootMode.Ball)
                {
                    remainBall--;
                    ballRB.rotation = angle;
                    ballRB.velocity = speed * transform.up;
                    ballRB.transform.SetParent(null);
                    
                    StartCoroutine(MakeCollider());
                    ballList.RemoveAt(0);
                }
                else if (shootMode == ShootMode.Supply)
                {
                    remainSupply--;
                    
                    StartCoroutine(MakeCollider());
                    supplyList.RemoveAt(0);
                }
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
        tracerList.Clear();
        gameObject.SetActive(false);
    }

    void InitializeObjects(ObjectPool objectPool, List<GameObject> objectList, int remainObject)
    {
        Vector3 pos = new Vector3(transform.position.x - 0.3f, transform.position.y - 0.3f);
        for (int i = 0; i < remainObject; i++)
        {
            GameObject newObject = objectPool.GetFromPool();
            newObject.transform.position = pos;
            newObject.transform.SetParent(player.transform, true);
            newObject.GetComponent<Collider2D>().enabled = false;
            objectList.Add(newObject);
            pos.x += xOffset;
        }
    }
    
    void ShiftObjects(List<GameObject> objectList)
    {
        if (objectList.Count > 1)
        {
            for (int i = objectList.Count - 1; i > 0; i--)
            {
                Vector3 newPos = objectList[i - 1].transform.position;
                objectList[i].transform.position = newPos;
            }
        }

        GameObject firstObject = objectList[0];
        if (shootMode == ShootMode.Ball) firstObject.transform.position = transform.position;
        firstObject.GetComponent<Collider2D>().enabled = true;
    }

    void AddObject(ObjectPool objectPool, List<GameObject> objectList)
    {
        Vector3 newPos;
        if (objectList.Count == 0)
        {
            newPos = new Vector3(transform.position.x - 0.3f, transform.position.y - 0.3f);
        }
        else
        {
            newPos = objectList[^1].transform.position;
        }
        newPos.x += xOffset;
        GameObject newObject = objectPool.GetFromPool();
        newObject.transform.position = newPos;
        newObject.GetComponent<Collider2D>().enabled = false;
        objectList.Add(newObject);
    }
}
