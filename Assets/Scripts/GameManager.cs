using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameObject endMenuUI;
    
    // Score
    public GameObject ScoreUI;
    private TextMeshProUGUI _scoreText;
    private int _score;
    
    // Health
    public GameObject healthBar;
    private Slider _healthSlider;
    private TextMeshProUGUI _healthText;
    public int maxHealthPoint = 30;
    private int _currentHealthPoint;
    
    // Energy
    public GameObject energyBar;
    private Slider _energySlider;
    private TextMeshProUGUI _energyText;
    public int maxEnergyPoint = 40;
    private int _currentEnergyPoint;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Score
        _scoreText = ScoreUI.GetComponent<TextMeshProUGUI>();
        _score = 0;
        
        // Health Bar
        _healthSlider = healthBar.GetComponent<Slider>();
        _healthText = healthBar.GetComponentInChildren<TextMeshProUGUI>();
        _currentHealthPoint = maxHealthPoint;
        GainHealth(0);
        
        // Energy Bar
        _energySlider = energyBar.GetComponent<Slider>();
        _energyText = energyBar.GetComponentInChildren<TextMeshProUGUI>();
        _currentEnergyPoint = maxEnergyPoint;
        GainEnergy(0);
    }

    void Update()
    {
        if (Time.timeScale > 0f)
        {
            GainScore(1);
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver!");
        Time.timeScale = 0f;
        endMenuUI.SetActive(true);
        CellManager.Instance.SetCell(GetComponent<Test>().cellSOList);
    }

    public void GainScore(int amount)
    {
        _score += amount;
        _scoreText.text = _score.ToString("N0");
    }

    public void GainHealth(int amount)
    {
        _currentHealthPoint += amount;
        _currentHealthPoint = _currentHealthPoint < 0 ? 0 : _currentHealthPoint;
        _currentHealthPoint = _currentHealthPoint > maxHealthPoint ? maxHealthPoint : _currentHealthPoint;

        _healthSlider.value = _currentHealthPoint * 1f / maxHealthPoint;
        _healthText.text = $"{_currentHealthPoint} / {maxHealthPoint}";

        if (_currentHealthPoint == 0)
        {
            Instance.GameOver();
        }
    }

    public void GainEnergy(int energy)
    {
        int newPoint = _currentEnergyPoint + energy;
        _currentEnergyPoint = newPoint > maxEnergyPoint ? maxEnergyPoint : newPoint;
        _currentEnergyPoint = newPoint < 0 ? 0 : newPoint;
        
        _energySlider.value = _currentEnergyPoint * 1f / maxEnergyPoint;
        _energyText.text = $"{_currentEnergyPoint} / {maxEnergyPoint}";
    }
}
