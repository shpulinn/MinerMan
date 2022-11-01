using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningState : BaseState
{
    [SerializeField] private float minMiningTime = 2.0f;
    [SerializeField] private float maxMiningTime = 4.0f;
    [SerializeField] private float energyMiningValue = 0.2f;

    private float miningTime;

    private PlayerEnergy _playerEnergy;
    private LevelGoal _levelGoal;
    
    private enum CrystalTypes
    {
        Energy,
        Crystal
    }
    
    public override void Construct()
    {
        stateName = "Mining";

        miningTime = Random.Range(minMiningTime, maxMiningTime);

        _playerEnergy = GetComponent<PlayerEnergy>();
        _levelGoal = GetComponent<LevelGoal>();

        if (playerMotor.CanMiningEnergy)
        {
            Mine(CrystalTypes.Energy); 
        }

        if (playerMotor.CanMiningCrystal)
        {
            Mine(CrystalTypes.Crystal);
        }
    }

    public override void Destruct()
    {
        GetComponent<Animator>().SetBool("isMining", false);
        playerMotor.CanMiningEnergy = false;
        playerMotor.CanMiningCrystal = false;
    }

    public override void Transition()
    {
        // if click while mining => break mining and run
        if (InputManager.Instance.Tap)
        {
            Ray ray;
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                ray = Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition);
            } else 
                ray = Camera.main.ScreenPointToRay(UnityEngine.InputSystem.Touchscreen.current.touches[0].position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Move player to click position
                GetComponent<RunningState>().MoveToPoint(hit.point);
                playerMotor.ChangeState(GetComponent<RunningState>());
            }
        }
        // other transitions here:
        // Running state
        // Fighting state
        // RocketMissile state 
        // Death state
    }

    private void Mine(CrystalTypes type)
    {
        switch (type)
        {
            case CrystalTypes.Energy:
                GetComponent<Animator>().SetBool("isMining", true);
                Invoke(nameof(StopMiningEnergy), miningTime);
                break;
            case CrystalTypes.Crystal:
                GetComponent<Animator>().SetBool("isMining", true);
                Invoke(nameof(StopMiningCrystal), miningTime);
                break;
        }
    }

    private void StopMiningEnergy()
    {
        if (playerMotor.CanMiningEnergy == false)
        {
            return;
        }
        playerMotor.DestroyMiningObject();
        _playerEnergy.IncreaseEnergy(energyMiningValue);
        playerMotor.ChangeState(GetComponent<IdleState>());
    }
    
    private void StopMiningCrystal()
    {
        if (playerMotor.CanMiningCrystal == false)
        {
            return;
        }
        playerMotor.DestroyMiningObject();
        _levelGoal.GatherCrystal();
        playerMotor.ChangeState(GetComponent<IdleState>());
    }
}
