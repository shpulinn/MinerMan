using UnityEngine;

public class RocketMissileState : BaseState
{
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float energyCost = 0.6f;

    private PlayerEnergy _playerEnergy;

    private IdleState _idleState;
    private DeathState _deathState;
    private InputManager _inputManager;

    private void Start()
    {
        _playerEnergy = GetComponent<PlayerEnergy>();

        _idleState = GetComponent<IdleState>();
        _deathState = GetComponent<DeathState>();
        _inputManager = InputManager.Instance;
    }

    public override void Construct()
    {
        stateName = "Rocket missile";
        playerMotor.StopMoving();
    }
    
    // _______------------____________
    // Animation: player with telephone (radio) calls for air strike
    // ---------_________-------------

    public override void Transition()
    {
        if (_inputManager.Tap)
        {
            // Instantiate rocket above tap point 
            Quaternion rot = new Quaternion(0, 0, 180, -1);
            if (_playerEnergy.CurrentEnergy >= energyCost)
            {
                _playerEnergy.DecreaseEnergy(energyCost);
                Instantiate(rocketPrefab, _inputManager.TapPosition + Vector3.up * 10, rot);
            } else UIController.Instance.ShowInfoScreen();
        }

        if (playerMotor.IsRocketing == false)
        {
            playerMotor.ChangeState(_idleState);
        }

        if (playerMotor.IsDead)
        {
            playerMotor.ChangeState(_deathState);
        }
    }
}
