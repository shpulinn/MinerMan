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

    public override void Transition()
    {
        if (_inputManager.Joystick)
        {
            playerMotor.MoveToDirection(transform.position + _inputManager.MoveVector);
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
