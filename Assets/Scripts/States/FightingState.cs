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
        // other transitions here:
        // Idle state
        // Running state
        // RocketMissile state 
        // Death state
    }
}
