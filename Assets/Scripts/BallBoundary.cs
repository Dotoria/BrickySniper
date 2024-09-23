using System;
using Unity.VisualScripting;
using UnityEngine;

public class BallBoundary : MonoBehaviour
{
    public GameObject arrow;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            arrow.GetComponent<Arrow>().ballPool.ReturnToPool(other.gameObject);
        }
    }
}