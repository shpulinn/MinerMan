using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMissileState : BaseState
{
    public override void Construct()
    {
        stateName = "Rocket missile";
    }
    
    // _______------------____________
    // Animation: player with telephone (radio) calls for air strike
    // ---------_________-------------

    public override void Transition()
    {
        // other transitions here:
        // Idle state
        // Running state
        // Fighting state
        // Death state
    }
}
