using KinematicCharacterController;
using System;
using UnityEngine;
using UnityEngine.Serialization;

//Third Party Asset, modified
public class PlayerController : MonoBehaviour
{
    public PlayerCharacterController Character;
    public PlayerCamera CharacterCamera;

    [SerializeField] private InputReader inputReader;

    private PlayerCharacterInputs _characterInputs = new PlayerCharacterInputs();
    private Action _setStartJump, _setStopJump;
    private float _scrollInput = 0;

    private void Awake()
    {
        SubscribeToEvents();
        Initialize();
    }

    private void Update()
    {
        HandleCharacterInput();
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void Initialize()
    {
        Cursor.lockState = CursorLockMode.Locked;

        CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

        // Ignore the character's colliders for camera obstruction checks
        CharacterCamera.IgnoredColliders.Clear();
        CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
    }

    private void SubscribeToEvents()
    {
        //Set actions for event subscription
        _setStartJump = () => SetJump(true);
        _setStopJump = () => SetJump(false);

        //Actual subscription
        inputReader.JumpEvent += _setStartJump;
        inputReader.JumpCancelledEvent += _setStopJump;
    }

    private void UnsubscribeToEvents()
    {
        inputReader.JumpEvent -= _setStartJump;
        inputReader.JumpCancelledEvent -= _setStopJump;
    }

    private void LateUpdate()
    {
        // Handle rotating the camera along with physics movers
        if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
        {
            CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>()
            .RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
            
            CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera
            .PlanarDirection, Character.Motor.CharacterUp).normalized;
        }

        HandleCameraInput();
    }

    private void HandleCameraInput()
    {
        float mouseLookAxisUp = inputReader.GetMouseAxisY();
        float mouseLookAxisRight = inputReader.GetMouseAxisX();
        Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        // Prevent moving the camera while the cursor isn't locked
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            lookInputVector = Vector3.zero;
        }

        // Input for zooming the camera (disabled in WebGL because it can cause problems)
        //scrollInput = -Input.GetAxis(MouseScrollInput);
        
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

        CharacterCamera.UpdateWithInput(Time.deltaTime, _scrollInput, lookInputVector);
    }

    private void SetJump(bool dodgeValue)
    {
        _characterInputs.JumpDown = dodgeValue;
    }

    private void HandleCharacterInput()
    {
        _characterInputs.MoveAxisForward = inputReader.GetMoveAxisForward();
        _characterInputs.MoveAxisRight = inputReader.GetMoveAxisRight();

        _characterInputs.CameraRotation = CharacterCamera.Transform.rotation;

        Character.SetInputs(ref _characterInputs);
    }
}