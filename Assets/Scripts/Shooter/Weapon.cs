using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * Attach this to the weapon
 */

public class Weapon : MonoBehaviour
{

    private XRGrabInteractable interactable = null;

    private void Awake()
    {
        // on awake gets attached XRGrabInteractable component
        interactable = GetComponent<XRGrabInteractable>();
        
        // Handy tip: GetComponentInParent<>() & GetComponentInChildren<>() also possible.
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
        print("Fire");
    }
}
