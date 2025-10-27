using UnityEngine;

/// <summary>
/// Defines the carrying capacity of a specific chest rig (ammunition vest).
/// Determines how many magazines, loose rounds, and grenades the player can store.
/// </summary>
[CreateAssetMenu(fileName = "ChestRigData", menuName = "Custom/ChestRigData")]
public class ChestRigData : ScriptableObject
{
    [Header("General Info")]
    public string RigName;

    [Header("Storage Limits")]
    [Tooltip("Maximum number of pistol magazines.")]
    public int MaxSmallMagazines;

    [Tooltip("Maximum number of rifle magazines.")]
    public int MaxMediumMagazines;

    [Tooltip("Maximum number of sniper or anti-material weapon magazines.")]
    public int MaxLargeMagazines;

    [Tooltip("Maximum count of loose rounds (e.g., shotguns or single-load weapons).")]
    public int MaxSpareRounds;

    [Tooltip("Maximum number of grenades or throwable explosives.")]
    public int MaxGrenades;
}