using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContentBook : InputManager
{
    public GameObject paperPrefab;
    [HideInInspector] public GameObject paper;
    public Image dragFlagImage;
    
    private RectTransform _rect;
    public float longPressDuration = 0.1f;
    public float draggingDuration = 0.5f;
    
    private float pointerDownTimer = 0f;
    private Vector3 offset;
    private Transform _parentCanvas;
    
    private void Awake()
    {
        _parentCanvas = transform.parent;
        _rect = GetComponent<RectTransform>();
        
        paper = Instantiate(paperPrefab, transform);
        paper.SetActive(false);
        
        dragFlagImage.fillAmount = 0f;
        dragFlagImage.gameObject.SetActive(false);
        
        base.Initialize(Camera.main, layerName: "UI");
    }

    private void Update()
    {
        if (_isDragging) pointerDownTimer += Time.deltaTime;
        base.InputUpdate(_rect.position);
    }

    protected override void HandleDragBegin(Vector3 input)
    {
        base.HandleDragBegin(input);
        pointerDownTimer = 0f;
        transform.parent.parent.parent.TryGetComponent(out ScrollRect scroll);
        Debug.Log("parentImage: " + scroll.name);
        scroll.enabled = false;
    }

    protected override void HandleDragMove(Vector3 pos, Vector3 input, Vector3 init)
    {
        if (pointerDownTimer < draggingDuration)
        {
            if ((input - init).magnitude > 300f)
            {
                HandleDragEnd();
                return;
            }
            
            if (pointerDownTimer > longPressDuration)
            {
                dragFlagImage.gameObject.SetActive(true);
                dragFlagImage.fillAmount += Time.deltaTime / draggingDuration;
            }
        }
        else
        {
            dragFlagImage.gameObject.SetActive(false);
            transform.SetParent(canvas.transform);
            
            base.HandleDragMove(pos, input, init);
            float deltaX = (inputPosition.x - initPosition.x);
            float clampedX = Mathf.Clamp(pos.x + deltaX, -1800, 1800);

            _rect.position = new Vector3(clampedX, pos.y);
        }
    }

    protected override void HandleDragEnd()
    {
        dragFlagImage.gameObject.SetActive(false);
        dragFlagImage.fillAmount = 0f;
        
        transform.SetParent(_parentCanvas);
        transform.parent.parent.parent.TryGetComponent(out ScrollRect scroll);
        scroll.enabled = true;

        // 짧게 터치하면 paper 활성화
        if (pointerDownTimer < longPressDuration)
        {
            paper.SetActive(true);
        }

        base.HandleDragEnd();
    }
}
