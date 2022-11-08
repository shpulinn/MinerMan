using System.Collections;
using UnityEngine;

public class FightingState : BaseState
{
    private IdleState _idleState;
    private RunningState _runningState;
    private RocketMissileState _rocketMissileState;
    private DeathState _deathState;

    [SerializeField] private float visionRadius = 5.0f;
    [SerializeField] private float damage = 4.0f;
    [SerializeField] private float reloadingTime = 1.0f;
    [SerializeField] private ParticleSystem firePartisces;

    private bool _isReloading = false;

    private void Start()
    {
        _idleState = GetComponent<IdleState>();
        _runningState = GetComponent<RunningState>();
        _rocketMissileState = GetComponent<RocketMissileState>();
        _deathState = GetComponent<DeathState>();
    }
    
    public override void Construct()
    {
        stateName = "Fighting";
    }

    public override void Transition()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, visionRadius);
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out EnemyHealth enemyHealth))
            {
                transform.LookAt(enemyHealth.transform, Vector3.up);
                if (!_isReloading)
                {
                    Shoot(enemyHealth);
                }
            }
        }
        
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
                // Move player to click position, but still in fight state 
                _runningState.MoveToPoint(hit.point);
            }
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
}
