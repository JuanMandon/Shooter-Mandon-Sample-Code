using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Smoothly transitions the weapon between idle and aiming states using DOTween. 
/// Reacts to player input to adjust local position and rotation with configurable timing and easing.
/// </summary>
public class PlayerAimWeapon : MonoBehaviour
{
    [Header("Aim settings")]
    [SerializeField] InputReader inputReader;
    [SerializeField] Vector3 aimPosition;
    [SerializeField] Vector3 aimRotation;
    [SerializeField] float aimTime = 0.25f;
    [SerializeField] float aimReleaseTime = 0.25f;
    [SerializeField] Ease aimStartEase = Ease.OutExpo;
    [SerializeField] Ease aimStopEase = Ease.OutBounce;
    
    private Vector3 _originalPosition;
    private Vector3 _originalRotation;
    private Sequence _aimTween;

    private void Awake()
    {
        Initialize();
        SubscribeToEvents(true);

    }

    private void Initialize()
    {
        _originalPosition = transform.localPosition;
        _originalRotation = transform.rotation.eulerAngles;
    }
    private void OnDestroy()
    {
        SubscribeToEvents(false);
    }

    private void SubscribeToEvents(bool subscribe)
    {
        if (subscribe)
        {
            inputReader.AimEvent += Aim;
            inputReader.AimCancelledEvent += ReleaseAim;
        }
        else
        {
            inputReader.AimEvent -= Aim;
            inputReader.AimCancelledEvent -= ReleaseAim;
        }
    }

    private void Aim()
    {
        if (_aimTween != null && _aimTween.IsActive())
        {
            _aimTween.Kill();
        }

        _aimTween = DOTween.Sequence();

        // Move and rotate at the same time
        _aimTween.Join(transform.DOLocalMove(aimPosition, aimTime));
        _aimTween.Join(transform.DOLocalRotate(aimRotation, aimTime));
        _aimTween.SetEase(aimStartEase);

        Debug.Log("AIMING!");
    }

    private void ReleaseAim()
    {
        if (_aimTween != null && _aimTween.IsActive())
        {
            _aimTween.Kill();
        }
        _aimTween = DOTween.Sequence();

        _aimTween.Join(transform.DOLocalMove(_originalPosition, aimReleaseTime));
        _aimTween.Join(transform.DOLocalRotate(_originalRotation, aimReleaseTime));
        _aimTween.SetEase(aimStopEase);
        
        Debug.Log("NOT AIMING!");
    }
}
