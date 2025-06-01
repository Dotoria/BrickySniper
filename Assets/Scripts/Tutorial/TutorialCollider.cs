using System;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    public TutorialScene ts;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Paddle") ts.LoadScript();
    }
}