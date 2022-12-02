using System;
using System.Collections.Generic;
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
        UpdateMotor();
    }

    private void UpdateMotor()
    {
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
        else if (other.CompareTag(CrystalTag) && _isRunning == false)
        {
            _canMiningCrystal = true;
            _destroyable = other.gameObject;
            transform.LookAt(_destroyable.transform);
        }
        // else
        // {
        //     _canMiningEnergy = false;
        //     _canMiningCrystal = false;
        // }
    }

    public void DestroyMiningObject()
    {
        if (_destroyable != null)
        {
            Destroy(_destroyable);
        }
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