using System;
using UnityEngine;
using UnityEngine.AI;

/* This component moves our player using a NavMeshAgent. */

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{

    [SerializeField] private float _movementSpeed = 4.0f;

    private Transform _target;		// Target to follow
    private NavMeshAgent _agent;		// Reference to our agent
    private Animator _animator;
    private Rigidbody _rb;

    private bool _isRunning = false;
    private bool _isFighting = false;
    private bool _isDead = false;
    private bool _canMining = true;
    private bool _isRocketing = false;
    private bool _canMiningEnergy = false;
    private bool _canMiningCrystal = false;

    private int animRunningBool;
    private int animFightingBool;
    private int animDeadBool;

    private BaseState _state;

    private GameObject _destroyable;

    public bool IsRunning => _isRunning;

    public bool IsFighting => _isFighting;

    public bool IsDead => _isDead;

    public bool IsRocketing => _isRocketing;
    
    public bool CanMiningEnergy
    {
        get { return _canMiningEnergy; }
        set { _canMiningEnergy = value; }
    }
    
    public bool CanMiningCrystal
    {
        get { return _canMiningCrystal; }
        set { _canMiningCrystal = value; }
    }

    private const string EnemyTag = "Enemy";
    private const string CrystalTag = "Crystal";
    private const string EnergyCrystalTag = "EnergyCrystal";

    // Get references
    void Start () {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _movementSpeed;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        _state = GetComponent<IdleState>();
        _state.Construct();

        AssignAnimationID();
    }

    private void AssignAnimationID()
    {
        animFightingBool = Animator.StringToHash("isFighting");
        animRunningBool = Animator.StringToHash("isRunning");
        animDeadBool = Animator.StringToHash("isDead");
    }

    void Update ()
    {
        // If we have a target
        // if (_target != null)
        // {
        //     // Move towards it and look at it
        //     _agent.SetDestination(_target.position);
        //     FaceTarget();
        // }
        
        UpdateMotor();
    }

    private void UpdateMotor()
    {
        //Debug.Log("Current state is: " + _state.stateName);
        // Are we changing state?
        _state.Transition();
        
        if (_isRunning == false) return;
        
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    _animator.SetBool(animRunningBool, false);
                    _isRunning = false;
                }
            }
        }
    }

    public void StopMoving()
    {
        _agent.isStopped = true;
        _animator.SetBool(animRunningBool, false);
        _isRunning = false;
    }
	
    public void MoveToPoint (Vector3 point)
    {
        _agent.isStopped = false;
        _agent.SetDestination(point);
        _animator.SetBool(animRunningBool, true);
        _isRunning = true;
    }

    public void MoveToDirection(Vector3 direction)
    {
        _agent.isStopped = false;
        _agent.SetDestination(direction);
        _animator.SetBool(animRunningBool, true);
        _isRunning = true;
    }
    
    public void ChangeState(BaseState state)
    {
        _state.Destruct();
        _state = state;
        _state.Construct();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_canMining == false)
            return;

        if (other.CompareTag(EnergyCrystalTag) && _isRunning == false)
        {
            _canMiningEnergy = true;
            _destroyable = other.gameObject;
            transform.LookAt(_destroyable.transform);
        }
        else _canMiningEnergy = false;

        if (other.CompareTag(CrystalTag) && _isRunning == false)
        {
            _canMiningCrystal = true;
            _destroyable = other.gameObject;
            transform.LookAt(_destroyable.transform);
        }
        else _canMiningCrystal = false;
    }

    public void DestroyMiningObject()
    {
        if (_destroyable != null)
        {
            Destroy(_destroyable);
        }
    }

    // Start following a target
    // public void FollowTarget (Interactable newTarget)
    // {
    //     agent.stoppingDistance = newTarget.radius * .8f;
    //     agent.updateRotation = false;
    //
    //     target = newTarget.interactionTransform;
    // }

    // Stop following a target
    public void StopFollowingTarget ()
    {
        _agent.stoppingDistance = 0f;
        _agent.updateRotation = true;

        _target = null;
    }

    // Make sure to look at the target
    void FaceTarget ()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void TakeGun()
    {
        _animator.SetBool(animFightingBool, true);
        _isFighting = true;
        _canMining = false;
    }

    public void TakePickaxe()
    {
        _animator.SetBool(animFightingBool, false);
        _isFighting = false;
        _canMining = true;
    }

    public void StartRocketing()
    {
        _isRocketing = true;
        _canMining = false;
    }

    public void StopRocketing()
    {
        _isRocketing = false;
        _canMining = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(EnemyTag))
        {
            _isDead = true;
            _animator.SetBool(animDeadBool, true);
        }
    }
}