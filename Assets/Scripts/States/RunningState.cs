using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : BaseState
{
    public override void Construct()
    {
        stateName = "Run";
    }
    public override void MoveToPoint(Vector3 point)
    {
        playerMotor.MoveToPoint(point);
    }

    public override void Transition()
    {
        // player can change direction while running
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
                MoveToPoint(hit.point);
            }
        }
        
        // when player reached position
        if (playerMotor.IsRunning == false)
        {
            playerMotor.ChangeState(GetComponent<IdleState>());
        }

        if (playerMotor.IsRocketing)
        {
            playerMotor.ChangeState(GetComponent<RocketMissileState>());
        }

        if (playerMotor.IsDead)
        {
            playerMotor.ChangeState(GetComponent<DeathState>());
        }
        // other transitions here:
        // Fighting state
        // RocketMissile state 
        // Death state
    }
}
