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
        
        SliderUpdate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyManager.Instance.DestroyEnemy(other.gameObject);
            int newPoint = currentHealthPoint - other.GetComponent<Enemy>().attackPoint;
            currentHealthPoint = newPoint < 0 ? 0 : newPoint;

            SliderUpdate();

            if (currentHealthPoint == 0)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    private void SliderUpdate()
    {
        _slider.value = currentHealthPoint * 1f / maxHealthPoint;
        _text.text = $"{currentHealthPoint} / {maxHealthPoint}";
    }
}
