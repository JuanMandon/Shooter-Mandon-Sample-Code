using UnityEngine;

/// <summary>
/// Handles all weapon-related audio feedback such as gunshots, fire-mode switches, and shot tails using event-driven callbacks.
/// Utilizes the observer and component-based patterns with randomized pitch and volume for variation.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class WeaponSounds : MonoBehaviour
{
    [Header("Gunshot Sounds and settings")]
    [SerializeField] private AudioClip[] gunshotClips;          // multiple variations
    [SerializeField] private AudioClip tailClip;                // optional echo
    [SerializeField] private float pitchRandomRange = 0.05f;    // 5% pitch variation
    [SerializeField] private float volumeRandomRange = 0.05f;   // optional volume variation
    [SerializeField] private float tailTriggerDelay = 0.2f;     // time after last shot before tail plays

    [SerializeField] private AudioClip fireModeSwitchClip;  
    
    private AudioSource _audioSource;
    private Weapon _weapon;

    private float _lastShotTime;
    private bool _isFiring;

    private float _originalVolume;
    private float _originalPitch;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
        _audioSource = GetComponent<AudioSource>();

        _originalVolume = _audioSource.volume;
        _originalPitch = _audioSource.pitch;
        _weapon.OnFireBullet += PlayGunshot;
        _weapon.OnChangeFireMode += PlayFireModeSwitch;
    }

    private void OnDestroy()
    {
        _weapon.OnFireBullet -= PlayGunshot;
    }

    private void Update()
    {
        if (_isFiring && Time.time - _lastShotTime > tailTriggerDelay)
        {
            _isFiring = false;
            PlayTail();
        }
    }
    private void PlayGunshot()
    {
        if (gunshotClips == null || gunshotClips.Length == 0) return;

        AudioClip selectedClip = gunshotClips[Random.Range(0, gunshotClips.Length)];

        _audioSource.pitch = _originalPitch + Random.Range(-pitchRandomRange, pitchRandomRange);
        _audioSource.volume = _originalVolume + Random.Range(-volumeRandomRange, volumeRandomRange);

        _audioSource.PlayOneShot(selectedClip);

        _lastShotTime = Time.time;
        _isFiring = true;
    }
    private void PlayTail()
    {
        if (tailClip == null) return;
        _audioSource.pitch = _originalPitch;
        _audioSource.volume = _originalVolume;
        _audioSource.PlayOneShot(tailClip);
    }
    private void PlayFireModeSwitch()
    {
        if (fireModeSwitchClip == null) return;

        _audioSource.pitch = _originalPitch;
        _audioSource.volume = _originalVolume;

        _audioSource.PlayOneShot(fireModeSwitchClip);
    }
}
