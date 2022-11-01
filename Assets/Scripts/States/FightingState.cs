using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingState : BaseState
{
    public override void Construct()
    {
        stateName = "Fighting";
    }

    public override void Transition()
    {
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
        // Idle state
        // Running state
        // RocketMissile state 
        // Death state
    }
}
