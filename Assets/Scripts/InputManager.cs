using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private LayerMask clickableLayerMask;
    [SerializeField] private Joystick _moveJoystick;

    // Input action schemes
    private InputActions _inputActions;

    private bool _tap = false;
    private bool _swipe = false;
    private bool _joystick = false;

    private bool _inputUI;

    private Vector3 _tapPosition;
    private Vector3 _moveVector;

    private Camera _mainCamera;

    #region Properties

    public bool Tap => _tap;
    public bool Swipe => _swipe;

    public bool Joystick => _joystick;

    public Vector3 TapPosition => _tapPosition;
    public Vector3 MoveVector => _moveVector;

    #endregion
    
    public Vector3 MousePosition { get; private set; }

    private void Awake()
    {
        Instance = this;

        SetupControls();
    }

    private void SetupControls()
    {
        _inputActions = new InputActions();
        _mainCamera = Camera.main;
        
        // Register actions
        _inputActions.Main.Tap.performed += ctx => OnTap(ctx);
        _inputActions.Main.Swipe.performed += ctx => OnSwipe(ctx);
    }

    private void ResetInputs()
    {
        _swipe = _tap = false;
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        if (_inputUI)
            return;
        _tap = true;
        Ray ray;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            ray = _mainCamera.ScreenPointToRay(MousePosition);
        } else 
            ray = _mainCamera.ScreenPointToRay(Touchscreen.current.touches[0].position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayerMask))
        {
            _tapPosition = hit.point;
        }
    }
    
    private void OnSwipe(InputAction.CallbackContext ctx)
    {
        _swipe = true;
    }

    private void LateUpdate()
    {
        ResetInputs();
    }

    private void Update()
    {
        MousePosition = Mouse.current.position.ReadValue();
        if (_moveJoystick.Horizontal == 0 && _moveJoystick.Vertical == 0)
        {
            _joystick = false;
            return;
        }

        _joystick = true;
        _moveVector = new Vector3(_moveJoystick.Horizontal, 0, _moveJoystick.Vertical);
    }

    public void ToggleControl(bool value)
    {
        if (value == _inputUI)
            return;
        _inputUI = value;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}
