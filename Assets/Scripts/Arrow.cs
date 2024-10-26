using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlayMode
{
    Cell,
    Supply,
}

public class Arrow : MonoBehaviour
{
    // player
    private Paddle paddle;
    private BoxCollider2D paddleCollider;
    
    // arrow
    public Vector2 direction;
    public float activeAngle = 80f;
    public PlayMode shootMode = PlayMode.Cell;
    
    // cell
    public GameObject cell;
    private int _poolSize = 10;
    private ObjectPool _cellPool;

    private void Awake()
    {
        ObjectPool.Instance.Add("cell", new ObjectPool(cell, _poolSize));
        _cellPool = ObjectPool.Instance["cell"];
    }

    void OnEnable()
    {
        paddle = GetComponentInParent<Paddle>();
        paddleCollider = paddle.GetComponent<BoxCollider2D>();
        paddleCollider.enabled = false;
        
        paddle.canDrag = true;
    }

    void Update()
    {
        // 조준
        if (!paddle.canDrag)
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
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                
                Time.timeScale = 1f;
                paddle.canDrag = !paddle.canDrag;
            }
        }

        // 장착
        else
        {
            
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
        paddleCollider.enabled = true;
        gameObject.SetActive(false);
    }
}
