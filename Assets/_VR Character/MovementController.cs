using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class MovementController : MonoBehaviour
{
    
    // Ref: https://www.youtube.com/watch?v=cxRnK8aIUSI
    
    
    public float speed = 1.0f;
    



    [SerializeField]
    TeleportationProvider teleportationProvider;
    public GameObject MainVRPlayer;
    public GameObject XRRigGameobject;

    
    
    public GameObject xrRig;
    public GameObject vrPlayer;
    public float moveSpeed = 3;
    public Transform XR_Camera;
    
    public List<XRController> controllers;
    
    public Transform mainAvatar;
    public GameObject avatarHead = null;
    public Transform avatarBody;
    public Transform avatarLeftHand;
    public Transform avatarRightHand;

    public Vector3 headPositionOffset = new Vector3(0,-0.9f,0);
    public Vector3 handRotationOffset = new Vector3(0, 0, 0);
    
    
    
    
    
    
    
    private void OnEnable(){
        teleportationProvider.endLocomotion += OnEndLocomotion;
    }
    
    private void OnDisable(){
        teleportationProvider.endLocomotion -= OnEndLocomotion;
    }
    
    void OnEndLocomotion(LocomotionSystem locomotionSystem){
        Debug.Log("Teleporation ended - reset rig position");
        
        // reset player body to center of xr rig on teleport end
        MainVRPlayer.transform.position = MainVRPlayer.transform.TransformPoint(XRRigGameobject.transform.localPosition);
        XRRigGameobject.transform.localPosition = Vector3.zero;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
        foreach (XRController xRController in controllers)
        {
            Debug.Log(xRController.name);
        
            if (xRController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis,out Vector2 positionVector))
            {
                if (positionVector.magnitude > 0.15f)
                {
                    Debug.Log(positionVector);
                    Move(positionVector);
                }
            }
        }
    }


     private void Move(Vector2 positionVector)
     {
         // Apply the touch position to the head's forward Vector
         Vector3 direction = new Vector3(positionVector.x, 0, positionVector.y);
         Vector3 headRotation = new Vector3(0, avatarHead.transform.eulerAngles.y, 0);
    
         // Rotate the input direction by the horizontal head rotation
         direction = Quaternion.Euler(headRotation) * direction;
    
         // Apply speed and move
         Vector3 movement = direction * speed;
         transform.position += (Vector3.ProjectOnPlane(Time.deltaTime * movement, Vector3.up));
    }
}
