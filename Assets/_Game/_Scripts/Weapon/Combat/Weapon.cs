using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Manages weapon firing, fire modes, reloading, and magazine logic using event-driven and component-based patterns.
/// Implements observer events and enum-based state control for modular weapon behavior.
/// </summary>
public class Weapon : MonoBehaviour
{
    public enum WeaponFireMode
    {
        SemiAuto,
        FullAuto,
        ThreeRoundBurst
    }

    public enum WeaponPlatform
    {
        AK,
        AR
        //9mm Pistol
        //MP5
        //Etc.
    }

    public event Action OnFireBullet;
    public event Action OnChangeFireMode;
    public event Action OnReload;
    public string Name;
    public WeaponPlatform ThisWeaponPlatform;
    public Magazine LoadedMagazine { get; private set; }
    
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private WeaponEventChannel weaponEventChannel;
    
    [Header("Weapon Settings")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [Tooltip("Rounds per minute (RPM). Example: 600 for AK47 realistic RPM")] 
    [SerializeField]private float roundsPerMinute = 600f;
    [SerializeField] private WeaponFireMode weaponFireMode;
    [SerializeField] private WeaponFireMode[] allowedFireModes;
    [SerializeField] private Magazine.MagazineType allowedMagazineType;
    
    private float _nextFireTime;
    private bool _triggerHeld;
    private bool _isFireSystemReady;
    private int _fireModeIndex;

    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        _fireModeIndex = (int)weaponFireMode;
        if (allowedFireModes.Length < 1) Debug.LogError("Weapon has no allowed fire modes! Add some in inspector!");
        
        SubscribeToEvents(true);
    }

    private void OnDisable()
    {
        SubscribeToEvents(false);
    }

    private void SubscribeToEvents(bool subscribe)
    {
        if (subscribe)
        {
            inputReader.AttackEvent += HoldWeaponTrigger;
            inputReader.AttackCancelledEvent += ReleaseWeaponTrigger;
            inputReader.ChangeFireModeEvent += ChangeFireMode; 
        }
        else
        {
            inputReader.AttackEvent -= HoldWeaponTrigger;
            inputReader.AttackCancelledEvent -= ReleaseWeaponTrigger;
            inputReader.ChangeFireModeEvent -= ChangeFireMode; 
        }
    }

    private void Update()
    {
        CheckFireMode();
    }

    private void CheckFireMode()
    {
        if (_triggerHeld && _isFireSystemReady)
        {
            switch (weaponFireMode)
            {
                case WeaponFireMode.FullAuto:
                    FireWeaponFullAuto();
                    break;
                case WeaponFireMode.SemiAuto:
                    FireWeaponSemiAuto();
                    break;
                case WeaponFireMode.ThreeRoundBurst:
                    Debug.LogError("Three round burst not implemented");
                    break;
            }
        }
    }

    private void HoldWeaponTrigger()
    {
        _triggerHeld = true;
    }

    private void ReleaseWeaponTrigger()
    {
        _triggerHeld = false;
        _isFireSystemReady = true;
    }

    private void ChangeFireMode()
    {
        if (allowedFireModes == null || allowedFireModes.Length == 0)
            return;

        _fireModeIndex = (_fireModeIndex + 1) % allowedFireModes.Length;
        weaponFireMode = allowedFireModes[_fireModeIndex];
        
        OnChangeFireMode?.Invoke();
        weaponEventChannel.RaiseEventChangeFireMode(weaponFireMode);
        Debug.Log("Fire mode " + weaponFireMode.ToString());
        
    }
    
    private void FireWeaponFullAuto()
    {
        _isFireSystemReady = true;
        if (Time.time >= _nextFireTime)
        {
            FireWeapon();
            // convert rpm to seconds per round: 60 seconds / rounds per minute
            float secondsPerRound = 60f / Mathf.Max(roundsPerMinute, 0.0001f);
            _nextFireTime = Time.time + secondsPerRound;
        }
    }

    private void FireWeaponSemiAuto()
    {
        if (Time.time >= _nextFireTime && _isFireSystemReady)
        {
            FireWeapon();
            float secondsPerRound = 60f / Mathf.Max(roundsPerMinute, 0.0001f);
            _nextFireTime = Time.time + secondsPerRound;
            _isFireSystemReady = false;
        }
    }
    
    private void FireWeapon()
    {
        if (LoadedMagazine != null && LoadedMagazine.CurrentBulletCount > 0)
        {
            GameObject firedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Bullet bullet = firedBullet.GetComponent<Bullet>();
            bullet.BulletData = LoadedMagazine.LoadedBulletData;
            bullet.FireBullet();
            LoadedMagazine.ConsumeBullet();
            OnFireBullet?.Invoke();
        }
        else
        {
            Debug.Log("No ammo!");
        }
    }

    #region Public API
    public Magazine.MagazineType GetWeaponMagazineType()
    {
        return allowedMagazineType;
    }

    public void ChangeWeaponMagazine(Magazine newMagazine)
    {
        if (newMagazine != null && newMagazine.Type == allowedMagazineType)
        {
            LoadedMagazine = newMagazine;
            Debug.Log("Changed weapon mag for: " + newMagazine.Name);
            OnReload?.Invoke();
        }
    }

    #endregion
}
