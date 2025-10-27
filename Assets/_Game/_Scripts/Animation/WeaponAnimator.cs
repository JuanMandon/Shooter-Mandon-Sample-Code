using System;
using UnityEngine;

/// <summary>
/// Handles weapon animation triggers (reloads, fire mode changes) in response to weapon events.
/// Uses an event-driven observer pattern to synchronize animation states with weapon logic.
/// </summary>
[RequireComponent(typeof(Animator))]
public class WeaponAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Weapon weapon;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (weapon == null)
        {
            Debug.LogError($"{nameof(WeaponAnimator)} on {name} has no weapon reference assigned.", this);
            enabled = false;
            return;
        }

        SubscribeToEvents(true);
    }

    private void OnDestroy() => SubscribeToEvents(false);

    private void SubscribeToEvents(bool subscribe)
    {
        if (subscribe)
        {
            weapon.OnChangeFireMode += OnFireModeChanged;
            weapon.OnReload += OnReload;
        }
        else
        {
            weapon.OnChangeFireMode -= OnFireModeChanged;
            weapon.OnReload -= OnReload;
        }
    }

    private void OnFireModeChanged() =>
        animator.SetTrigger("TriggerChangeFireMode");

    private void OnReload()
    {
        string triggerName = weapon.ThisWeaponPlatform switch
        {
            Weapon.WeaponPlatform.AK => "TriggerReloadAK",
            Weapon.WeaponPlatform.AR => "TriggerReloadAR",
            _ => null
        };

        if (!string.IsNullOrEmpty(triggerName))
            animator.SetTrigger(triggerName);
        else
            Debug.LogWarning($"No reload animation mapped for {weapon.ThisWeaponPlatform}", this);
    }
}