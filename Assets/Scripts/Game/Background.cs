using UnityEngine;

namespace Game
{
    public class Background : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Cell"))
            {
                other.gameObject.TryGetComponent(out Cell cell);
                cell.Destroy();
            }
            else if (other.CompareTag("Enemy"))
            {
                other.gameObject.TryGetComponent(out Enemy enemy);
                enemy.Destroy();
            }
        }
    }
}