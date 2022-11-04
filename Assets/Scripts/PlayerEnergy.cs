using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private Slider energySlider;

    private float _currentEnergy;

    public float CurrentEnergy => _currentEnergy;

    private void Start()
    {
        _currentEnergy = energySlider.value;
        UpdateEnergySlider();
    }

    public void IncreaseEnergy(float amount)
    {
        _currentEnergy += amount;
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
