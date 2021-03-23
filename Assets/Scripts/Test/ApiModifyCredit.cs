using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
 
public class ApiCheckin : MonoBehaviour
{
    public string modifyCreditURL = "http://vrcade.jamessiebert.com/api/modify_credit";
    public string playerId = "UNITY TEST"; 
    public int modifyAmount = 100;
    public string lastResponseData = "";
 
    void Awake()
    {
        InvokeRepeating("RepeatDeduct", checkinFrequency, checkinFrequency);
    }

    private void ModifyCredit(string playerId, int modifyAmount)
    {
        
        this.StartCoroutine(this.CheckinRequest(modifyCreditURL, this.ResponseCallback));
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