using UnityEngine;

public class RocketMissileState : BaseState
{
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float energyCost = 0.6f;

    private PlayerEnergy _playerEnergy;

    private IdleState _idleState;
    private DeathState _deathState;
    private InputManager _inputManager;
    private Animator _animator;

    private int IsRocketingAnimID;

    private void Start()
    {
        _playerEnergy = GetComponent<PlayerEnergy>();

        _idleState = GetComponent<IdleState>();
        _deathState = GetComponent<DeathState>();
        _animator = GetComponent<Animator>();
        IsRocketingAnimID = Animator.StringToHash("isRocketing");
        _inputManager = InputManager.Instance;
    }

    public override void Construct()
    {
        stateName = "Rocket missile";
        playerMotor.StopMoving();
        _animator.SetBool(IsRocketingAnimID, true);
        UIController.Instance.HideActiveItem();
    }

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
                playerMotor.ChangeState(_idleState);
                UIController.Instance.ExitRocketing();
            }
            else UIController.Instance.ShowInfoScreen();
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

    public override void Destruct()
    {
        _animator.SetBool(IsRocketingAnimID, false);
        UIController.Instance.ShowHiddenItem();
    }
}
