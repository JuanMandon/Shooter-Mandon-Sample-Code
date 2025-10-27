using UnityEngine;

/// <summary>
/// Smoothly aligns this transform’s position and rotation to a target,
/// with optional axis control and adaptive smoothing for realistic sway effects.
/// </summary>
public class FPSHandsFollower : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;

    [Header("Rotation Smoothing")]
    [SerializeField] private float timeToTarget = 0.3f;
    [SerializeField] private float fastLerpMultiplier = 0.33f;
    [SerializeField] private float valueDifferenceThreshold = 25f;

    [Header("Axis Control")]
    [SerializeField] private bool allowRotateX = true;
    [SerializeField] private bool allowRotateY = true;
    [SerializeField] private bool allowRotateZ = true;

    // Debug info
    [SerializeField, Tooltip("For debugging only")] private float localEulerX;
    [SerializeField, Tooltip("For debugging only")] private float targetEulerX;
    [SerializeField, Tooltip("For debugging only")] private float currentValueDifferenceAbs;

    private float _timeToTargetOriginal;
    private float _timeToTargetFast;
    private float _xVelocity, _yVelocity, _zVelocity;
    private float _appliedXAngle, _appliedYAngle, _appliedZAngle;
    private float _lerpToFastTimer, _lerpToNormalTimer;

    private void Awake()
    {
        _timeToTargetOriginal = timeToTarget;
        _timeToTargetFast = timeToTarget * fastLerpMultiplier;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        UpdateAdaptiveXRotation();
        ApplyRotations();
        transform.position = target.position; // keep positional sync instantaneous
    }

    private void ApplyRotations()
    {
        if (allowRotateY)
            _appliedYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref _yVelocity, timeToTarget);
        else
            _appliedYAngle = transform.eulerAngles.y;

        if (allowRotateZ)
            _appliedZAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, target.eulerAngles.z, ref _zVelocity, timeToTarget);
        else
            _appliedZAngle = transform.eulerAngles.z;

        transform.rotation = Quaternion.Euler(_appliedXAngle, _appliedYAngle, _appliedZAngle);
    }

    private void UpdateAdaptiveXRotation()
    {
        if (!allowRotateX)
        {
            _appliedXAngle = transform.eulerAngles.x;
            return;
        }

        localEulerX = transform.eulerAngles.x;
        targetEulerX = target.eulerAngles.x;
        currentValueDifferenceAbs = Mathf.Abs(Mathf.DeltaAngle(localEulerX, targetEulerX));

        bool needsFastResponse = currentValueDifferenceAbs > valueDifferenceThreshold;
        float lerpDuration = 0.1f;

        if (needsFastResponse)
        {
            timeToTarget = Mathf.Lerp(_timeToTargetOriginal, _timeToTargetFast, _lerpToFastTimer);
            _lerpToFastTimer += Time.deltaTime / lerpDuration;
            _lerpToNormalTimer = 0f;
        }
        else
        {
            timeToTarget = Mathf.Lerp(_timeToTargetFast, _timeToTargetOriginal, _lerpToNormalTimer);
            _lerpToNormalTimer += Time.deltaTime / lerpDuration;
            _lerpToFastTimer = 0f;
        }

        _appliedXAngle = Mathf.SmoothDampAngle(localEulerX, targetEulerX, ref _xVelocity, timeToTarget);
    }
}