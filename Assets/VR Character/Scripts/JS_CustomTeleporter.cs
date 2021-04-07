using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class JS_CustomTeleporter : MonoBehaviour
{
    
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider provider;
    
    
    
    public void RunTeleport()
    {
        // First frame _isActive was false - Button released
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit) == false)
        {
            Debug.Log("teleport hit false");
        }
        else
        {
            Debug.Log("teleport hit true");

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