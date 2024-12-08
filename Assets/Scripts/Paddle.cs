using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Paddle : MonoBehaviour
{
    private Camera _camera;
    public GameObject touchArea;
    private BoxCollider2D _touchCollider;
    private Vector2 spriteSize = new Vector2(3.8f, 0.8f);
    private Vector2 cameraSize;
    private RaycastHit2D hit;
    private Vector2 _pos;
    private Vector2 _initialPos;

    public bool canDrag;
    public float paddleSpeed;
    private bool _isDragging;

    void Awake()
    {
        _camera = Camera.main;
        cameraSize = new Vector2(_camera.orthographicSize * _camera.aspect, _camera.orthographicSize);
        _isDragging = false;

        _touchCollider = touchArea.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && !_isDragging)
            {
                HandleDragBegin(touch.position);
                _initialPos = touch.position;
                _pos = transform.position;
                _isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && _isDragging)
            {
                HandleDragMove(_pos, touch.position, _initialPos);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                HandleDragEnd();
            }
        }
        else if (Input.GetMouseButtonDown(0) && !_isDragging)
        {
            HandleDragBegin(Input.mousePosition);
            _initialPos = Input.mousePosition;
            _pos = transform.position;
            _isDragging = true;
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            HandleDragMove(_pos, Input.mousePosition, _initialPos);
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
    }

    private void HandleDragMove(Vector3 pos, Vector3 input, Vector3 init)
    {
        Vector3 inputPosition = _camera.ScreenToWorldPoint(input);
        Vector3 initPosition = _camera.ScreenToWorldPoint(init);
        float deltaX = (inputPosition.x - initPosition.x) * paddleSpeed;
        float clampedX = Mathf.Clamp(pos.x + deltaX, -cameraSize.x + spriteSize.x / 2, cameraSize.x - spriteSize.x / 2);

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    private void HandleDragEnd()
    {
        _isDragging = false;
    }
}