using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ApiCommunication : MonoBehaviourPunCallbacks
{
    public bool enableApiCommunication = true;
    public PhotonView vrPlayerPhotonView;
    
    public string checkinURL = "http://vrcade.jamessiebert.com/api/checkin";
    public string modifyCreditURL = "http://vrcade.jamessiebert.com/api/modify_credit";
    public string getCreditBalanceURL = "http://vrcade.jamessiebert.com/api/check_credit";
    
    public string playerId = "UNKNOWN";
    public string roomId = "UNKNOWN";
    public float checkinFrequency = 60;

    public string lastResponseData = "";
    public int creditBalance = 0;
    

    private void Start()
    {
        if (enableApiCommunication)
        {
            InvokeRepeating("RepeatCheckin", checkinFrequency, checkinFrequency);
        }
        
        playerId = vrPlayerPhotonView.Owner.NickName;
        roomId = SceneManager.GetActiveScene().name;
        GetCreditBalance();
    }

    private void RepeatCheckin()
    {
        this.StartCoroutine(this.CheckinRequest(checkinURL, this.CheckinResponseCallback));
    }

    private IEnumerator CheckinRequest(string url, Action<string> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("data",
            "{\"player_id\": \"" + playerId + "\", \"room_id\":\"" + roomId + "\"}"));

        UnityWebRequest request = UnityWebRequest.Post(checkinURL, formData);

        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;

        if (callback != null)
            callback(data);
    }

    // Callback to act on our response data
    private void CheckinResponseCallback(string data)
    {
        Debug.Log("Checkin Response: " + data);
        lastResponseData = data;

        CreditResponse jsonResponseObject = CreditResponse.CreateFromJSON(data);
        creditBalance = jsonResponseObject.balance;
    }



    // Call this to add / deduct credit via API
    public void ModifyCredit(int modifyAmount)
    {
        this.StartCoroutine(this.ModifyCreditRequest(modifyAmount, this.ModifyCreditResponseCallback));
    }

    private IEnumerator ModifyCreditRequest(int modifyAmount, Action<string> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("data",
            "{\"player_id\": \"" + playerId + "\", \"amount\":\"" + modifyAmount + "\"}"));

        UnityWebRequest request = UnityWebRequest.Post(modifyCreditURL, formData);

        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;

        if (callback != null)
            callback(data);
    }

    // Callback to act on our response data
    private void ModifyCreditResponseCallback(string data)
    {
        Debug.Log("Credit Response: " + data);
        lastResponseData = data;

        CreditResponse jsonResponseObject = CreditResponse.CreateFromJSON(data);
        creditBalance = jsonResponseObject.balance;
    }


    // Call this to add / deduct credit via API
    public void GetCreditBalance()
    {
        this.StartCoroutine(this.GetCreditBalanceRequest(this.GetCreditBalanceResponseCallback));
    }

    private IEnumerator GetCreditBalanceRequest(Action<string> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("data", "{\"player_id\": \"" + playerId + "\"}"));

        UnityWebRequest request = UnityWebRequest.Post(getCreditBalanceURL, formData);

        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;

        if (callback != null)
            callback(data);
    }

    // Callback to act on our response data
    private void GetCreditBalanceResponseCallback(string data)
    {
        Debug.Log("Get Credit Balance Response: " + data);
        lastResponseData = data;

        CreditResponse jsonResponseObject = CreditResponse.CreateFromJSON(data);
        creditBalance = jsonResponseObject.balance;
    }
}