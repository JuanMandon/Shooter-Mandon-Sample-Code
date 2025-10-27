using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Centralized input handler for gameplay actions using Unity's Input System.
/// Implements the IGameplayActions interface to broadcast input events (e.g., attack, aim, movement) via delegates,
/// enabling decoupled communication between player input and gameplay logic.
/// </summary>
[CreateAssetMenu(menuName = "Custom/InputReader")]
public class InputReader : ScriptableObject, MyGameInput.IGameplayActions
{
    [Header("Debug")]
    public bool ShowInputLogWarnings = true;
    
    //Interaction
    public event Action AttackEvent;
    public event Action AttackCancelledEvent;

    public event Action AimEvent;
    public event Action AimCancelledEvent;
    
    public event Action InteractEvent;
    public event Action InteractCancelledEvent;

    //Movement
    public event Action UpEvent;          
    public event Action UpCancelledEvent; 

    public event Action CrouchEvent;
    public event Action CrouchCancelledEvent;

    public event Action JumpEvent;
    public event Action JumpCancelledEvent;
    
    public event Action SprintEvent;
    public event Action SprintCancelledEvent;
    
    //Combat
    public event Action PrimaryWeaponEvent;
    public event Action PrimaryWeaponCancelledEvent;
    
    public event Action SecondaryWeaponEvent;
    public event Action SecondaryWeaponCancelledEvent;

    public event Action ChangeFireModeEvent;
    public event Action ChangeFireModeCancelledEvent;

    public event Action ReloadEvent;
    public event Action ReloadCancelledEvent;
    
    //Internal
    public bool LockInputDetection { get; private set; } = false;
    private MyGameInput _gameInput;
    private Vector2 _lookInput;
    private bool _disableClickButtons;
    
    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        _gameInput = new MyGameInput();
        _gameInput.Gameplay.SetCallbacks(this);
        SetGameplay();
        WarningMessages();
    }

    private void OnDisable()
    {
        UnsetGameplay();
    }

    private void WarningMessages()
    {
        ShowInputLogWarnings = false;
        _disableClickButtons = false;

        Debug.Log("Is showInputLogWarnings active: " + ShowInputLogWarnings);

        if (_disableClickButtons)
        {
            Debug.LogWarning("disableClickButtons is ACTIVE");
        }
    }

    #region Input Detection

    private void SetGameplay()
    {
        _gameInput.Gameplay.Enable();
        Debug.Log("InputReader is ready!");
    }

    private void UnsetGameplay()
    {
        _gameInput.Gameplay.Disable();
    }

    public void OnUp(InputAction.CallbackContext context) //reference code for adding new events and callbacks
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            UpEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: UpEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            UpCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: UpCancelledEvent");
            }
        }
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            AttackEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: AttackEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            AttackCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: AttackCancelledEvent");
            }
        }
    }
    
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            AimEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: AimEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            AimCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: AimCancelledEvent");
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            InteractEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: InteractEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            InteractCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: InteractCancelledEvent");
            }
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            CrouchEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: CrouchEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            CrouchCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: CrouchCancelledEvent");
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            JumpEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: JumpEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            JumpCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: JumpCancelledEvent");
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            SprintEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: SprintEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            SprintCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: SprintCancelledEvent");
            }
        }
    }

    public void OnChangeFireMode(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            ChangeFireModeEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: ChangeFireModeEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            ChangeFireModeCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: ChangeFireModeCancelledEvent");
            }
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            ReloadEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: ReloadEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            ReloadCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: ReloadCancelledEvent");
            }
        }
    }

    public void OnPrimaryWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            PrimaryWeaponEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: PrimaryWeaponEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            PrimaryWeaponCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: PrimaryWeaponCancelledEvent");
            }
        }
    }

    public void OnSecondaryWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !LockInputDetection)
        {
            SecondaryWeaponEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: SecondaryWeaponEvent");
            }
        }

        if (context.phase == InputActionPhase.Canceled && !LockInputDetection)
        {
            SecondaryWeaponCancelledEvent?.Invoke();

            if (ShowInputLogWarnings)
            {
                Debug.LogWarning("INPUT: SecondaryWeaponCancelledEvent");
            }
        }
    }

    #endregion Input Detection

    #region Public API

    /// <summary>
    /// Locks input, e.g keyboard input wont be detected
    /// </summary>
    public void DisableInput()
    {
        Debug.Log("Disabled Input");
        LockInputDetection = true;
    }

    /// <summary>
    /// Unlocks input, e.g keyboard input WILL be detected
    /// </summary>
    public void EnableInput()
    {
        Debug.Log("Enabled Input");
        LockInputDetection = false;
    }

    // /// <summary>
    // /// Disables all input except movement
    // /// </summary>
    // public void DisablAllButMovementInput()
    // {
    //     _disableAllButMovement = true;
    //     Debug.Log("Disabled All except movement");
    // }
    //
    // /// <summary>
    // /// Restores all and keeps movement how it was
    // /// </summary>
    // public void EnableAllButMovementInput()
    // {
    //     _disableAllButMovement = false;
    //     Debug.Log("Enabled All, kept movement the same");
    // }

    //Return Values Functions

    /// <summary>
    /// Returns the float value that represents forward axis movement, from the _gameInput.
    /// </summary>
    public float GetMoveAxisForward()
    {
        return _gameInput.Gameplay.Move.ReadValue<Vector2>().y;
    }

    /// <summary>
    /// Returns the float value that represents right axis movement, from the _gameInput.
    /// </summary>
    public float GetMoveAxisRight()
    {
        return _gameInput.Gameplay.Move.ReadValue<Vector2>().x;
    }

    /// <summary>
    /// Returns the float value that represents X (Up) mouse axis, from the _gameInput.
    /// </summary>
    public float GetMouseAxisX()
    {
        return _gameInput.Gameplay.Look.ReadValue<Vector2>().x;

    }

    /// <summary>
    /// Returns the float value that represents Y (Up) mouse axis, from the _gameInput.
    /// </summary>
    public float GetMouseAxisY()
    {
        return _gameInput.Gameplay.Look.ReadValue<Vector2>().y;
    }

    #endregion Public API

    
}