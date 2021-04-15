using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MultiplayerVRSynchronization : MonoBehaviour, IPunObservable
{
    private PhotonView m_PhotonView;
    
    public Transform vrPlayerTransform;
    public Transform mainAvatarTransform;
    public Transform headTransform;
    public Transform bodyTransform;
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    
    bool m_firstTake = false;
    
    //VR Player
    public float m_Distance_VRPlayer;
    public Vector3 m_Direction_VRPlayer;
    public Vector3 m_NetworkPosition_VRPlayer;
    public Vector3 m_StoredPosition_VRPlayer;
    public Quaternion m_NetworkRotation_VRPlayer;
    public float m_Angle_VRPlayer;
    
    //Main Avatar
    public float m_Distance_MainAvatar;
    public Vector3 m_Direction_MainAvatar;
    public Vector3 m_NetworkPosition_MainAvatar;
    public Vector3 m_StoredPosition_MainAvatar;
    public Quaternion m_NetworkRotation_MainAvatar;
    public float m_Angle_MainAvatar;
    
    // Avatar Head
    public Quaternion m_NetworkRotation_Head;
    public float m_Angle_Head;
    
    // Avatar Body
    public Quaternion m_NetworkRotation_Body;
    public float m_Angle_Body;
    
    // Left Hand
    public float m_Distance_LeftHand;
    public Vector3 m_Direction_LeftHand;
    public Vector3 m_NetworkPosition_LeftHand;
    public Vector3 m_StoredPosition_LeftHand;
    public Quaternion m_NetworkRotation_LeftHand;
    public float m_Angle_LeftHand;
    
    // Right Hand
    public float m_Distance_RightHand;
    public Vector3 m_Direction_RightHand;
    public Vector3 m_NetworkPosition_RightHand;
    public Vector3 m_StoredPosition_RightHand;
    public Quaternion m_NetworkRotation_RightHand;
    public float m_Angle_RightHand;

    public void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();

        //Main VRPlayer Sync Init
        m_StoredPosition_VRPlayer = vrPlayerTransform.position;
        m_NetworkPosition_VRPlayer = Vector3.zero;
        m_NetworkRotation_VRPlayer = Quaternion.identity;

        //Main Avatar Sync Init
        m_StoredPosition_MainAvatar = mainAvatarTransform.localPosition;
        m_NetworkPosition_MainAvatar = Vector3.zero;
        m_NetworkRotation_MainAvatar = Quaternion.identity;

        //Head Sync Init
        m_NetworkRotation_Head = Quaternion.identity;

        //Body Sync Init
        m_NetworkRotation_Body = Quaternion.identity;

        //Left Hand Sync Init
        m_StoredPosition_LeftHand = leftHandTransform.localPosition;
        m_NetworkPosition_LeftHand = Vector3.zero;
        m_NetworkRotation_LeftHand = Quaternion.identity;

        //Right Hand Sync Init
        m_StoredPosition_RightHand = rightHandTransform.localPosition;
        m_NetworkPosition_RightHand = Vector3.zero;
        m_NetworkRotation_RightHand = Quaternion.identity;
    }

    void OnEnable()
    {
        m_firstTake = true;
    }

    public void Update()
    {
        if (!this.m_PhotonView.IsMine)
        {
            // Move my vr player towards 
            vrPlayerTransform.position = Vector3.MoveTowards(vrPlayerTransform.position, this.m_NetworkPosition_VRPlayer, this.m_Distance_VRPlayer * (1.0f / PhotonNetwork.SerializationRate));
            vrPlayerTransform.rotation = Quaternion.RotateTowards(vrPlayerTransform.rotation, this.m_NetworkRotation_VRPlayer, this.m_Angle_VRPlayer * (1.0f / PhotonNetwork.SerializationRate));

            mainAvatarTransform.localPosition = Vector3.MoveTowards(mainAvatarTransform.localPosition, this.m_NetworkPosition_MainAvatar, this.m_Distance_MainAvatar * (1.0f / PhotonNetwork.SerializationRate));
            mainAvatarTransform.localRotation = Quaternion.RotateTowards(mainAvatarTransform.localRotation, this.m_NetworkRotation_MainAvatar, this.m_Angle_MainAvatar * (1.0f / PhotonNetwork.SerializationRate));
            headTransform.localRotation = Quaternion.RotateTowards(headTransform.localRotation, this.m_NetworkRotation_Head, this.m_Angle_Head * (1.0f / PhotonNetwork.SerializationRate));
            bodyTransform.localRotation = Quaternion.RotateTowards(bodyTransform.localRotation, this.m_NetworkRotation_Body, this.m_Angle_Body * (1.0f / PhotonNetwork.SerializationRate));

            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, this.m_NetworkPosition_LeftHand, this.m_Distance_LeftHand * (1.0f / PhotonNetwork.SerializationRate));
            leftHandTransform.localRotation = Quaternion.RotateTowards(leftHandTransform.localRotation, this.m_NetworkRotation_LeftHand, this.m_Angle_LeftHand * (1.0f / PhotonNetwork.SerializationRate));

            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, this.m_NetworkPosition_RightHand, this.m_Distance_RightHand * (1.0f / PhotonNetwork.SerializationRate));
            rightHandTransform.localRotation = Quaternion.RotateTowards(rightHandTransform.localRotation, this.m_NetworkRotation_RightHand, this.m_Angle_RightHand * (1.0f / PhotonNetwork.SerializationRate));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // SENDING DATA
            
            // VR Player
            this.m_Direction_VRPlayer = vrPlayerTransform.position - this.m_StoredPosition_VRPlayer;
            this.m_StoredPosition_VRPlayer = vrPlayerTransform.position;
            stream.SendNext(vrPlayerTransform.position);
            stream.SendNext(this.m_Direction_VRPlayer);
            stream.SendNext(vrPlayerTransform.rotation);

            // Main Avatar
            this.m_Direction_MainAvatar = mainAvatarTransform.localPosition - this.m_StoredPosition_MainAvatar;
            this.m_StoredPosition_MainAvatar = mainAvatarTransform.localPosition;
            stream.SendNext(mainAvatarTransform.localPosition);
            stream.SendNext(this.m_Direction_MainAvatar);
            stream.SendNext(mainAvatarTransform.localRotation);
            stream.SendNext(headTransform.localRotation);                                                        // Head Rotation
            stream.SendNext(bodyTransform.localRotation);                                                        // Body Rotation

            // Left Hand
            this.m_Direction_LeftHand = leftHandTransform.localPosition - this.m_StoredPosition_LeftHand;        // Position
            this.m_StoredPosition_LeftHand = leftHandTransform.localPosition;                                    // Position
            stream.SendNext(leftHandTransform.localPosition);                                                    // Position
            stream.SendNext(this.m_Direction_LeftHand);                                                          // Position
            stream.SendNext(leftHandTransform.localRotation);                                                    // Rotation

            //Right Hand
            this.m_Direction_RightHand = rightHandTransform.localPosition - this.m_StoredPosition_RightHand;    // Position
            this.m_StoredPosition_RightHand = rightHandTransform.localPosition;                                 // Position
            stream.SendNext(rightHandTransform.localPosition);                                                  // Position
            stream.SendNext(this.m_Direction_RightHand);                                                        // Position
            stream.SendNext(rightHandTransform.localRotation);                                                  // Rotation
        }
        else
        {
            // RECEIVE DATA
            
            // VR Player
            this.m_NetworkPosition_VRPlayer = (Vector3)stream.ReceiveNext();
            this.m_Direction_VRPlayer = (Vector3)stream.ReceiveNext();
            
            if (m_firstTake)
            {
                vrPlayerTransform.position = this.m_NetworkPosition_VRPlayer;                                                    // Position
                this.m_Distance_VRPlayer = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_VRPlayer += this.m_Direction_VRPlayer * lag;
                this.m_Distance_VRPlayer = Vector3.Distance(vrPlayerTransform.position, this.m_NetworkPosition_VRPlayer);
            }
            
            this.m_NetworkRotation_VRPlayer = (Quaternion)stream.ReceiveNext();                                                  // Rotation
            if (m_firstTake)
            {
                this.m_Angle_VRPlayer = 0f;
                vrPlayerTransform.rotation = this.m_NetworkRotation_VRPlayer;                                                    // Rotation
            }
            else
            {
                this.m_Angle_VRPlayer = Quaternion.Angle(vrPlayerTransform.rotation, this.m_NetworkRotation_VRPlayer);    // Rotation
            }
            
            // Main Avatar
            this.m_NetworkPosition_MainAvatar = (Vector3)stream.ReceiveNext();                                                  // Position
            this.m_Direction_MainAvatar = (Vector3)stream.ReceiveNext();                                                        // Position

            if (m_firstTake)
            {
                mainAvatarTransform.localPosition = this.m_NetworkPosition_MainAvatar;                                          // Position
                this.m_Distance_MainAvatar = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));                                    // Position
                this.m_NetworkPosition_MainAvatar += this.m_Direction_MainAvatar * lag;
                this.m_Distance_MainAvatar = Vector3.Distance(mainAvatarTransform.localPosition, this.m_NetworkPosition_MainAvatar);
            }
            
            this.m_NetworkRotation_MainAvatar = (Quaternion)stream.ReceiveNext();                                               // Rotation
            if (m_firstTake)
            {
                this.m_Angle_MainAvatar = 0f;
                mainAvatarTransform.rotation = this.m_NetworkRotation_MainAvatar;                                               // Rotation
            }
            else
            {
                this.m_Angle_MainAvatar = Quaternion.Angle(mainAvatarTransform.rotation, this.m_NetworkRotation_MainAvatar); // Rotation
            }


            // Avatar Head
            this.m_NetworkRotation_Head = (Quaternion)stream.ReceiveNext();

            if (m_firstTake)
            {
                this.m_Angle_Head = 0f;
                headTransform.localRotation = this.m_NetworkRotation_Head;                                               // Rotation
            }
            else
            {
                this.m_Angle_Head = Quaternion.Angle(headTransform.localRotation, this.m_NetworkRotation_Head);    // Rotation
            }

            // Avatar Body
            this.m_NetworkRotation_Body = (Quaternion)stream.ReceiveNext();                                              // Rotation

            if (m_firstTake)
            {
                this.m_Angle_Body = 0f;
                bodyTransform.localRotation = this.m_NetworkRotation_Body;                                               // Rotation
            }
            else
            {
                this.m_Angle_Body = Quaternion.Angle(bodyTransform.localRotation, this.m_NetworkRotation_Body);    // Rotation
            }

            // Left Hand
            this.m_NetworkPosition_LeftHand = (Vector3)stream.ReceiveNext();                                                            // Position
            this.m_Direction_LeftHand = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                leftHandTransform.localPosition = this.m_NetworkPosition_LeftHand;                                                     // Position
                this.m_Distance_LeftHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_LeftHand += this.m_Direction_LeftHand * lag;
                this.m_Distance_LeftHand = Vector3.Distance(leftHandTransform.localPosition, this.m_NetworkPosition_LeftHand);     // Position
            }
            
            this.m_NetworkRotation_LeftHand = (Quaternion)stream.ReceiveNext();                                                            // Rotation
            if (m_firstTake)
            {
                this.m_Angle_LeftHand = 0f;
                leftHandTransform.localRotation = this.m_NetworkRotation_LeftHand;                                                            // Rotation
            }
            else
            {
                this.m_Angle_LeftHand = Quaternion.Angle(leftHandTransform.localRotation, this.m_NetworkRotation_LeftHand);            // Rotation
            }

            // Right Hand
            this.m_NetworkPosition_RightHand = (Vector3)stream.ReceiveNext();                                                                // Position
            this.m_Direction_RightHand = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                rightHandTransform.localPosition = this.m_NetworkPosition_RightHand;                                                        // Position
                this.m_Distance_RightHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_RightHand += this.m_Direction_RightHand * lag;
                this.m_Distance_RightHand = Vector3.Distance(rightHandTransform.localPosition, this.m_NetworkPosition_RightHand);        // Position
            }
            
            this.m_NetworkRotation_RightHand = (Quaternion)stream.ReceiveNext();                                                            // Rotation
            if (m_firstTake)
            {
                this.m_Angle_RightHand = 0f;
                rightHandTransform.localRotation = this.m_NetworkRotation_RightHand;                                                            // Rotation
            }
            else
            {
                this.m_Angle_RightHand = Quaternion.Angle(rightHandTransform.localRotation, this.m_NetworkRotation_RightHand);          // Rotation
            }
            if (m_firstTake)
            {
                m_firstTake = false;
            }
        }
    }
}
