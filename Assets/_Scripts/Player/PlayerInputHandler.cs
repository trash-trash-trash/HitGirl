using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private HGPlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;

    public event Action<Vector2> AnnounceLookVector2;
    public event Action<Vector2> AnnounceMoveVector2;
    
    public event Action<InputAction.CallbackContext> AnnounceAggressiveAction;
    public event Action<InputAction.CallbackContext> AnnounceUseAction;
    public event Action<InputAction.CallbackContext> AnnounceInventory01;
    public event Action<InputAction.CallbackContext> AnnounceInventory02;
    public event Action<InputAction.CallbackContext> AnnounceNextInventory;
    public event Action<InputAction.CallbackContext> AnnounceToggleRun;
    
    
    private void Awake()
    {
        controls = new HGPlayerControls();

        controls.InGameActionMap.MoveAction.performed += OnMove;
        controls.InGameActionMap.MoveAction.canceled += OnMove;

        controls.InGameActionMap.LookAction.performed += OnLook;
        controls.InGameActionMap.LookAction.canceled += OnLook;

        controls.InGameActionMap.UseAction.performed += OnUse;
        controls.InGameActionMap.UseAction.canceled += OnUse;
        
        controls.InGameActionMap.AggressiveAction.performed += OnAggressiveAction;
        controls.InGameActionMap.AggressiveAction.canceled += OnAggressiveAction;

        controls.InGameActionMap.SelectInventory01.performed += OnInventory01;
        controls.InGameActionMap.SelectInventory01.canceled += OnInventory01;
        
        controls.InGameActionMap.SelectInventory02.performed += OnInventory02;
        controls.InGameActionMap.SelectInventory02.canceled += OnInventory02;
        
        controls.InGameActionMap.NextInventory.performed += NextInventory;
        controls.InGameActionMap.NextInventory.canceled += NextInventory;
        
        controls.InGameActionMap.ToggleRun.performed += OnToggleRun;
        controls.InGameActionMap.ToggleRun.canceled += OnToggleRun;
    }
    
    private void OnInventory01(InputAction.CallbackContext context)
    {
        AnnounceInventory01?.Invoke(context);
    }
    private void OnInventory02(InputAction.CallbackContext context)
    {
        AnnounceInventory02?.Invoke(context);
    }

    private void NextInventory(InputAction.CallbackContext context)
    {
        AnnounceNextInventory?.Invoke(context);
    }

    private void OnToggleRun(InputAction.CallbackContext context)
    {
        AnnounceToggleRun?.Invoke(context);
    }

    private void OnEnable()
    {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMove(); 
    }

    private void HandleLook()
    {
        AnnounceLookVector2?.Invoke(lookInput);
    }

    private void HandleMove()
    {
        Vector2 clampedMove = new Vector2(
            Mathf.Abs(moveInput.x) < 0.1f ? 0 : Mathf.Sign(moveInput.x),
            Mathf.Abs(moveInput.y) < 0.1f ? 0 : Mathf.Sign(moveInput.y)
        );
        moveInput = clampedMove;
        AnnounceMoveVector2?.Invoke(moveInput);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
            moveInput = context.ReadValue<Vector2>();
        else
            moveInput = Vector2.zero;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        if(context.performed)
            lookInput = context.ReadValue<Vector2>();
        else 
            lookInput = Vector2.zero;
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        AnnounceUseAction?.Invoke(context);
    }

    private void OnAggressiveAction(InputAction.CallbackContext context)
    {
        AnnounceAggressiveAction?.Invoke(context);
    }

    void OnDisable()
    {
        controls.Disable();
        
        controls.InGameActionMap.MoveAction.performed -= OnMove;
        controls.InGameActionMap.MoveAction.canceled -= OnMove;
        controls.InGameActionMap.LookAction.performed -= OnLook;
        controls.InGameActionMap.LookAction.canceled -= OnLook;
        controls.InGameActionMap.UseAction.performed -= OnUse;
        controls.InGameActionMap.UseAction.canceled -= OnUse;
        controls.InGameActionMap.AggressiveAction.performed -= OnAggressiveAction;
        controls.InGameActionMap.AggressiveAction.canceled -= OnAggressiveAction;
        controls.InGameActionMap.SelectInventory01.performed -= OnInventory01;
        controls.InGameActionMap.SelectInventory01.canceled -= OnInventory01;
        controls.InGameActionMap.SelectInventory02.performed -= OnInventory02;
        controls.InGameActionMap.SelectInventory02.canceled -= OnInventory02;
    }
}
