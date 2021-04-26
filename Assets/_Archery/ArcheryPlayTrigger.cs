using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ArcheryPlayTrigger : MonoBehaviourPunCallbacks, IArrowHittable
{
    public GameManager gameManager;
    private AudioSource audioSource;
    private Renderer rend;
    public PhotonView photonView;

    public Material gameActiveMaterial;
    public Material gameInactiveMaterial;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        rend.material = gameInactiveMaterial;
    }
    
    
    // executes on hit
    public void Hit(Arrow arrow)
    {
        audioSource.Play();

        if (gameManager.timerActive)
        {
            // Initiate Stop game
            
            // Game is in play mode, end session
            gameManager.EndGameSession();
            // rend.material = gameOffMaterial;
            photonView.RPC("RPC_DeactivateGameCube", RpcTarget.AllBuffered);
            
            
            //arrow.MakeInvisible();
        }
        else
        {
            // initiate start game
            
            gameManager.StartGameSession();
            
            photonView.RPC("RPC_ActivateGameCube", RpcTarget.AllBuffered);
            
            //arrow.MakeInvisible();
        }
    }

    [PunRPC]
    public void RPC_ActivateGameCube()
    {
        rend.material = gameActiveMaterial;
    }
    
    [PunRPC]
    public void RPC_DeactivateGameCube()
    {
        rend.material = gameInactiveMaterial;
    }
    
    
}