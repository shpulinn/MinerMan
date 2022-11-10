using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealthPoints = 10.0f;
    [SerializeField] private ParticleSystem bloodParticles;
    [SerializeField] private float timeToDestroyAfterDeath = 2.0f;

    private float _currentHP;
    private Animator _animator;
    private EnemyMovement _enemyMovement;

    private int takeDamageAnimationTriggerID;
    private int isDeadAnimationID;

    private void Start()
    {
        _currentHP = maxHealthPoints;
        _animator = GetComponent<Animator>();
        _enemyMovement = GetComponent<EnemyMovement>();

        AssignAnimationsID();
    }

    private void AssignAnimationsID()
    {
        takeDamageAnimationTriggerID = Animator.StringToHash("TakeDamage");
        isDeadAnimationID = Animator.StringToHash("IsDead");
    }

    public void TakeDamage(float damage)
    {
        bloodParticles.Play();
        _animator.SetTrigger(takeDamageAnimationTriggerID);
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _enemyMovement.DeathAction();
        _animator.SetBool(isDeadAnimationID, true);
        Destroy(gameObject, timeToDestroyAfterDeath);
        
        // Destroying this component and EnemyMovement component for "disabling" enemy, 
        // but leave on scene for "timeToDestroyAfterDeath" seconds after death and then disappear
        Destroy(this);
        Destroy(_enemyMovement);
    }
}
