using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private int _crystalsGoal = 0;
    private int _currentCrystalsMined = 0;

    private LevelManager _levelManager;

    public static Action BossAction;

    public static void SendBossAction()
    {
        BossAction?.Invoke();
    }

    private void Start()
    {
        _levelManager = LevelManager.Instance;
        _crystalsGoal = _levelManager.LevelCrystalGoal;
    }

    public void GatherCrystal()
    {
        _currentCrystalsMined++;
        UIController.Instance.UpdateCurrentScore(_currentCrystalsMined);
        if (_currentCrystalsMined == _crystalsGoal)
        {
            _levelManager.FinishLevel(LevelManager.FinishTypes.Complete);
        }
    }
}
