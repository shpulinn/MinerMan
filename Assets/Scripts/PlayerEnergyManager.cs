using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyManager : MonoBehaviour
{
    public static PlayerEnergyManager Instance;
    
    [SerializeField] private Slider energySlider;

    private float _currentEnergy;

    public float CurrentEnergy => _currentEnergy;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        _currentEnergy = energySlider.value;
        UpdateEnergySlider();
    }

    public void IncreaseEnergy(float amount)
    {
        _currentEnergy += amount;
        _currentEnergy = _currentEnergy > 1 ? 1 : _currentEnergy;
        UpdateEnergySlider();
    }

    public void DecreaseEnergy(float amount)
    {
        _currentEnergy -= amount;
        UpdateEnergySlider();
    }

    private void UpdateEnergySlider()
    {
        energySlider.value = _currentEnergy;
    }
}
