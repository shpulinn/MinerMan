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
    
    public override void Construct()
    {
        stateName = "Mining";

        miningTime = Random.Range(minMiningTime, maxMiningTime);

        _playerEnergy = GetComponent<PlayerEnergy>();

        if (playerMotor.CanMiningEnergy)
        {
            MineEnergy();
        }
    }

    public override void Destruct()
    {
        GetComponent<Animator>().SetBool("isMining", false);
        playerMotor.CanMiningEnergy = false;
    }

    public override void Transition()
    {
        // if click while mining => break mining and run
        if (InputManager.Instance.Tap)
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition);
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

    private void MineEnergy()
    {
        GetComponent<Animator>().SetBool("isMining", true);
        Invoke(nameof(StopMiningEnergy), miningTime);
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
}
