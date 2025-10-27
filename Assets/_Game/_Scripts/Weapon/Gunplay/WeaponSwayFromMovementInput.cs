using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Simulates dynamic weapon sway based on player movement and mouse input using smooth interpolation and damping.
/// Implements observer and component-based patterns to react to aim state and input events for realistic weapon motion.
/// </summary>
public class WeaponSwayFromMovementInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform trackingTarget; // anchor or camera follow

    [Header("Sway Settings")]
    [SerializeField] private float recoilMultiplier = 0.025f;
    [SerializeField] private float smoothTime = 0.15f;    // lower = snappier
    [SerializeField] private float returnSmoothTime = 0.2f; // smooth return when input stops
    [SerializeField] private float maxOffset = 0.05f;     // clamp offset for realism
    
    [Header("Aiming Sway Settings")]
    [SerializeField] private float smoothTimeAim = 0.5f;
    [SerializeField] private float aimingMultiplier = 0.33f;

    private Vector3 _targetOffset;
    private Vector3 _currentPosOffset;
    private Vector3 _posOffsetVelocity;
    private Vector3 _basePosition;
    
    private Vector3 _currentRotOffset;
    private Vector3 _rotOffsetVelocity;
    private bool _isAiming;
    
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _basePosition = transform.localPosition;
        SubscribeToEvents(true);
    }

    private void OnDestroy()
    {
        SubscribeToEvents(false);
    }
    private void SubscribeToEvents(bool subscribe)
    {
        if (subscribe)
        {
            inputReader.AimEvent += SetIsAiming;
            inputReader.AimCancelledEvent += UnsetIsAiming;
        }
        else
        {
            inputReader.AimEvent -= SetIsAiming;
            inputReader.AimCancelledEvent -= UnsetIsAiming;
        }
    }
    private void LateUpdate()
    {
        UpdateWeaponSway();
    }
    private void UpdateWeaponSway()
    {
        float moveForward = inputReader.GetMoveAxisForward(); // W/S
        float moveRight = inputReader.GetMoveAxisRight();     // A/D

        float maxGetMouseAxisValue = 100;
        float mouseX = (inputReader.GetMouseAxisX()) / (maxGetMouseAxisValue - (maxGetMouseAxisValue * -1)); //Horizontal
        float mouseY = (inputReader.GetMouseAxisY()) / (maxGetMouseAxisValue - (maxGetMouseAxisValue * -1));

        //Movement sway
        Vector3 moveOffset = Vector3.zero;
        moveOffset += Vector3.left * (moveRight * recoilMultiplier* (_isAiming ? aimingMultiplier/5 : 1));
        moveOffset += Vector3.back * (moveForward * recoilMultiplier * (_isAiming ? aimingMultiplier/5 : 1));

        //Mouse sway
        Vector3 mouseOffset = Vector3.zero;
        Vector3 mouseRotationOffset = Vector3.zero;
        mouseOffset += Vector3.left * (mouseX * recoilMultiplier);
        mouseOffset += Vector3.up * (mouseY * recoilMultiplier * 0.5f);
        mouseRotationOffset += Vector3.forward * inputReader.GetMouseAxisX();

        Vector3 totalOffset = moveOffset + mouseOffset;

        //Clamp max sway
        totalOffset = Vector3.ClampMagnitude(totalOffset, maxOffset * (_isAiming ? aimingMultiplier : 1));
        _targetOffset = totalOffset;

        float calculatedSmoothTime = (_isAiming ? smoothTimeAim : smoothTime);
        
        // Smooth position sway
        _currentPosOffset = Vector3.SmoothDamp(
            _currentPosOffset,
            _targetOffset,
            ref _posOffsetVelocity,
            _targetOffset.sqrMagnitude > 0.001f ? calculatedSmoothTime : returnSmoothTime
        );
        
        //Smooth rotation sway
        _currentRotOffset = Vector3.SmoothDamp(
            _currentRotOffset,
            mouseRotationOffset,
            ref _rotOffsetVelocity,
            mouseRotationOffset.sqrMagnitude > 0.001f ? calculatedSmoothTime : returnSmoothTime
        );
        
        transform.localPosition = _basePosition + _currentPosOffset;
        transform.localRotation = Quaternion.Euler(_currentRotOffset);
    }
    private void SetIsAiming()
    {
        _isAiming = true;
    }
    private void UnsetIsAiming()
    {
        _isAiming = false;
    }
}
    

