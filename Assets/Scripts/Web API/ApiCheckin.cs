using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class ApiCheckin : MonoBehaviourPunCallbacks
{
    private PhotonView m_PhotonView;
    public string checkinURL = "http://vrcade.jamessiebert.com/api/checkin";
    public string playerId = "UNKNOWN"; 
    public string roomId = "UNKNOWN";
    public float checkinFrequency = 60;

    public string lastResponseData = "";
 
    void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();
        InvokeRepeating("RepeatCheckin", checkinFrequency, checkinFrequency);
    }

    private void Start()
    {
        playerId = m_PhotonView.Owner.NickName;
        roomId = SceneManager.GetActiveScene().name;
    }

    private void RepeatCheckin()
    {
        this.StartCoroutine(this.CheckinRequest(checkinURL, this.ResponseCallback));
    }
    
    private IEnumerator CheckinRequest(string url, Action<string> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("data", "{\"player_id\": \"" + playerId + "\", \"room_id\":\"" + roomId + "\"}"));
        
        UnityWebRequest request = UnityWebRequest.Post("http://vrcade.jamessiebert.com/api/checkin", formData);
 
        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;
        
        if (callback != null)
            callback(data);
    }
 
    // Callback to act on our response data
    private void ResponseCallback(string data)
    {
        Debug.Log("Checkin Response: " + data);
        lastResponseData = data;
    }
}