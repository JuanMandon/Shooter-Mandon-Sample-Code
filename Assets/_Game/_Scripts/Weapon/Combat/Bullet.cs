using System;
using UnityEngine;

/// <summary>
/// Handles the behavior of a fired bullet, including movement, collision detection, 
/// and automatic destruction after a short lifetime.
/// </summary>
public class Bullet : MonoBehaviour
{
    public BulletData BulletData;
    private void Awake()
    {
        Destroy(gameObject, 10f);
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit " + other.gameObject.name);
        Destroy(gameObject);
    }
    public void FireBullet()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * BulletData.BulletSpeed, ForceMode.VelocityChange);
    }
}
