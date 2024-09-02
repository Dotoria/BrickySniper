using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Arrow : MonoBehaviour
{
    private Player player;
    public bool canDirect = true;
    public Vector2 direction;
    
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
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                canDirect = !canDirect;
                player.canDrag = !player.canDrag;
                gameObject.SetActive(false);
            }
        }
    }
}
