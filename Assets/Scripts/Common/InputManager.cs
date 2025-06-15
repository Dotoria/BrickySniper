using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    public class InputManager : MonoBehaviour
    {
        private Camera _camera;
        protected Canvas canvas;

        protected RaycastHit2D hit;
        private BoxCollider2D _touchCollider;
        private Vector2 _pos;
        private Vector2 _initialPos;
        protected bool _isDragging;

        protected Vector3 inputPosition;
        protected Vector3 initPosition;
        private string _layerName;
        private bool _isUI;

        protected void Initialize(Camera camera, BoxCollider2D touchCollider = null, string layerName = "Default")
        {
            canvas = GetComponentInParent<Canvas>();
            _isUI = (canvas != null); // UI 요소인지 확인
            _camera = camera;
            _touchCollider = touchCollider;
            _layerName = layerName;
        }

        protected virtual void InputUpdate(Vector3 position)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began && !_isDragging)
                {
                    HandleDragBegin(touch.position);
                    _initialPos = touch.position;
                    _pos = position;
                }
                else if (touch.phase == TouchPhase.Moved && _isDragging)
                {
                    HandleDragMove(_pos, touch.position, _initialPos);
                }
                else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && _isDragging)
                {
                    HandleDragEnd();
                }
            }
            else if (Input.GetMouseButtonDown(0) && !_isDragging)
            {
                HandleDragBegin(Input.mousePosition);
                _initialPos = Input.mousePosition;
                _pos = position;
            }
            else if (Input.GetMouseButton(0) && _isDragging)
            {
                HandleDragMove(_pos, Input.mousePosition, _initialPos);
            }
            else if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                HandleDragEnd();
            }
        }

        protected virtual void HandleDragBegin(Vector3 input)
        {
            if (_isUI)
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
                {
                    position = input
                };

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (var result in results)
                {
                    if (result.gameObject == gameObject)
                    {
                        _isDragging = true;
                        return;
                    }
                }
            }
            else
            {
                inputPosition = _camera.ScreenToWorldPoint(input);
                int layerMask = LayerMask.GetMask(_layerName);
                hit = Physics2D.Raycast(inputPosition, Vector2.down, Mathf.Infinity, layerMask);
                if (hit.collider == _touchCollider) _isDragging = true;
            }
        }

        protected virtual void HandleDragMove(Vector3 pos, Vector3 input, Vector3 init)
        {
            if (_isUI)
            {
                inputPosition = input;
                initPosition = init;
            }
            else
            {
                inputPosition = _camera.ScreenToWorldPoint(input);
                initPosition = _camera.ScreenToWorldPoint(init);
            }
        }

        protected virtual void HandleDragEnd()
        {
            _isDragging = false;
        }
    }
}