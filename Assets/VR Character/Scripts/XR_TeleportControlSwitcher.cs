using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
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
    public bool leftTeleportActive;
    public bool rightTeleportActive;
    private float rotationAngleMultiplier = 3;
    private Quaternion leftTeleportRotation;
    private Quaternion rightTeleportRotation;

    // Local master or remote copy (networking)
    public bool thisIsMaster = false;
    
    public GameObject xrRig;
    public GameObject vrPlayer;
    public float moveSpeed = 3;
    public Transform XR_Camera;
    
    public List<XRController> controllers;
    
    public Transform mainAvatar;
    public Transform avatarHead = null;
    public Transform avatarBody;
    public Transform avatarLeftHand;
    public Transform avatarRightHand;

    public Vector3 headPositionOffset = new Vector3(0,-0.9f,0);
    public Vector3 handRotationOffset = new Vector3(0, 0, 0);
    

    private int teleportationLayer;

    private void Start()
    {
        //Debug.Log("EpochUnix: " + EpochUnixTime().ToString());
        
        teleportationLayer = LayerMask.NameToLayer("Teleportation");

        if (useDirectionalTeleporting)
            SpawnDirectionalMarkers();

        
    }

    private void Update()
    {

        // override position of the vr controllers because tracking is lost when in multiplayer
        leftDirectController.transform.position = XRInputEventTriggerRef.leftControllerPosition + xrRig.transform.position;
        leftRayController.transform.position = XRInputEventTriggerRef.leftControllerPosition + xrRig.transform.position;

        rightDirectController.transform.position = XRInputEventTriggerRef.rightControllerPosition + xrRig.transform.position;
        rightRayController.transform.position = XRInputEventTriggerRef.rightControllerPosition + xrRig.transform.position;
        
        
        

        if (leftTeleportActive)
        {
            if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit) && hit.transform.gameObject.layer == teleportationLayer)
            {
                leftDirectionalMarkerRef.SetActive(true);

                // Update directional marker position
                leftDirectionalMarkerRef.transform.position = hit.point;
                
                // Get Controller Rotation
                Quaternion leftControllerRot = XRInputEventTriggerRef.leftControllerRotation;
                
                
                // -- DIRECTIONAL MARKER --
                
                // Gets direction of hit point from controller
                Vector3 hitDirection = hit.point - leftRayController.transform.position;
                
                // Locked y to account for downwards angle
                Quaternion forwardHeading = Quaternion.LookRotation(new Vector3(hitDirection.x, 0, hitDirection.z), Vector3.up);
                
                // Convert to Vector3 and get y rotation only
                float forwardHeadingAngle = forwardHeading.eulerAngles.y;

                // Get controller rotation * multiplier, then offset looking heading 
                float newDirectionalYRot = (-leftControllerRot.eulerAngles.z * rotationAngleMultiplier) + forwardHeadingAngle;

                // Update variable for teleporter
                leftTeleportRotation = Quaternion.Euler(0, newDirectionalYRot, 0);
                
                // Set directional marker rotation Y Axis only
                leftDirectionalMarkerRef.transform.localRotation = leftTeleportRotation;
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
                //Debug.Log("right");
                
                rightDirectionalMarkerRef.SetActive(true);
                
                // Update directional marker position
                rightDirectionalMarkerRef.transform.position = hit.point;
                
                // Get Controller Rotation
                Quaternion rightControllerRot = XRInputEventTriggerRef.rightControllerRotation;
                
                
                // -- DIRECTIONAL MARKER --
                
                // Gets direction of hit point from controller
                Vector3 hitDirection = hit.point - rightRayController.transform.position;
                
                // Locked y to account for downwards angle
                Quaternion forwardHeading = Quaternion.LookRotation(new Vector3(hitDirection.x, 0, hitDirection.z), Vector3.up);
                
                // Convert to Vector3 and get y rotation only
                float forwardHeadingAngle = forwardHeading.eulerAngles.y;

                // Get controller rotation * multiplier, then offset looking heading 
                float newDirectionalYRot = (-rightControllerRot.eulerAngles.z * rotationAngleMultiplier) + forwardHeadingAngle;

                // Update variable for teleporter
                rightTeleportRotation = Quaternion.Euler(0, newDirectionalYRot, 0);
                
                // Set directional marker rotation Y Axis only
                rightDirectionalMarkerRef.transform.localRotation = rightTeleportRotation;
            }
            else
            {
                rightDirectionalMarkerRef.SetActive(false);
            }
        }

        if (thisIsMaster)
        {
            UpdateAvatarBodyPositions();
        }
        
            
        // Left Controller - Continuous move stick - fixes issue with continuous move provider not working in multiplayer.
        if (XRInputEventTriggerRef.leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxis,out Vector2 positionVector))
        {
            if (positionVector.magnitude > 0.15f)
            {
                Debug.Log(positionVector);
                Move(positionVector);
            }
        }
        
        // Right Controller - 
        // if (XRInputEventTriggerRef.rightDevice.TryGetFeatureValue(CommonUsages.primary2DAxis,out Vector2 positionVector))
        // {
        //     if (positionVector.magnitude > 0.15f)
        //     {
        //         Debug.Log(positionVector);
        //         Move(positionVector);
        //     }
        // }
    }
    
    
    void UpdateAvatarBodyPositions()
    {
        //Head and Body position
        mainAvatar.position = Vector3.Lerp(mainAvatar.position, XR_Camera.position + headPositionOffset, 0.5f);

        avatarHead.rotation = Quaternion.Lerp(avatarHead.rotation, XR_Camera.rotation, 0.5f);
        avatarBody.rotation = Quaternion.Lerp(avatarBody.rotation, Quaternion.Euler(new Vector3(0, avatarHead.rotation.eulerAngles.y, 0)), 0.05f);
        
        // Left Hand Position
        if (leftTeleportActive)
        {
            //  *** Ray Controller
            avatarLeftHand.position = Vector3.Lerp(avatarLeftHand.position,xrRig.transform.position + XRInputEventTriggerRef.leftControllerPosition,0.5f);
            avatarLeftHand.rotation = Quaternion.Lerp(avatarLeftHand.rotation,XRInputEventTriggerRef.leftControllerRotation,0.5f)*Quaternion.Euler(handRotationOffset);
        }
        else
        {
            // *** Direct Interactor 
            avatarLeftHand.position = Vector3.Lerp(avatarLeftHand.position,xrRig.transform.position + XRInputEventTriggerRef.leftControllerPosition,0.5f);
            avatarLeftHand.rotation = Quaternion.Lerp(avatarLeftHand.localRotation,XRInputEventTriggerRef.leftControllerRotation,0.5f)*Quaternion.Euler(handRotationOffset);
        }
        
        // Right Hand Position
        if (rightTeleportActive)
        {
            avatarRightHand.position = Vector3.Lerp(avatarRightHand.position,xrRig.transform.position + XRInputEventTriggerRef.rightControllerPosition,0.5f);
            avatarRightHand.rotation = Quaternion.Lerp(avatarRightHand.rotation,XRInputEventTriggerRef.rightControllerRotation,0.5f)*Quaternion.Euler(handRotationOffset);
        }
        else
        {
            avatarRightHand.position = Vector3.Lerp(avatarRightHand.position,xrRig.transform.position + XRInputEventTriggerRef.rightControllerPosition,0.5f);
            avatarRightHand.rotation = Quaternion.Lerp(avatarRightHand.rotation,XRInputEventTriggerRef.rightControllerRotation,0.5f) * Quaternion.Euler(handRotationOffset); // hand rotation offset?
        }
    }
    
    // movement through joystick in multiplayer when continuous move provider stops working
    private void Move(Vector2 positionVector)
    {
        // Apply the touch position to the head's forward Vector
        Vector3 direction = new Vector3(positionVector.x, 0, positionVector.y);
        Vector3 headRotation = new Vector3(0, avatarHead.eulerAngles.y, 0);
    
        // Rotate the input direction by the horizontal head rotation
        direction = Quaternion.Euler(headRotation) * direction;
    
        // Apply speed and move
        Vector3 movement = direction * moveSpeed;
        vrPlayer.transform.position += (Vector3.ProjectOnPlane(Time.deltaTime * movement, Vector3.up));
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
        if (useDirectionalTeleporting)
            rightTeleportActive = true;
        
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
    
    public float EpochUnixTime()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        float currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
 
        return currentEpochTime;
    }
    
    
    public void InitiateLeftTeleport()
    {
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit) == false)
        {
            Debug.Log("teleport hit false");
        }
        else
        {

            if (hit.transform.gameObject.layer == teleportationLayer)
            {
                // Debug.Log($"destinationPosition = {hit.point} destinationRotation = {leftTeleportRotation}, matchOrientation = {MatchOrientation.None}, requestTime = {EpochUnixTime()}");
                
                // valid teleport destination
                TeleportRequest request = new TeleportRequest()
                {
                    destinationPosition = hit.point,
                    destinationRotation = leftTeleportRotation, // Attempting to teleport with rotation - not working?
                    matchOrientation = MatchOrientation.None,
                    requestTime = EpochUnixTime()
                };
                // Process teleport
                provider.QueueTeleportRequest(request);
                
                // Not sure if this is the right way to do this but Snap-Turn uses a similar method.
                xrRig.transform.rotation = leftTeleportRotation;
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
            if (hit.transform.gameObject.layer == teleportationLayer)
            {
                // valid teleport destination
                TeleportRequest request = new TeleportRequest()
                {
                    destinationPosition = hit.point,
                    destinationRotation = rightTeleportRotation, // Attempting to teleport with rotation - not working?
                    matchOrientation = MatchOrientation.None,
                    requestTime = EpochUnixTime()
                };
                // Process teleport
                provider.QueueTeleportRequest(request);
                
                // Not sure if this is the right way to do this but Snap-Turn uses a similar method.
                xrRig.transform.rotation = rightTeleportRotation;
            }
        }
    }
    
    // From movement controller
    void OnEndLocomotion(LocomotionSystem locomotionSystem){
        Debug.Log("Teleporation ended - reset rig position");
        
        // reset player body to center of xr rig on teleport end
        vrPlayer.transform.position = vrPlayer.transform.TransformPoint(xrRig.transform.localPosition);
        xrRig.transform.localPosition = Vector3.zero;
    }
    
    
    public void SpawnDirectionalMarkers()
    {
        leftDirectionalMarkerRef = Instantiate(DirectionalMarkerPrefab, new Vector3(0,0,0), Quaternion.identity);
        leftDirectionalMarkerRef.SetActive(false);
        
        rightDirectionalMarkerRef = Instantiate(DirectionalMarkerPrefab, new Vector3(0,0,0), Quaternion.identity);
        rightDirectionalMarkerRef.SetActive(false);
    }
}