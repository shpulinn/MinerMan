using UnityEngine;

public class IdleState : BaseState
{
    private RunningState _runningState;
    private MiningState _miningState;
    private FightingState _fightingState;
    private RocketMissileState _rocketMissileState;
    private DeathState _deathState;
    private InputManager _inputManager;

    private void Start()
    {
        _runningState = GetComponent<RunningState>();
        _miningState = GetComponent<MiningState>();
        _fightingState = GetComponent<FightingState>();
        _rocketMissileState = GetComponent<RocketMissileState>();
        _deathState = GetComponent<DeathState>();
        _inputManager = InputManager.Instance;
    }

    public override void Construct()
    {
        stateName = "Idle";
    }
    
    public override void Transition()
    {
        if (_inputManager.Joystick)
        {
            playerMotor.MoveToDirection(transform.position + _inputManager.MoveVector);
        }

        if (playerMotor.CanMiningEnergy || playerMotor.CanMiningCrystal)
        {
            playerMotor.ChangeState(_miningState);
        }

        if (playerMotor.IsFighting)
        {
            playerMotor.ChangeState(_fightingState);
        }

        if (playerMotor.IsRocketing)
        {
            playerMotor.ChangeState(_rocketMissileState);
        }
        
        if (playerMotor.IsDead)
        {
            playerMotor.ChangeState(_deathState);
        }
    }
}
