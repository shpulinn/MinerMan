using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCrystal : MonoBehaviour
{
    [SerializeField] private bool isFirstCrystal;
    private void OnDestroy()
    {
        if (isFirstCrystal) IntroMiningEvent.SendFirstCrystallMined();
            else IntroMiningEvent.SendSecondCrystallMined();
    }
}
