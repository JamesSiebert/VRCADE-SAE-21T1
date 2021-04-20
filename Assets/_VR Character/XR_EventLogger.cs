using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XR_EventLogger : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;

    void Start()
    {
        // Teleport Select
        var select = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Select");
        select.Enable();
        select.performed += OnTeleportSelect;
        
        var activate = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportModeActivate;
        
        // Teleport Mode Cancel
        var cancel = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTeleportModeCancel;
        
    }

    private void OnTeleportSelect(InputAction.CallbackContext context)
    {
        Debug.Log("OnTeleportSelect");
    }
    
    private void OnTeleportModeActivate(InputAction.CallbackContext context)
    {
        Debug.Log("OnTeleportActivate");
    }
    
    private void OnTeleportModeCancel(InputAction.CallbackContext context)
    {
        Debug.Log("OnTeleportCancel");
    }
}
