using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Barricade : MonoBehaviour
{
    public GameObject healthBar;
    private Slider _slider;
    private TextMeshProUGUI _text;
    
    public int maxHealthPoint = 100;
    public int currentHealthPoint;
    private BoxCollider2D _collider;
    
    void Awake()
    {
        _slider = healthBar.GetComponent<Slider>();
        _text = healthBar.GetComponentInChildren<TextMeshProUGUI>();

        currentHealthPoint = maxHealthPoint;
        _collider = GetComponent<BoxCollider2D>();
        
        _slider.value = maxHealthPoint;
        _text.text = $"{currentHealthPoint} / {maxHealthPoint}";
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyManager.Instance.DestroyEnemy(other.gameObject);
            currentHealthPoint -= other.GetComponent<Enemy>().attackPoint;

            _slider.value = currentHealthPoint * 1f / maxHealthPoint;
            _text.text = $"{currentHealthPoint} / {maxHealthPoint}";
        }
    }
}
