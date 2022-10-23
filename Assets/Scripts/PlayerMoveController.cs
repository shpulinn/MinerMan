using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerMoveController : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private float controlDelay = .2f;

    private PlayerMotor _motor;
    
    private void Start () {
        _camera = Camera.main;
        _motor = GetComponent<PlayerMotor>();
    }
	
    private void Update () {
        if (InputManager.Instance.Tap)
        {
            Ray ray = _camera.ScreenPointToRay(InputManager.Instance.MousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Move player to click position
                _motor.MoveToPoint(hit.point);
            }
        }
    }
}