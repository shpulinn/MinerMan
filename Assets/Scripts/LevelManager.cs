using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private int levelCrystalGoal = 5;

    public int LevelCrystalGoal => levelCrystalGoal;
    
    public enum FinishTypes
    {
        Death,
        Complete
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        
        UIController.Instance.SetGoalAmount(levelCrystalGoal);
    }

    public void FinishLevel(FinishTypes type)
    {
        switch (type)
        {
            case FinishTypes.Death:
                break;
            case FinishTypes.Complete:
                UIController.Instance.ShowWinScreen();
                break;
        }
    }
}
