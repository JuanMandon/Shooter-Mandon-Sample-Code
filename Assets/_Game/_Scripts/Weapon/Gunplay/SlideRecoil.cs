using UnityEngine;

/// <summary>
/// Simulates the weapon slide’s physical recoil motion and spark effects when firing.
/// Uses event-driven callbacks and smooth positional interpolation for realistic mechanical feedback.
/// </summary>
public class SlideRecoil : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Weapon weapon;
    [SerializeField] private ParticleSystem slideSparks;
    
    [Header("Slide Recoil Settings")] 
    [SerializeField] private Transform slide;
    [SerializeField] private float slideTravelDistance = 0.05f; // max slide distance
    [SerializeField] private float slideSpeed = 2;           
    [SerializeField] private float backDelay = 0.02f;           // delay before returning

    private float _minTravelDistanceMultiplier = 0.8f;   // min distance for randomization
    private Vector3 _basePosition;
    private Vector3 _targetPosition;
    private float _backTimer;
    private bool _isReturning;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _basePosition = slide.localPosition;
        _targetPosition = _basePosition;
        SubscribeToEvents(true);
    }

    private void OnDestroy()
    {
        SubscribeToEvents(false);
    }

    private void SubscribeToEvents(bool subscribe)
    {
        if (subscribe)
            weapon.OnFireBullet += AddSlideRecoil;
        else
            weapon.OnFireBullet -= AddSlideRecoil;
    }

    private void Update()
    {
        if (!_isReturning)
        {
            // Snap backwards quickly
            slide.localPosition = Vector3.MoveTowards(
                slide.localPosition,
                _targetPosition,
                slideSpeed * Time.deltaTime
            );

            // Start timer for forward return
            if (Vector3.Distance(slide.localPosition, _targetPosition) < 0.0001f)
            {
                _backTimer += Time.deltaTime;
                if (_backTimer >= backDelay)
                {
                    _isReturning = true;
                    _targetPosition = _basePosition;
                    _backTimer = 0f;
                }
            }
        }
        else
        {
            // Return forward linearly
            slide.localPosition = Vector3.MoveTowards(
                slide.localPosition,
                _basePosition,
                slideSpeed * Time.deltaTime
            );

            // Done returning
            if (Vector3.Distance(slide.localPosition, _basePosition) < 0.0001f)
            {
                slide.localPosition = _basePosition;
                _isReturning = false;
            }
        }
    }

    private void AddSlideRecoil()
    {
        slideSparks?.Stop();
        slideSparks?.Play();
        
        // Randomize distance a bit for realism
        float minTravelDistance = slideTravelDistance * _minTravelDistanceMultiplier;
        float distance = Random.Range(minTravelDistance, slideTravelDistance);
        _targetPosition = _basePosition + Vector3.back * distance;
        _isReturning = false;
        _backTimer = 0f;
    }
}
