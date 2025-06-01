using System;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    private GameScene _game;
    [SerializeField] private EnemyManager em;

    private void Awake()
    {
        _game = FindAnyObjectByType<GameScene>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.TryGetComponent(out Enemy enemy);
            int newPoint = - enemy.attackPoint;
            _game.GainHealth(newPoint);
            enemy.Destroy();
        }
    }
}
