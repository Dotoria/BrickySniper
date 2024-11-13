using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Paddle : MonoBehaviour
{
    private Camera _camera;
    public BoxCollider2D boxCollider;
    private Vector2 spriteSize;
    private Vector2 cameraSize;
    private RaycastHit2D hit;

    public GameObject endMenuUI;

    public bool canDrag;
    public int health;
    private bool _isDragging = false;

    void Awake()
    {
        _camera = Camera.main;
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = true;
        spriteSize = boxCollider.size;
        cameraSize = new Vector2(_camera.orthographicSize * _camera.aspect, _camera.orthographicSize);
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

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                HandleDragBegin(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && _isDragging)
            {
                HandleDragMove(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                HandleDragEnd();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            HandleDragBegin(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            HandleDragMove(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleDragEnd();
        }
    }

    private void HandleDragBegin(Vector3 input)
    {
        Vector3 inputPosition = _camera.ScreenToWorldPoint(input);
        int layerMask = LayerMask.GetMask("Paddle");
        hit = Physics2D.Raycast(inputPosition, Vector2.down, Mathf.Infinity, layerMask);

        if (hit.collider == boxCollider)
        {
            _isDragging = true;
        }
    }

    private void HandleDragMove(Vector3 input)
    {
        Vector3 inputPosition = _camera.ScreenToWorldPoint(input);
        float clampedX = Mathf.Clamp(inputPosition.x, -cameraSize.x + spriteSize.x / 2, cameraSize.x - spriteSize.x / 2);

        if (canDrag)
        {
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }

    private void HandleDragEnd()
    {
        _isDragging = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cell"))
        {
            boxCollider.enabled = true;
            boxCollider.isTrigger = false;
        }
    }
}