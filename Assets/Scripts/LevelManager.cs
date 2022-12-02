using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelType
{
    Crystal,
    Boss
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private int levelCrystalGoal = 5;
    [SerializeField] private GameObject crystalImage;
    [SerializeField] private GameObject bossLabel;

    [SerializeField] private LevelType _levelType;

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

        switch (_levelType)
        {
            case LevelType.Crystal:
                UIController.Instance.SetGoalAmount(levelCrystalGoal);
                if (bossLabel)
                {
                    bossLabel.SetActive(false);
                }
                crystalImage.SetActive(true);
            break;
            case LevelType.Boss:
                UIController.Instance.SetGoalAmount(1); // 1 boss
                if (crystalImage)
                {
                    crystalImage.SetActive(false);
                }
                bossLabel.SetActive(true);
                break;
        }
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
