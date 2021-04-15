using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * Attach this to the weapon
 */

public class ShooterWeapon : MonoBehaviour
{
    public float recoil = 1.0f;
    
    // Where bullet is spawned from
    public Transform barrel = null;
    
    public GameObject projectilePrefab = null;
    
    private XRGrabInteractable interactable = null;

    private Rigidbody rigidBody = null;

    private void Awake()
    {
        // on awake gets attached XRGrabInteractable component
        interactable = GetComponent<XRGrabInteractable>();
        
        // Handy tip: GetComponentInParent<>() & GetComponentInChildren<>() also possible.
        
        rigidBody = GetComponent<Rigidbody>();
    }
    
    // When this script is enabled its going to hook itself up to the interactable component event "onActivate"(trigger press)
    // This is the same as adding an OnActivate event in the inspector
    private void OnEnable()
    {
        interactable.onActivate.AddListener(Fire);
    }
    private void OnDisable()
    {
        interactable.onDeactivate.RemoveListener(Fire);        // on disable - un hook event
    }

    private void Fire(XRBaseInteractor interactor)
    {
        CreateProjectile();
        ApplyRecoil();
    }

    private void CreateProjectile()
    {
        // Instantiate projectile at barrel pos and return reference
        GameObject projectileObject = Instantiate(projectilePrefab, barrel.position, barrel.rotation);
        
        // Get the projectile component from the projectileObject we just instantiated.
        ShooterProjectile projectile = projectileObject.GetComponent<ShooterProjectile>();
        
        // Call the Launch function within the Projectile script on the projectile object.
        projectile.Launch();
    }

    private void ApplyRecoil()
    {
        rigidBody.AddRelativeForce(Vector3.back * recoil, ForceMode.Impulse);
    }
}
