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
        if (playerMotor.IsRunning == false)
        {
            playerMotor.ChangeState(GetComponent<IdleState>());
        }
        
        // other transitions here:
        // Fighting state
        // RocketMissile state 
        // Death state
    }
}
