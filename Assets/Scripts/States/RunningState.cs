using UnityEngine;

public class RunningState : BaseState
{
    private IdleState _idleState;
    private FightingState _fightingState;
    private RocketMissileState _rocketMissileState;
    private DeathState _deathState;
    
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
