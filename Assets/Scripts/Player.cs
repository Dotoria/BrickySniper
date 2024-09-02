using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2 spriteSize;
    private Vector2 cameraSize;

    public bool canDrag = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            spriteSize = spriteRenderer.sprite.bounds.size;
        }

        // 카메라의 orthographicSize와 aspect를 사용하여 화면의 크기를 계산합니다.
        cameraSize = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
    }

    private void OnMouseDrag()
    {
        if (!canDrag) return;
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        float clampedX = Mathf.Clamp(mousePosition.x, -cameraSize.x + spriteSize.x / 2, cameraSize.x - spriteSize.x / 2);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}