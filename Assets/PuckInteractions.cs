using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PuckInteractions : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    PhotonView puckPhotonView;
    public AudioSource audioSource;

    private void Awake()
    {
        puckPhotonView = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Handle"))
        {
            audioSource.Play();
            
            SetNewOwner(other.gameObject.GetComponent<NetworkedGrabbing>().m_photon_view); // Gets the Handles photon view for ownership
        }
    }
    
    private void SetNewOwner(PhotonView handlesPhotonView)
    {
        Debug.Log("Puck - SetNewOwner");
        
        if(puckPhotonView.Owner == handlesPhotonView.Owner)
        {
            Debug.Log("Puck and Handle already have the same owner");
        }
        else
        {
            Debug.Log("Call transfer ownership for puck");
            puckPhotonView.RequestOwnership(); //  Call ownership transfer request - Calls OnOwnershipRequest
        }
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        // Stops transferring ownership of multiple objects
        if(targetView != puckPhotonView){
            return;
        }
        
        Debug.Log("OnOwnership requested for: " + targetView.name + " from: " + requestingPlayer.NickName);
        
        puckPhotonView.TransferOwnership(requestingPlayer); // Ownership Transfer
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Transfer is complete. New owner: " + targetView.Owner.NickName);
    }
}
