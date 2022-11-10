using System;
using UnityEngine;

public class RunningState : BaseState
{
    private IdleState _idleState;
    private FightingState _fightingState;
    private RocketMissileState _rocketMissileState;
    private DeathState _deathState;
    private InputManager _inputManager;

    private void Start()
    {
        _idleState = GetComponent<IdleState>();
        _fightingState = GetComponent<FightingState>();
        _rocketMissileState = GetComponent<RocketMissileState>();
        _deathState = GetComponent<DeathState>();
        _inputManager = InputManager.Instance;
    }

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
        if (_inputManager.Tap)
        {
            MoveToPoint(_inputManager.TapPosition);
        }
        
        // when player reached position
        if (playerMotor.IsRunning == false)
        {
            playerMotor.ChangeState(_idleState);
        }

        if (playerMotor.IsRocketing)
        {
            playerMotor.ChangeState(_rocketMissileState);
        }

        if (playerMotor.IsDead)
        {
            playerMotor.ChangeState(_deathState);
        }

        if (playerMotor.IsFighting)
        {
            playerMotor.ChangeState(_fightingState);
        }
    }
}
