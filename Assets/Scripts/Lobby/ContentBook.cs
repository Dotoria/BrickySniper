using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContentBook : InputManager
{
    public ScriptableObject bookSO;
    public GameObject paperPrefab;
    private GameObject _paper;
    
    public Image dragFlagImage;
    private Transform _preview;
    
    private ScrollRect _scroll;
    private RectTransform _rect;
    public float longPressDuration = 0.2f;
    public float draggingDuration = 0.5f;
    
    private float pointerDownTimer = 0f;
    private Vector3 offset;
    private Transform _parentCanvas;
    
    private void Awake()
    {
        TryGetComponent(out Image bookImage);
        _parentCanvas = transform.parent;
        _rect = GetComponent<RectTransform>();
        
        Transform paperParent = GameObject.Find("ContentUI").transform;
        
        _paper = Instantiate(paperPrefab, paperParent);
        _paper.TryGetComponent(out Image paperImage);
        Button paperButton = _paper.GetComponentInChildren<Button>();
        paperButton.onClick.AddListener(() => UIManager.Instance.CloseUI(_paper));

        _preview = transform.parent.GetChild(0);
        _preview.gameObject.SetActive(false);
        
        dragFlagImage.fillAmount = 0f;
        dragFlagImage.gameObject.SetActive(false);

        _scroll = GetComponentsInParent<ScrollRect>()[0];
        if (bookSO is CellScriptableObject cellSO)
        {
            bookImage.sprite = cellSO.bookSprite;
            paperImage.sprite = cellSO.prefabSprite;
        }
        else if (bookSO is EnemyScriptableObject enemySO)
        {
            bookImage.sprite = enemySO.bookSprite;
            paperImage.sprite = enemySO.prefabSprite;
        }
        else if (bookSO is SkinScriptableObject skinSO)
        {
            // bookImage.sprite = skinSO.bookSprite;
            // paperImage.sprite = skinSO.prefabSprite;
        }
        
        _paper.SetActive(false);
        
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
        
        _preview.SetSiblingIndex(transform.GetSiblingIndex());
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
            _scroll.enabled = false;
            
            dragFlagImage.gameObject.SetActive(false);
            transform.SetParent(canvas.transform);
            
            base.HandleDragMove(pos, input, init);
            float deltaX = (inputPosition.x - initPosition.x);
            float clampedX = Mathf.Clamp(pos.x + deltaX, -1800, 1800);
            _rect.position = new Vector3(clampedX, pos.y + inputPosition.y - initPosition.y);
            
            _preview.gameObject.SetActive(true);
            _preview.SetSiblingIndex(CalculateSiblingIndex(clampedX));
        }
    }

    protected override void HandleDragEnd()
    {
        dragFlagImage.gameObject.SetActive(false);
        dragFlagImage.fillAmount = 0f;
        
        _preview.gameObject.SetActive(false);
        
        transform.SetParent(_parentCanvas);
        transform.SetSiblingIndex(CalculateSiblingIndex(transform.GetComponent<RectTransform>().position.x));
        _scroll.enabled = true;

        // 짧게 터치하면 paper 활성화
        if (pointerDownTimer < longPressDuration)
        {
            _paper.SetActive(true);
        }

        base.HandleDragEnd();
    }
    
    private int CalculateSiblingIndex(float xRectPos)
    {
        int newIndex = 0;
        for (int i = 0; i < _parentCanvas.childCount; i++)
        {
            _parentCanvas.GetChild(i).TryGetComponent(out RectTransform rect);
            if (xRectPos > rect.position.x)
            {
                newIndex = i;
            }
            else
            {
                break;
            }
        }
        return newIndex;
    }
}
