using UnityEngine;

public class Barricade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyManager.Instance.DestroyEnemy(other.gameObject);
            int newPoint = - other.GetComponent<Enemy>().attackPoint;
            GameScene.Instance.GainHealth(newPoint);
        }
    }
}
