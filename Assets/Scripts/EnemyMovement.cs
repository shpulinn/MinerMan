using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class EnemyMovement : MonoBehaviour
{
    // radius of range when enemy would "see" player and start follow
    [SerializeField] private float visionRadius = 7.0f;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 100.0f;
    // the distance between enemy & player to stop following and back to guarding/patrolling
    [SerializeField] private float stopFollowingDistance = 7.0f; 
    [SerializeField] private List<Transform> wayPoints = new List<Transform>();
    private int _currentWayPointIndex;

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

    private bool _isChasing = false;
    private bool _isGuarding = false;
    private bool _isPatrolling = false;

    private void Start()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _meshAgent.speed = moveSpeed;
        _meshAgent.angularSpeed = rotationSpeed;
        _startPosition = transform.position;
        //_player = GameObject.Find("Player");
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = visionRadius;
        _sphereCollider.isTrigger = true;
        _currentWayPointIndex = 0;
        
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
        CheckDistance();
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
        if (other.TryGetComponent(out PlayerMotor playerMotor))
        {
            _player = other.gameObject;
            StartChasingPlayer();
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
        _isPatrolling = true;
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
}
