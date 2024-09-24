using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2 spriteSize;
    private Vector2 cameraSize;
    private RaycastHit2D hit;

    public GameObject endMenuUI;

    public int remainBall;
    public bool canDrag;
    public int health;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            spriteSize = spriteRenderer.sprite.bounds.size;
        }

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
    }
}