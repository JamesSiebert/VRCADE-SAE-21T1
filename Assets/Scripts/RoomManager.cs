using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForSchool;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForOutdoor;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForLobby;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForVertigo;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForAirHockey;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForBasketball;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForArchery;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForShooter;
    string mapType;

    void Start()
    {
        Debug.Log("RoomManager - start");
        
        // sync scene changes to all players.
        PhotonNetwork.AutomaticallySyncScene = true;

        if(!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("photon NOT connected and ready");
            // Handles reconnect from disconnect
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("photon Connected and ready");
            // Join lobby again
            PhotonNetwork.JoinLobby();
            // Callback - OnConnectToMaster
        }
    }

    #region UI Callback Methods

    public void JoinRandomRoom(){
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnEnterRoomButtonClicked_Outdoor()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_OUTDOOR;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterRoomButtonClicked_School()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_SCHOOL;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterRoomButtonClicked_Lobby()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_LOBBY;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterRoomButtonClicked_Vertigo()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VERTIGO;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterRoomButtonClicked_AirHockey()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_AIR_HOCKEY;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterRoomButtonClicked_Basketball()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_BASKETBALL;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterRoomButtonClicked_Archery()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_ARCHERY;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterRoomButtonClicked_Shooter()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_SHOOTER;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }


    #endregion

    #region Photon Callback Methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateAndJoinRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to servers again");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Local player: " + PhotonNetwork.NickName + " joined: " + PhotonNetwork.CurrentRoom.Name + ". Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
        {
            object mapType;
            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out mapType))
            {
                Debug.Log("Joined room with map: " + (string)mapType);
                if((string)mapType == MultiplayerVRConstants.MAP_TYPE_SCHOOL)
                {
                    // Load school scene
                    PhotonNetwork.LoadLevel("World_School");
                } 
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_OUTDOOR)
                {
                    // Load outdoor scene
                    PhotonNetwork.LoadLevel("World_Outdoor");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_LOBBY)
                {
                    // Load outdoor scene
                    PhotonNetwork.LoadLevel("Room_Lobby");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VERTIGO)
                {
                    // Load outdoor scene
                    PhotonNetwork.LoadLevel("Room_Vertigo");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_AIR_HOCKEY)
                {
                    // Load outdoor scene
                    PhotonNetwork.LoadLevel("Room_AirHockey");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_BASKETBALL)
                {
                    // Load outdoor scene
                    PhotonNetwork.LoadLevel("Room_Basketball");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_ARCHERY)
                {
                    // Load outdoor scene
                    PhotonNetwork.LoadLevel("Room_Archery");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_SHOOTER)
                {
                    // Load outdoor scene
                    PhotonNetwork.LoadLevel("Room_Shooter");
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player: " + newPlayer.NickName + " joined room. Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room List Update called");

        // Called if create, join, change property of room
        if(roomList.Count == 0 )
        {
            // No Room
            OccupancyRateText_ForSchool.text = 0 + " / " + 20;
            OccupancyRateText_ForOutdoor.text = 0 + " / " + 20;

            OccupancyRateText_ForLobby.text = 0 + " / " + 20;
            OccupancyRateText_ForVertigo.text = 0 + " / " + 20;
            OccupancyRateText_ForAirHockey.text = 0 + " / " + 20;
            OccupancyRateText_ForBasketball.text = 0 + " / " + 20;
            OccupancyRateText_ForArchery.text = 0 + " / " + 20;
            OccupancyRateText_ForShooter.text = 0 + " / " + 20;
        }

        foreach(RoomInfo room in roomList)
        {
            Debug.Log(room.Name);

            if(room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_OUTDOOR))
            {
                // Update the outdoor room map
                Debug.Log("Room is an OUTDOOR map. Player count is: " + room.PlayerCount);
                OccupancyRateText_ForOutdoor.text = room.PlayerCount + " / " + 20;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_SCHOOL))
            {
                // Update the school room map
                Debug.Log("Room is a SCHOOL map. Player count is: " + room.PlayerCount);
                OccupancyRateText_ForSchool.text = room.PlayerCount + " / " + 20;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_LOBBY))
            {
                Debug.Log("Room is a LOBBY map. Player count is: " + room.PlayerCount);
                OccupancyRateText_ForLobby.text = room.PlayerCount + " / " + 20;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VERTIGO))
            {
                Debug.Log("Room is a VERTIGO map. Player count is: " + room.PlayerCount);
                OccupancyRateText_ForVertigo.text = room.PlayerCount + " / " + 20;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_AIR_HOCKEY))
            {
                Debug.Log("Room is a AIR HOCKEY map. Player count is: " + room.PlayerCount);
                OccupancyRateText_ForAirHockey.text = room.PlayerCount + " / " + 20;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_BASKETBALL))
            {
                Debug.Log("Room is a BASKETBALL map. Player count is: " + room.PlayerCount);
                OccupancyRateText_ForBasketball.text = room.PlayerCount + " / " + 20;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_ARCHERY))
            {
                Debug.Log("Room is a ARCHERY map. Player count is: " + room.PlayerCount);
                OccupancyRateText_ForArchery.text = room.PlayerCount + " / " + 20;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_SHOOTER))
            {
                Debug.Log("Room is a SHOOTER map. Player count is: " + room.PlayerCount);
                OccupancyRateText_ForShooter.text = room.PlayerCount + " / " + 20;
            }
            else{
                Debug.Log(room.Name + " didnt match map names");
            }
        }

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    #endregion

    #region Private Methods
    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room_" + mapType + Random.Range(0,10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        string[] roomPropsInLobby = {MultiplayerVRConstants.MAP_TYPE_KEY};

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY, mapType}};

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
    #endregion

  



}
