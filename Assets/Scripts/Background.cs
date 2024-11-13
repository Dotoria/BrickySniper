using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    void Awake()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cell"))
        {
            CellManager.Instance.DestroyCell(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            EnemyManager.Instance.DestroyEnemy(other.gameObject);
        }
    }
}
