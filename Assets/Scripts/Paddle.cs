using System;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    private BoxCollider2D _collider;
    private Vector2 spriteSize;
    private Vector2 cameraSize;
    private RaycastHit2D hit;

    public GameObject endMenuUI;

    public bool canDrag;
    public int health;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = true;
        spriteSize = _collider.size;
        cameraSize = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
    }

    void Update()
    {
        if (health <= 0)
        {
            if (endMenuUI)
            {
                Time.timeScale = 0f;
                endMenuUI.SetActive(true);
            }
        }
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float clampedX = Mathf.Clamp(mousePosition.x, -cameraSize.x + spriteSize.x / 2, cameraSize.x - spriteSize.x / 2);

        if (canDrag)
        {
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
        else
        {
            _collider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cell"))
        {
            _collider.enabled = true;
            _collider.isTrigger = false;
        }
    }
}