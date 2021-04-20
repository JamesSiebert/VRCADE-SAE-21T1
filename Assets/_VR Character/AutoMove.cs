using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public bool autoMoveActive;
    public GameObject puck;
    private Rigidbody puckRb;
    private Vector3 lockedPosition;
    public float reflectForce;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        puckRb = puck.GetComponent<Rigidbody>();
        
        // Used to lock Y & Z Axis
        lockedPosition = transform.position;
        
        // Call every X Seconds
        InvokeRepeating("CheckForSoloPlayer", 2.0f, 2.0f);
    }

    public void CheckForSoloPlayer()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            autoMoveActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (autoMoveActive)
        {
            transform.position = new Vector3(puck.transform.position.x, lockedPosition.y, lockedPosition.z);
        }
    }


    void OnCollisionEnter (Collision other) {

        if (other.gameObject.CompareTag("Puck"))
            puckRb.AddForce(Vector3.back * reflectForce, ForceMode.Impulse);
    }
}
