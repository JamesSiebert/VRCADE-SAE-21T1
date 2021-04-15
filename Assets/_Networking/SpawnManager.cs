using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    GameObject vrPlayerPrefab;

    public Vector3 SpawnPosition;

    void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            // Instantiates a player across the network for all clients
            PhotonNetwork.Instantiate(vrPlayerPrefab.name, SpawnPosition, Quaternion.identity);
        }
    }
}
