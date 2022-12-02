using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private void Start()
    {
        LevelGoal.BossAction += BossDeath;
    }

    private void BossDeath()
    {
        LevelManager.Instance.FinishLevel(LevelManager.FinishTypes.Complete);
    }

    private void OnDestroy()
    {
        LevelGoal.SendBossAction();
    }
}
