using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void Construct()
    {
        stateName = "Idle";
    }
    
    public override void Transition()
    {
        if (InputManager.Instance.Tap)
        {
            //Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition);
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
                //_motor.MoveToPoint(hit.point);
                GetComponent<RunningState>().MoveToPoint(hit.point);
                playerMotor.ChangeState(GetComponent<RunningState>());
            }
        }

        if (playerMotor.CanMiningEnergy || playerMotor.CanMiningCrystal)
        {
            playerMotor.ChangeState(GetComponent<MiningState>());
        }

        if (playerMotor.IsFighting)
        {
            playerMotor.ChangeState(GetComponent<FightingState>());
        }

        // other transitions here:
        // Mining state
        // Fighting state
        // RocketMissile state 
        // Death state
    }
}
