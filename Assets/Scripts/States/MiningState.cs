using UnityEngine;

public class MiningState : BaseState
{
    [SerializeField] private float minMiningTime = 2.0f;
    [SerializeField] private float maxMiningTime = 4.0f;
    [SerializeField] private float energyMiningValue = 0.2f;

    private float miningTime;

    private PlayerEnergy _playerEnergy;
    private LevelGoal _levelGoal;
    private Animator _animator;

    private int _isMiningAnimBoolID;
    
    private enum CrystalTypes
    {
        Energy,
        Crystal
    }

    private IdleState _idleState;
    private RunningState _runningState;
    private FightingState _fightingState;
    private RocketMissileState _rocketMissileState;
    private DeathState _deathState;
    private InputManager _inputManager;

    private void Start()
    {
        _idleState = GetComponent<IdleState>();
        _runningState = GetComponent<RunningState>();
        _fightingState = GetComponent<FightingState>();
        _rocketMissileState = GetComponent<RocketMissileState>();
        _deathState = GetComponent<DeathState>();
        
        _inputManager = InputManager.Instance;
        
        _playerEnergy = GetComponent<PlayerEnergy>();
        _levelGoal = GetComponent<LevelGoal>();
        _animator = GetComponent<Animator>();

        AssignAnimationsIDs();
    }

    private void AssignAnimationsIDs()
    {
        _isMiningAnimBoolID = Animator.StringToHash("isMining");
    }

    public override void Construct()
    {
        stateName = "Mining";

        miningTime = Random.Range(minMiningTime, maxMiningTime);
        
        if (playerMotor.CanMiningEnergy)
        {
            Mine(CrystalTypes.Energy); 
        }

        if (playerMotor.CanMiningCrystal)
        {
            Mine(CrystalTypes.Crystal);
        }
    }

    public override void Destruct()
    {
        _animator.SetBool(_isMiningAnimBoolID, false);
        playerMotor.CanMiningEnergy = false;
        playerMotor.CanMiningCrystal = false;
    }

    public override void Transition()
    {
        // if click while mining => break mining and run
        if (_inputManager.Tap)
        {
            _runningState.MoveToPoint(_inputManager.TapPosition);
            playerMotor.ChangeState(_runningState);
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

    private void Mine(CrystalTypes type)
    {
        switch (type)
        {
            case CrystalTypes.Energy:
                _animator.SetBool(_isMiningAnimBoolID, true);
                Invoke(nameof(StopMiningEnergy), miningTime);
                break;
            case CrystalTypes.Crystal:
                _animator.SetBool(_isMiningAnimBoolID, true);
                Invoke(nameof(StopMiningCrystal), miningTime);
                break;
        }
    }

    private void StopMiningEnergy()
    {
        if (playerMotor.CanMiningEnergy == false)
        {
            return;
        }
        playerMotor.DestroyMiningObject();
        _playerEnergy.IncreaseEnergy(energyMiningValue);
        playerMotor.ChangeState(_idleState);
    }
    
    private void StopMiningCrystal()
    {
        if (playerMotor.CanMiningCrystal == false)
        {
            return;
        }
        playerMotor.DestroyMiningObject();
        _levelGoal.GatherCrystal();
        playerMotor.ChangeState(_idleState);
    }
}
