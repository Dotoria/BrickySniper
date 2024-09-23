using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Player player;
    private BoxCollider2D playerCollider;
    
    public GameObject ball;
    public GameObject tracer;
    
    public bool canDirect = true;
    public Vector2 direction;
    public float activeAngle = 80f;

    public int remainBall;
    private List<GameObject> ballList = new();
    public ObjectPool ballPool;

    private Rigidbody2D ballRB;
    private Rigidbody2D tracerRB;
    private float speed = 10f;
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
    }

    void Update()
    {
        if (canDirect)
        {
            float angle = 0f;
            
            if (Input.GetMouseButton(0))
            {
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
                // tracer 작성 필요
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                canDirect = !canDirect;
                player.canDrag = !player.canDrag;
                remainBall--;

                ballRB.rotation = angle;
                ballRB.velocity = speed * transform.up;
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
