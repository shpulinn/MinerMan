using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveController : MonoBehaviour
{

    private Camera _camera;
    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _isRunning = false;

    [SerializeField] private float controlDelay = .2f;
    private float _timer;
    
    private void Start () {
        _camera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
	
    private void Update () {
        if (InputManager.Instance.Tap)
        {
            Ray ray = _camera.ScreenPointToRay(InputManager.Instance.MousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Move player to click position
                _agent.SetDestination(hit.point);
                _animator.SetBool("isRunning", true);
                _isRunning = true;
            }
        }

        if (_isRunning == false) return;
        
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    _animator.SetBool("isRunning", false);
                    _isRunning = false;
                }
            }
        }

    }
}