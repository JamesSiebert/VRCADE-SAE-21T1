using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;


// BASED ON PhotonTransformView Photon script
// https://doc.photonengine.com/en-us/pun/v2/gameplay/synchronization-and-state

/*
 *    NetworkPosition - received from network, received because this is a copy not the original player.
 * 
 */


public class ExampleHandSync : MonoBehaviour, IPunObservable
{
    public Transform leftHandTransform;

    private PhotonView m_PhotonView;
    
    public float m_Distance_LeftHand;
    
    public Vector3 m_Direction_LeftHand;
    public Vector3 m_NetworkPosition_LeftHand;
    public Vector3 m_StoredPosition_LeftHand;

    public Quaternion m_NetworkRotation_LeftHand;
    public float m_Angle_LeftHand;

    public bool m_firstTake = false;


    private void OnEnable()
    {
        m_firstTake = true;
    }

    private void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();

        m_StoredPosition_LeftHand = leftHandTransform.localPosition;
        m_NetworkPosition_LeftHand = Vector3.zero;
        m_NetworkRotation_LeftHand = Quaternion.identity;
    }

    void Update()
    {
        // Remote players only (not owner)
        if (!m_PhotonView.IsMine)
        {
            // UPDATE MODEL POSITIONS WITH RECEIVED DATA
            leftHandTransform.localPosition = Vector3.MoveTowards(
                leftHandTransform.localPosition,                                         // From original position
                m_NetworkPosition_LeftHand,                                               // To network position
                m_Distance_LeftHand * (1.0f / PhotonNetwork.SerializationRate)    // Distance, serialization rate = how many times / sec is called.
                );
        
            // UPDATE MODEL ROTATION WITH RECEIVED DATA
            leftHandTransform.localRotation = Quaternion.RotateTowards(
                leftHandTransform.localRotation,                                            // From original rotation
                m_NetworkRotation_LeftHand,                                                   // To network rotation
                m_Angle_LeftHand * (1.0f / PhotonNetwork.SerializationRate)        // Distance
                );
        }
    }


    // Required by IPunObservable interface
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.IsWriting)
        {
            Debug.Log("writing");
            // SEND DATA (owning player)
            m_Direction_LeftHand = leftHandTransform.localPosition - m_StoredPosition_LeftHand;
            m_StoredPosition_LeftHand = leftHandTransform.localPosition;
            
            stream.SendNext(leftHandTransform.localPosition);
            stream.SendNext(m_Direction_LeftHand);
            stream.SendNext(leftHandTransform.localRotation);
        }
        else
        {
            Debug.Log("reading");
            // RECEIVE DATA (replicated player on clients)
            
            // POSITION
            m_NetworkPosition_LeftHand = (Vector3)stream.ReceiveNext();
            m_Direction_LeftHand = (Vector3) stream.ReceiveNext();
            
            // LAG COMPENSATION
            if (m_firstTake)
            {
                
                leftHandTransform.localPosition = m_NetworkPosition_LeftHand;    // vector3.zero (awake method)
                m_Distance_LeftHand = 0;
            }
            else
            {
                // LAG - server time - time data was sent = delay time between send and receive
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)); // Positive float
                m_NetworkPosition_LeftHand += m_Direction_LeftHand * lag;
                m_Distance_LeftHand = Vector3.Distance(leftHandTransform.localPosition, m_NetworkPosition_LeftHand);
            }

            // ROTATION
            m_NetworkRotation_LeftHand = (Quaternion) stream.ReceiveNext();
            if (m_firstTake)
            {
                m_Angle_LeftHand = 0;
                leftHandTransform.localRotation = m_NetworkRotation_LeftHand; // from awake
            }
            else
            {
                m_Angle_LeftHand = Quaternion.Angle(leftHandTransform.localRotation, m_NetworkRotation_LeftHand);
            }

            
            if (m_firstTake)
            {
                m_firstTake = false;
            }

        }
    }
}
