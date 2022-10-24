using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* This component moves our player using a NavMeshAgent. */

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour {

    private Transform _target;		// Target to follow
    private NavMeshAgent _agent;		// Reference to our agent
    private Animator _animator;

    private bool _isRunning = false;

    private const string animRunningBoolName = "isRunning";

    private BaseState _state;

    public bool IsRunning => _isRunning;

    // Get references
    void Start () {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _state = GetComponent<IdleState>();
        _state.Construct();
    }

    void Update ()
    {
        Debug.Log("Current state is: " + _state.stateName);
        // If we have a target
        // if (_target != null)
        // {
        //     // Move towards it and look at it
        //     _agent.SetDestination(_target.position);
        //     FaceTarget();
        // }
        
        UpdateMotor();
        
        if (_isRunning == false) return;
        
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    _animator.SetBool(animRunningBoolName, false);
                    _isRunning = false;
                }
            }
        }
    }

    private void UpdateMotor()
    {
        // Are we changing state?
        _state.Transition();
    }
	
    public void MoveToPoint (Vector3 point)
    {
        _agent.SetDestination(point);
        _animator.SetBool(animRunningBoolName, true);
        _isRunning = true;
    }
    
    public void ChangeState(BaseState state)
    {
        _state.Destruct();
        _state = state;
        _state.Construct();
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

}