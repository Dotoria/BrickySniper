using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject energyBar;
    private Slider _slider;
    private TextMeshProUGUI _text;
    
    public int maxEnergyPoint = 40;
    public int currentEnergyPoint;
    
    void Awake()
    {
        _slider = energyBar.GetComponent<Slider>();
        _text = energyBar.GetComponentInChildren<TextMeshProUGUI>();

        currentEnergyPoint = maxEnergyPoint;
        
        _slider.value = maxEnergyPoint;
        _text.text = $"{currentEnergyPoint} / {maxEnergyPoint}";
        SliderUpdate();
    }

    void GainEnergy(int energy)
    {
        int newPoint = currentEnergyPoint + energy;
        currentEnergyPoint = newPoint > maxEnergyPoint ? maxEnergyPoint : newPoint;
        SliderUpdate();
    }

    void LoseEnergy(int energy)
    {
        int newPoint = currentEnergyPoint - energy;
        currentEnergyPoint = newPoint < 0 ? 0 : newPoint;
        SliderUpdate();
    }
    
    private void SliderUpdate()
    {
        _slider.value = currentEnergyPoint * 1f / maxEnergyPoint;
        _text.text = $"{currentEnergyPoint} / {maxEnergyPoint}";
    }
}
