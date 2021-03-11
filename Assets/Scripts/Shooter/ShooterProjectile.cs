using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterProjectile : MonoBehaviour
{
    public float launchForce = 10.0f;
    private Rigidbody rigidBody = null;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Called from Weapon.cs Fire function
    public void Launch()
    {
        // Look into object pooling
        
        // Add force to projectile
        rigidBody.AddRelativeForce(Vector3.forward * launchForce, ForceMode.Impulse);
        
        // Destroy self in 5 secs
        Destroy(gameObject, 5.0f);
    }
}
