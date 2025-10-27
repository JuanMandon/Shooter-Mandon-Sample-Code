using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles visual weapon effects such as muzzle flash and smoke using event-driven callbacks from the Weapon component.
/// Implements observer and coroutine patterns to manage rapid-fire visuals and timed particle cleanup.
/// </summary>
public class WeaponEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem muzzleSmokeTrail;
    
    private Weapon _weapon;
    private int _rapidFireBulletsShot;
    private float _rapidFireTimer;
    private readonly float _rapidFireResetTime = 0.5f;
    private bool _canCheckIfRapidFire;
    
    private Coroutine _clearSmokeTrailCoroutine;
    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
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
            _weapon.OnFireBullet += FireBullet;
            _weapon.OnFireBullet += AddToRapidFireCounter;
        }
        else
        {
            _weapon.OnFireBullet -= FireBullet;
            _weapon.OnFireBullet -= AddToRapidFireCounter;
        }
    }

    private void Update()
    {
        DoBarrelSmokeTrailIfRapidFire();
    }

    private void DoBarrelSmokeTrailIfRapidFire()
    {
        if (_canCheckIfRapidFire)
        {
            _rapidFireTimer += Time.deltaTime;
            if (_rapidFireTimer > _rapidFireResetTime)
            {
                if (_rapidFireBulletsShot > 5)
                {
                    muzzleSmokeTrail?.Play();
                }
                _canCheckIfRapidFire = false;
            }
        }
    }
    private void AddToRapidFireCounter()
    {
        if (muzzleSmokeTrail.isPlaying)
        {
            if (_clearSmokeTrailCoroutine != null)
            {
                StopCoroutine(_clearSmokeTrailCoroutine);
                _clearSmokeTrailCoroutine = null;
            }
            _clearSmokeTrailCoroutine = StartCoroutine(ClearSmokeTrailCoroutine());
        }
        
        _rapidFireTimer = 0f;
        _rapidFireBulletsShot++;
        _canCheckIfRapidFire = true;
    }
    private void FireBullet()
    {
        muzzleFlash.Play();
    }

    IEnumerator ClearSmokeTrailCoroutine()
    {
        float clearDelay = 6f;
        muzzleSmokeTrail.Stop();
        yield return new WaitForSeconds(clearDelay);
        muzzleSmokeTrail.Clear();
    }
}
