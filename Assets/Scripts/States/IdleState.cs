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
            Debug.LogWarning("Here");
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Move player to click position
                //_motor.MoveToPoint(hit.point);
                GetComponent<RunningState>().MoveToPoint(hit.point);
                playerMotor.ChangeState(GetComponent<RunningState>());
            }
        }
        
        // other transitions here:
        // Mining state
        // Fighting state
        // RocketMissile state 
        // Death state
    }
}
