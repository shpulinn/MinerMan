using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingState : BaseState
{
    private IdleState _idleState;
    private RunningState _runningState;
    private RocketMissileState _rocketMissileState;
    private DeathState _deathState;
    private InputManager _inputManager;
    private Animator _animator;
    private int _motionAnimationID;

    private PlayerEnergy _playerEnergy;

    [Header("Gun settings")]
    [SerializeField] private float damage = 4.0f;
    [SerializeField] private float energyCost = 0.2f;
    [SerializeField] private float reloadingTime = 1.0f;
    [Space]
    [SerializeField] private float visionRadius = 5.0f;
    [SerializeField] private ParticleSystem firePartisces;
    [SerializeField] private LayerMask enemyLayerMask;
    [Header("Gizmos showing vision radius for player shooting")]
    [SerializeField] private Color gizmosColor;

    [SerializeField] private List<EnemyHealth> enemyHealths = new List<EnemyHealth>();

    private bool _isReloading = false;

    private void Start()
    {
        _idleState = GetComponent<IdleState>();
        _runningState = GetComponent<RunningState>();
        _rocketMissileState = GetComponent<RocketMissileState>();
        _deathState = GetComponent<DeathState>();

        _animator = GetComponent<Animator>();
        _motionAnimationID = Animator.StringToHash("Motion");
        
        _inputManager = InputManager.Instance;
        _playerEnergy = GetComponent<PlayerEnergy>();
    }
    
    public override void Construct()
    {
        stateName = "Fighting";
    }

    public override void Transition()
    {
        if (_playerEnergy.CurrentEnergy < energyCost)
        {
            UIController.Instance.ShowInfoScreen();
            playerMotor.ChangeState(_idleState);
            return;
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, visionRadius, enemyLayerMask);
        foreach (var col in colliders)
        {
            Transform nearestEnemy = GetClosestEnemy(colliders);
            transform.LookAt(nearestEnemy);
            if (!_isReloading)
            {
                Shoot(nearestEnemy.GetComponent<EnemyHealth>());
            }
        }
        
        if (_inputManager.Joystick)
        {
            playerMotor.MoveToDirection(transform.position + _inputManager.MoveVector);
            if (_inputManager.MoveVector.x - Vector3.forward.x < 0 &&
                _inputManager.MoveVector.z - Vector3.forward.z < 0)
            {
                _animator.SetFloat(_motionAnimationID, -1);
            } else _animator.SetFloat(_motionAnimationID, 1);
        }

        if (playerMotor.IsFighting == false)
        {
            playerMotor.ChangeState(_idleState);
        }

        if (playerMotor.IsDead)
        {
            playerMotor.ChangeState(_deathState);
        }

        if (playerMotor.IsRocketing)
        {
            playerMotor.ChangeState(_rocketMissileState);
        }
    }

    private void Shoot(EnemyHealth enemyHealth)
    {
        if (enemyHealth == null)
        {
            return;
        }
        _playerEnergy.DecreaseEnergy(energyCost);
        playerMotor.StopMoving();
        enemyHealth.TakeDamage(damage);
        firePartisces.Play();
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        _isReloading = true;
        yield return new WaitForSeconds(reloadingTime);
        _isReloading = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawSphere(transform.position, visionRadius);
    }
    
    private Transform GetClosestEnemy (Collider[] enemies)
    {
        List<Transform> enemiesTransform = new List<Transform>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesTransform.Add(enemies[i].transform);
        }
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in enemiesTransform)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }
}
