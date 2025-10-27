using UnityEngine;

/// <summary>
/// Defines bullet parameters (damage, penetration, and speed) as data assets using Unity’s ScriptableObject pattern,
/// allowing designers to tweak create different bullet types with varied ballistic values independently of the weapon logic.
/// </summary>
[CreateAssetMenu(fileName = "BulletData", menuName = "Custom/BulletData")]
public class BulletData : ScriptableObject
{
    public string BulletName;
    public float BulletDamage;
    public float BulletPenetration;
    [Tooltip("Bullet velocity in meters per second")]
    public float BulletSpeed;
}
