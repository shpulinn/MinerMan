using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    // radius of range when enemy would "see" player and start follow
    [SerializeField] private float visionRadius = 7.0f;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 100.0f;
    // the distance between enemy & player to stop following and back to guarding/patrolling
    [SerializeField] private float stopFollowingDistance = 7.0f; 
    [SerializeField] private List<Transform> wayPoints = new List<Transform>();
    [SerializeField] private LayerMask defaultLayerMask;
    private int _currentWayPointIndex;
    private Animator _animator;
    private int _isWalkingAnimationID;
    
    private const string PlayerTag = "Player";

    public enum EnemyType
    {
        Guarding, // just guarding it's position
        Patrolling // patrol the area via waypoints (wayPoints list)
    }

    public EnemyType enemyType;

    private NavMeshAgent _meshAgent;
    private Vector3 _startPosition;
    private GameObject _player;
    private SphereCollider _sphereCollider;
    private CapsuleCollider _capsuleCollider;

    private bool _isChasing = false;
    private bool _isGuarding = false;
    private bool _isPatrolling = false;
    
    private void Start()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _meshAgent.speed = moveSpeed;
        _meshAgent.angularSpeed = rotationSpeed;
        _startPosition = transform.position;
        _sphereCollider = GetComponentInChildren<SphereCollider>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _sphereCollider.radius = visionRadius;
        _sphereCollider.isTrigger = true;
        _currentWayPointIndex = 0;
        _animator = GetComponent<Animator>();
        _isWalkingAnimationID = Animator.StringToHash("IsWalking");
        
        switch (enemyType)
        {
            case EnemyType.Guarding:
                StartGuarding();
                break;
            case EnemyType.Patrolling:
                StartPatrol();
                break;
        }
    }

    private void FixedUpdate()
    {
        // Play walk animation if enemy has path => moving
        if (_meshAgent.hasPath)
        {
            _animator.SetBool(_isWalkingAnimationID, true);
        } else _animator.SetBool(_isWalkingAnimationID, false); // if enemy has no path => standing idle 
        
        if (_isPatrolling)
        {
            CheckDistance();
        }

        if (_isGuarding && !_isChasing)
        {
            if (Vector3.Distance(transform.position, _startPosition) <= _meshAgent.stoppingDistance)
            {
                _animator.SetBool(_isWalkingAnimationID, false);
            }
        }
    }

    private void CheckDistance()
    {
        if (_isPatrolling)
        {
            if (Vector3.Distance(transform.position, wayPoints[_currentWayPointIndex].position) <= _meshAgent.stoppingDistance)
            {
                _currentWayPointIndex++;
                if (_currentWayPointIndex >= wayPoints.Count)
                {
                    _currentWayPointIndex = 0;
                }
                _meshAgent.SetDestination(wayPoints[_currentWayPointIndex].position);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            _player = other.gameObject;
            Ray ray = new Ray(transform.position, _player.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, defaultLayerMask))
            {
                // there is obstacle between player and enemy, skip that 
                return;
            }
            StartChasingPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            StopChasingPlayer();
        }
    }

    private void StartChasingPlayer()
    {
        _isGuarding = false;
        _isPatrolling = false;
        _meshAgent.SetDestination(_player.transform.position);
        if (Vector3.Distance(transform.position, _player.transform.position) >= stopFollowingDistance
            && _isChasing)
        {
            StopChasingPlayer();
        }

        _isChasing = true;
    }

    private void StartPatrol()
    {
        _isPatrolling = true;
        if (Vector3.Distance(transform.position, wayPoints[_currentWayPointIndex].position) > _meshAgent.stoppingDistance)
        {
            _meshAgent.SetDestination(wayPoints[_currentWayPointIndex].position);
        }
    }

    private void StartGuarding()
    {
        _isGuarding = true;
        if (Vector3.Distance(transform.position, _startPosition) > _meshAgent.stoppingDistance)
        {
            _meshAgent.SetDestination(_startPosition);
        }
    }

    private void StopChasingPlayer()
    {
        _isChasing = false;
        switch (enemyType)
        {
            case EnemyType.Guarding:
                StartGuarding();
                break;
            case EnemyType.Patrolling:
                StartPatrol();
                break;
        }
    }

    public void DeathAction()
    {
        _meshAgent.isStopped = true;
        // disabling colliders to reset camera 
        // (because player with camera rotates to nearest enemy collider)
        _capsuleCollider.enabled = false;
        _sphereCollider.enabled = false;
    }
}
