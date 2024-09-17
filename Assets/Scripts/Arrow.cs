using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Player player;
    private BoxCollider2D playerCollider;
    
    public GameObject ball;
    public GameObject tracer;
    private GameObject newBall;
    
    public bool canDirect = true;
    public Vector2 direction;
    public float activeAngle = 80f;

    public int remainBall;
    // private bool shootOk = true;

    private Rigidbody2D ballRB;
    private Rigidbody2D tracerRB;
    private float speed = 10f;
    
    void Awake()
    {
        player = GetComponentInParent<Player>();
        playerCollider = player.GetComponent<BoxCollider2D>();
        playerCollider.enabled = false;
        
        // warning message 없앨 방법
        newBall = Instantiate(ball, transform.position, transform.rotation);
        ballRB = newBall.GetComponent<Rigidbody2D>();
        tracerRB = tracer.GetComponent<Rigidbody2D>();
        
        // remainBall 수를 나중에 변경할 수 있도록
        if (remainBall > 0)
        {
            Vector3 pos = new Vector3(transform.position.x - 0.3f, transform.position.y - 0.3f);
            for (int i = 0; i < remainBall - 1; i++)
            {
                pos.x += 0.1f;
                Instantiate(ball, pos, transform.rotation);
                ball.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }

    void Update()
    {
        if (canDirect)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                
                direction = (mousePosition - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

                if ((activeAngle < angle && angle <= 180) || (-(360 - activeAngle) < angle && angle <= -180))
                {
                    angle = activeAngle;
                }
                else if (-180 < angle && angle < -activeAngle)
                {
                    angle = -activeAngle;
                }
                
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                newBall.transform.rotation = transform.rotation;
                // tracer 작성 필요
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                canDirect = !canDirect;
                player.canDrag = !player.canDrag;
                
                ballRB.velocity = speed * transform.up;
                StartCoroutine(MakeCollider());
            }
            
        }
    }

    IEnumerator MakeCollider()
    {
        yield return new WaitForSeconds(0.2f);
        playerCollider.enabled = true;
        gameObject.SetActive(false);
    }
}
