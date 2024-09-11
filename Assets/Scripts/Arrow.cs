using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Player player;
    public GameObject ball;
    public GameObject tracer;
    
    public bool canDirect = true;
    public Vector2 direction;
    public float activeAngle = 80f;
    private bool shootOk = true;

    public int remainBall;
    private Rigidbody2D ballRB;
    private Rigidbody2D tracerRB;
    private float speed = 10f;
    
    void Awake()
    {
        player = GetComponentInParent<Player>();
        // warning message 없앨 방법
        GameObject newBall = Instantiate(ball, transform.position, transform.rotation);
        ballRB = newBall.GetComponent<Rigidbody2D>();
        tracerRB = tracer.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canDirect)
        {
            // remainBall 수를 나중에 변경할 수 있도록
            // if (remainBall > 0)
            // {
            //     ball = Instantiate(ball, transform.position, transform.rotation);
            //     ballRB.gravityScale = 0f;
            //     ballRB.drag = 0f;
            //     ballRB.angularDrag = 0f;
            //
            //     remainBall--;
            // }
            
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
                ball.transform.rotation = transform.rotation;
                // tracer 작성 필요
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                canDirect = !canDirect;
                player.canDrag = !player.canDrag;
                
                ballRB.velocity = speed * transform.up;
                gameObject.SetActive(false);
            }
            
        }
    }
}
