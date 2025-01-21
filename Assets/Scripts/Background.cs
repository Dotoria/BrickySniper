using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Background : MonoBehaviour
{
    private CellManager cm;
    private EnemyManager em;
    
    void Awake()
    {
        // cm = FindObjectsByType<CellManager>(FindObjectsSortMode.None).FirstOrDefault();
        // em = FindObjectsByType<EnemyManager>(FindObjectsSortMode.None).FirstOrDefault();
        cm = GameScene.Instance.cellManager;
        em = GameScene.Instance.enemyManager;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cell"))
        {
            cm.DestroyCell(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            em.DestroyEnemy(other.gameObject);
        }
    }
}
