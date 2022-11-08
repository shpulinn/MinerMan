using UnityEngine;

public class IdleState : BaseState
{
    private RunningState _runningState;
    private MiningState _miningState;
    private FightingState _fightingState;
    private RocketMissileState _rocketMissileState;
    private DeathState _deathState;

    private void Start()
    {
        _runningState = GetComponent<RunningState>();
        _miningState = GetComponent<MiningState>();
        _fightingState = GetComponent<FightingState>();
        _rocketMissileState = GetComponent<RocketMissileState>();
        _deathState = GetComponent<DeathState>();
    }

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
                _runningState.MoveToPoint(hit.point);
                playerMotor.ChangeState(_runningState);
            }
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
