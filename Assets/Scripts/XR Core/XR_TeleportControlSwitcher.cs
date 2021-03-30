using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XR_TeleportControlSwitcher : MonoBehaviour
{
    public GameObject leftDirectController;
    public GameObject leftRayController;
    public XRRayInteractor leftRayInteractor;
    
    public GameObject rightDirectController;
    public GameObject rightRayController;
    public XRRayInteractor rightRayInteractor;

    public bool teleportOnDeactivate;
    public bool useDirectionalTeleporting;
    public GameObject DirectionalMarkerPrefab;
    public JS_XRInputEventTrigger XRInputEventTriggerRef;    // get controller rotations
    [SerializeField] private TeleportationProvider provider;
    
    private GameObject leftDirectionalMarkerRef;
    private GameObject rightDirectionalMarkerRef;
    private bool leftTeleportActive;
    private bool rightTeleportActive;
    
    
    
    
    private int teleportationLayer;

    private void Start()
    {
        teleportationLayer = LayerMask.NameToLayer("Teleportation");

        if (useDirectionalTeleporting)
            SpawnDirectionalMarkers();

    }

    private void Update()
    {
        if (leftTeleportActive)
        {
            if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit) && hit.transform.gameObject.layer == teleportationLayer)
            {
                leftDirectionalMarkerRef.SetActive(true);
                
                // Update directional marker position
                leftDirectionalMarkerRef.transform.position = hit.point;
                Quaternion leftControllerRot = XRInputEventTriggerRef.leftControllerRotation;
                leftDirectionalMarkerRef.transform.localRotation = leftControllerRot;
                Debug.Log(leftControllerRot);
                
            }
            else
            {
                leftDirectionalMarkerRef.SetActive(false);
            }
        }

        if (rightTeleportActive)
        {
            if (rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit) && hit.transform.gameObject.layer == teleportationLayer)
            {
                rightDirectionalMarkerRef.SetActive(true);
                
                // Update directional marker position
                rightDirectionalMarkerRef.transform.position = hit.point;
                Debug.Log(XRInputEventTriggerRef.rightControllerRotation);
                
            }
            else
            {
                rightDirectionalMarkerRef.SetActive(false);
            }
        }
    }


    // Left Teleport Controller
    public void ActivateLeftTeleportController()
    {
        if (useDirectionalTeleporting)
            leftTeleportActive = true;
        
        leftDirectController.SetActive(false);
        leftRayController.SetActive(true);
    }
    public void DeactivateLeftTeleportController()
    {
        // Teleport
        if(teleportOnDeactivate)
            InitiateLeftTeleport();
        
        // turn off directional marker
        leftTeleportActive = false;
        leftDirectionalMarkerRef.SetActive(false);
        
        leftDirectController.SetActive(true);
        leftRayController.SetActive(false);
    }
    
    
    // Right Teleport Controller
    public void ActivateRightTeleportController()
    {
        rightDirectController.SetActive(false);
        rightRayController.SetActive(true);
        
    }
    public void DeactivateRightTeleportController()
    {
        // Teleport
        if(teleportOnDeactivate)
            InitiateRightTeleport();
        
        // turn off directional marker
        rightTeleportActive = false;
        rightDirectionalMarkerRef.SetActive(false);
        
        rightDirectController.SetActive(true);
        rightRayController.SetActive(false);
    }
    
    
    
    public void InitiateLeftTeleport()
    {
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit) == false)
        {
            Debug.Log("teleport hit false");
        }
        else
        {
            Debug.Log("teleport hit true");

            if (hit.transform.gameObject.layer == teleportationLayer)
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
    
    public void InitiateRightTeleport()
    {
        if (rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit) == false)
        {
            Debug.Log("teleport hit false");
        }
        else
        {
            Debug.Log("teleport hit true");

            if (hit.transform.gameObject.layer == teleportationLayer)
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
    
    
    
    
    public void SpawnDirectionalMarkers()
    {
        leftDirectionalMarkerRef = Instantiate(DirectionalMarkerPrefab, new Vector3(0,0,0), Quaternion.identity);
        leftDirectionalMarkerRef.SetActive(false);
        
        rightDirectionalMarkerRef = Instantiate(DirectionalMarkerPrefab, new Vector3(0,0,0), Quaternion.identity);
        rightDirectionalMarkerRef.SetActive(false);
    }

}
