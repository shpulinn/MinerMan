using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool _canControl = true;
    private Vector3 _newPosition;
    [SerializeField] private float movementTime;
    private Camera _cam;
    private Vector3 _dragPosStart;
    private Vector3 _dragPosCurrent;

    private void Start()
    {
        _cam = GetComponent<Camera>();
        _newPosition = transform.position;
    }

    private void Update()
    {
        if (_canControl) {
            HandleMouseInput();
        }
    }

    private void HandleMouseInput()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _cam.ScreenPointToRay(InputManager.Instance.MousePosition);
        float entry;
        if (InputManager.Instance.Tap) { // if press (touch) 1 time
            if (plane.Raycast(ray, out entry))
            {
                _dragPosStart = ray.GetPoint(entry);
            }
        }
        if (InputManager.Instance.Swipe) { // if hold press (touch)
            if (plane.Raycast(ray, out entry)) {
                _dragPosCurrent = ray.GetPoint(entry);
        
                _newPosition = transform.position + _dragPosStart - _dragPosCurrent;
            }
        }

        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime);
    }
}
