using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameObject endMenuUI;
    public Animator animator;
    private bool _playing = false;
    
    // Coin
    public TextMeshProUGUI coinText;
    private int _coin;
    
    // Gem
    public TextMeshProUGUI gemText;
    private int _gem;
    
    // Score
    public TextMeshProUGUI scoreText;
    private float _score;
    
    // Health
    public GameObject healthBar;
    private Slider _healthSlider;
    private TextMeshProUGUI _healthText;
    public float maxHealthPoint = 30;
    private float _currentHealthPoint;
    
    // Energy
    public GameObject energyBar;
    private Slider _energySlider;
    private TextMeshProUGUI _energyText;
    public float maxEnergyPoint = 40;
    private float _currentEnergyPoint;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Coin
        _coin = DataManager.Instance.GameData.Coin;
        GainCoin(0);
        
        // Gem
        _gem = DataManager.Instance.GameData.Gem;
        GainGem(0);
        
        // Score
        _score = 0;
        GainScore(0);
        
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

    void Start()
    {
        StartCoroutine(PlayAnimation());
    }

    void Update()
    {
        if (_playing && Time.timeScale > 0f)
        {
            GainScore(0.1f * Time.timeScale);
        }
    }

    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveData();
    }

    private IEnumerator PlayAnimation()
    {
        animator.SetTrigger("EmergePanel");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        _playing = true;
        Destroy(animator.gameObject);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        endMenuUI.SetActive(true);
        
        DataManager.Instance.GameData.Coin = _coin;
        DataManager.Instance.GameData.Gem = _gem;
        if (DataManager.Instance.GameData.HighScore < (int)_score)
        {
            DataManager.Instance.GameData.HighScore = (int) _score;
        }
    }
    
    public void GainCoin(int amount)
    {
        _coin += amount;
        coinText.text = _coin.ToString("N0");
    }
    
    public void GainGem(int amount)
    {
        _gem += amount;
        gemText.text = _gem.ToString("N0");
    }

    public void GainScore(float amount)
    {
        _score += amount;
        scoreText.text = _score.ToString("N0");
    }

    public void GainHealth(float amount)
    {
        float newPoint = _currentHealthPoint + amount;
        _currentHealthPoint = newPoint > maxHealthPoint ? maxHealthPoint : newPoint;
        _currentHealthPoint = newPoint < 0 ? 0 : newPoint;

        _healthSlider.value = _currentHealthPoint * 1f / maxHealthPoint;
        _healthText.text = $"{_currentHealthPoint} / {maxHealthPoint}";

        if (_currentHealthPoint == 0)
        {
            Instance.GameOver();
        }
    }

    public void GainEnergy(int amount)
    {
        float newPoint = _currentEnergyPoint + amount;
        _currentEnergyPoint = newPoint > maxEnergyPoint ? maxEnergyPoint : newPoint;
        _currentEnergyPoint = newPoint < 0 ? 0 : newPoint;
        
        _energySlider.value = _currentEnergyPoint * 1f / maxEnergyPoint;
        _energyText.text = $"{_currentEnergyPoint} / {maxEnergyPoint}";
    }
}
