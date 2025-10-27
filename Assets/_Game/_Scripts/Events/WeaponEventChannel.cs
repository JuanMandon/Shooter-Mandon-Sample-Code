using System;
using UnityEngine;

/// <summary>
/// Handles and broadcasts weapon fire mode change events.
/// </summary>
[CreateAssetMenu(menuName = "Custom/Events/WeaponEventChannel")]
public class WeaponEventChannel : ScriptableObject
{
    public event Action<Weapon.WeaponFireMode> OnChangeFireModeEventRaised;

    /// <summary>
    /// Invokes the fire mode change event, notifying all subscribed listeners.
    /// </summary>
    /// <param name="fireMode">The new weapon fire mode.</param>
    public void RaiseEventChangeFireMode(Weapon.WeaponFireMode fireMode)
    {
        OnChangeFireModeEventRaised?.Invoke(fireMode);
    }
}