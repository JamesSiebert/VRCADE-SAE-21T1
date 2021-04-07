using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ApiCommunication : MonoBehaviourPunCallbacks
{
    public bool enableApiCommunication = true;
    public PhotonView vrPlayerPhotonView;
    
    public string checkinURL = "http://vrcade.jamessiebert.com/api/checkin";
    public string modifyCreditURL = "http://vrcade.jamessiebert.com/api/modify_credit";
    public string getCreditBalanceURL = "http://vrcade.jamessiebert.com/api/check_credit";
    public string getHighScoreURL = "http://vrcade.jamessiebert.com/api/get_scores";
    public string postHighScoreURL = "http://vrcade.jamessiebert.com/api/post_score";
    
    public string playerId = "UNKNOWN";
    public string roomId = "UNKNOWN";
    public float checkinFrequency = 60;

    public string lastResponseData = "";
    public string lastScoreResponseData = "";
    public int creditBalance = 0;

    
    //{"airHockeyTop":0,
    //"basketballTop":0,
    //"archeryTop":100,
    //"airHockeyPlayerBest":0,
    //"basketballPlayerBest":0,
    //"archeryPlayerBest":0}
    public int airHockeyTop = 0;
    public int basketballTop = 0;
    public int archeryTop = 0;
    
    public int airHockeyPlayerBest = 0;
    public int basketballPlayerBest = 0;
    public int archeryPlayerBest = 0;

    public bool playEnabled = true;
    public int minPlayCredit = -500; // $5
    
    public UnityEvent OnPlayEnabled;
    public UnityEvent OnPlayDisabled;


    private void Start()
    {
        if (enableApiCommunication)
        {
            InvokeRepeating("RepeatCheckin", checkinFrequency, checkinFrequency);
        }
        
        playerId = vrPlayerPhotonView.Owner.NickName;
        roomId = SceneManager.GetActiveScene().name;
        
        GetCreditBalance();
        if (roomId == "Room_AirHockey" || roomId == "Room_Basketball" || roomId == "Room_Archery")
        {
            GetHighScores();
        }
        
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
        
        if (creditBalance < minPlayCredit)
        {
            // Credit below min
            if (playEnabled)
            {
                // State changed from Play Enabled to disabled - Call event
                playEnabled = false;
                OnPlayDisabled.Invoke();
            }
        }
        else
        {
            // Credit above min
            if (!playEnabled)
            {
                // State changed from Play Disabled to Enabled - Call event
                playEnabled = true;
                OnPlayEnabled.Invoke();
            }
        }
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
    
    
    // Call this to get current high scores from web server
    public void GetHighScores()
    {
        this.StartCoroutine(this.GetHighScoresRequest(this.GetHighScoresResponseCallback));
    }
    
    // Call this to post a new score to web server
    public void PostHighScore(int score)
    {
        this.StartCoroutine(this.PostHighScoreRequest(this.GetHighScoresResponseCallback));
    }
    
    // Request a score update
    private IEnumerator GetHighScoresRequest(Action<string> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("data", "{\"player_id\": \"" + playerId + "\"}"));

        UnityWebRequest request = UnityWebRequest.Post(getHighScoreURL, formData);

        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;

        if (callback != null)
            callback(data);
    }
    
    // For posting a score to the Server
    private IEnumerator PostHighScoreRequest(Action<string> callback = null)
    {
        int score = 5; //TEMP
        string roomId = "test"; //TEMP
        
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("data", "{\"player_id\": \"" + playerId + "\" , \"room_id\": \"" + roomId + "\"  , \"score\": \"" + score + "\" }"));

        UnityWebRequest request = UnityWebRequest.Post(postHighScoreURL, formData);

        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;

        if (callback != null)
            callback(data);
    }
    
    // Callback to act on our response data
    private void GetHighScoresResponseCallback(string data)
    {
        Debug.Log("Get High Scores Response: " + data);
        lastScoreResponseData = data;

        // unpack json response
        ScoreResponse jsonResponseObject = ScoreResponse.CreateFromJSON(data);

        // Update variables
        airHockeyTop = jsonResponseObject.airHockeyTop;
        basketballTop = jsonResponseObject.basketballTop;
        archeryTop = jsonResponseObject.archeryTop;
        airHockeyPlayerBest = jsonResponseObject.airHockeyPlayerBest;
        basketballPlayerBest = jsonResponseObject.basketballPlayerBest;
        archeryPlayerBest = jsonResponseObject.archeryPlayerBest;
    }
}