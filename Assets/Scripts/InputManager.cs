using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private LayerMask clickableLayerMask;
    [SerializeField] private GameObject arrowFX;

    // Input action schemes
    private InputActions _inputActions;

    private bool _tap = false;
    private bool _swipe = false;

    private bool _inputUI;

    private Vector3 _tapPosition;

    private Camera _mainCamera;

    #region Properties

    public bool Tap => _tap;
    public bool Swipe => _swipe;

    public Vector3 TapPosition => _tapPosition;

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
            arrowFX.transform.position = hit.point;
            _tapPosition = hit.point;
        }
        ShowArrows();
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

    private void ShowArrows()
    {
        arrowFX.SetActive(true);
    }
}
