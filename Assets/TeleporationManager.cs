using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleporationManager : MonoBehaviour
{

    // Ref: https://www.youtube.com/watch?v=cxRnK8aIUSI
    
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider provider;
    public bool previousUpdateWasActive = false;
    public bool _isActive = false;
    
    void Start()
    {
        // rayInteractor.enabled = false;
        
        var activate = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate; // Called only when this action is performed
        
        // var cancel = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Cancel");
        // cancel.Enable();
        // cancel.performed += OnTeleportCancel; // Called only when this action is performed
        
    }
    
    void Update()
    {

    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        Debug.Log("OnTeleportActive");

        // destination ray
        // rayInteractor.enabled = true;
        _isActive = true;

        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            // valid teleport destination
            TeleportRequest request = new TeleportRequest()
            {
                destinationPosition = hit.point,
                //destinationRotation = ?, // rotation 
            };

            // Process teleport
            provider.QueueTeleportRequest(request);
        }
    }
}
