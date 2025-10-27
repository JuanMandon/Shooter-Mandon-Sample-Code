using System;
using UnityEngine;

/// <summary>
/// Defines a weapon magazine, including its capacity, size, and bullet type. 
/// Tracks and manages the current ammo count, providing basic functionality for consumption and status checks.
/// </summary>
public class Magazine : MonoBehaviour
{
    public enum MagazineSize
    {
        Small,
        Medium,
        Large,
    }

    public enum MagazineType
    {
        AKM,   // AKM, AK47, and other 7.62 AK rifles
        AK74,  // AK74, AKS74U, and other 5.45 AK rifles
        M4,    // All M4/AR-15 5.56 platforms
        // Extend as needed...
    }

    [Header("Magazine Settings")]
    public string Name;
    public BulletData LoadedBulletData;
    public MagazineSize Size = MagazineSize.Medium;
    public MagazineType Type = MagazineType.AKM;

    [SerializeField] private int maxBulletCount = 30;
    public int CurrentBulletCount { get; private set; }

    private void Awake()
    {
        CurrentBulletCount = maxBulletCount;
    }

    #region Public API

    public void ConsumeBullet()
    {
        if (CurrentBulletCount > 0)
        {
            CurrentBulletCount--;
        }
        else
        {
            Debug.LogWarning($"{Name} is empty!");
        }
    }

    #endregion
}