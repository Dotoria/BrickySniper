using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Player player;
    public GameObject ball;
    
    public bool canDirect = true;
    public Vector2 direction;
    public float activeAngle = 80f;
    
    void Awake()
    {
        player = GetComponentInParent<Player>();
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
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                canDirect = !canDirect;
                player.canDrag = !player.canDrag;
                
                Instantiate(ball, transform.position, transform.rotation);
                gameObject.SetActive(false);
            }
        }
    }
}
