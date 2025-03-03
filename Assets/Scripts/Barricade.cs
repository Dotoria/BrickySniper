using UnityEngine;

public class Barricade : MonoBehaviour
{
    [SerializeField] private EnemyManager em;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.TryGetComponent(out Enemy enemy);
            int newPoint = - enemy.attackPoint;
            GameScene.Instance.GainHealth(newPoint);
            enemy.Destroy();
        }
    }
}
