using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public enum PlayMode
{
    Cell,
    Supply,
}

public class Arrow : MonoBehaviour
{
    // paddle
    public GameObject paddleObject;
    private Paddle paddle;
    private BoxCollider2D paddleCollider;
    
    // arrow
    public Vector2 direction;
    public float activeAngle = 80f;
    public PlayMode shootMode = PlayMode.Cell;
    private float angle = 0f;
    
    // cell
    public GameObject cellPrefab;

    void OnEnable()
    {
        paddle = paddleObject.GetComponent<Paddle>();
        paddleCollider = paddle.GetComponent<BoxCollider2D>();
        
        paddle.canDrag = true;
    }
    
    void Update()
    {
        // 조준
        if (!paddle.canDrag)
        {
            Time.timeScale = 0.2f;
            
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    // UI와 겹칩 방지
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        return;
                    }
                    
                    // touch 입력
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 0;
                    
                    // arrow
                    direction = (touchPosition - transform.position).normalized;
                    angle = RestrictAngle(direction);
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
                
                if (touch.phase == TouchPhase.Ended)
                {
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        return;
                    }
                    
                    Time.timeScale = 1f;
                    paddle.canDrag = !paddle.canDrag;
                    paddleCollider.isTrigger = true;

                    cellPrefab.GetComponent<Cell>().Shoot(angle);
                }
            }
            
            else if (Input.GetMouseButton(0))
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
            
            else if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                
                Time.timeScale = 1f;
                paddle.canDrag = !paddle.canDrag;
                paddleCollider.isTrigger = true;

                cellPrefab.GetComponent<Cell>().Shoot(angle);
            }
        }
        else if (paddle.canDrag && !cellPrefab.GetComponent<Cell>().cellSO)
        {
            gameObject.SetActive(false);
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
}
