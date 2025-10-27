using UnityEngine;

/// <summary>
/// Handles sprinting by dynamically adjusting the player's movement speed 
/// in response to sprint input events.
/// </summary>
[RequireComponent(typeof(PlayerCharacterController))]
public class SprintSystem : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField, Min(1f)] private float runSpeedMultiplier = 1.4f;

    private PlayerCharacterController _playerController;
    private float _baseSpeed;
    private bool _isSprinting;

    private void Awake()
    {
        _playerController = GetComponent<PlayerCharacterController>();
        _baseSpeed = _playerController.MaxStableMoveSpeed;

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
            inputReader.SprintEvent += StartSprinting;
            inputReader.SprintCancelledEvent += StopSprinting;
        }
        else
        {
            inputReader.SprintEvent -= StartSprinting;
            inputReader.SprintCancelledEvent -= StopSprinting;
        }
    }

    private void StartSprinting()
    {
        if (_isSprinting) return;
        _isSprinting = true;
        _playerController.MaxStableMoveSpeed = _baseSpeed * runSpeedMultiplier;
    }

    private void StopSprinting()
    {
        if (!_isSprinting) return;
        _isSprinting = false;
        _playerController.MaxStableMoveSpeed = _baseSpeed;
    }
}