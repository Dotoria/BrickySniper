using UnityEngine;

public class Barricade : MonoBehaviour
{
    [SerializeField] private EnemyManager em;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            em.DestroyEnemy(other.gameObject);
            int newPoint = - other.GetComponent<Enemy>().attackPoint;
            GameScene.Instance.GainHealth(newPoint);
        }
    }
}
