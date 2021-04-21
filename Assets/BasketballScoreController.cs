using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BasketballScoreController : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
//public class BasketballScoreController : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks,  IPunObservable
{
    public Dictionary<string, int> scoresDictionary = new Dictionary<string, int>();
    
    public Text uiTopScoreText;
    public Text uiMyTopScoreText;

    public string topScoreName = "NA";
    public int topScore = 0;
    
    public int myTopScore = 0;
    
    public string lastScoreResponseData;

    public bool initialScoreRequestCalled;

    public string playerId = "NA";

    public PhotonView photonView;
    
    
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    
    public void Start()
    {
        StartCoroutine(GetInitialScore());
    }
    
    IEnumerator GetInitialScore()
    {
        yield return new WaitForSeconds(2);

        playerId = GetComponent<PhotonView>().Controller.NickName;
        
        this.StartCoroutine(this.TopScoreRequest(this.TopScoreResponseCallback));
    }

    
    
    public void SaveScores(int score)
    {
        Debug.Log("START SAVE SCORE");
        RequestOwnership();
        
        // I am the owner of this object, this will only execute on 1 pc not all clients
        if (this.photonView.IsMine)
        {
            Debug.Log("Save view is mine");
            
            //playerId = photonView.Owner.NickName;
            string roomId = "Room_Basketball";

            // Post score to server
            this.StartCoroutine(this.PostHighScoreRequest(roomId, score, this.TopScoreResponseCallback));
            
        }
    }
    
    
    
    // For getting the latest high score
    private IEnumerator TopScoreRequest(Action<string> callback = null)
    {
        initialScoreRequestCalled = true;
        
        string postURL = "http://vrcade.jamessiebert.com/api/get_scores";

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("data",
            "{\"player_id\": \"" + playerId + "\"}"));
        
        UnityWebRequest request = UnityWebRequest.Post(postURL, formData);
        
        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;
        
        if (callback != null)
            callback(data);
    }
    
    
    
    // For posting a high score
    private IEnumerator PostHighScoreRequest(string roomId, int score, Action<string> callback = null)
    {
        Debug.Log("START POST REQUEST");
        
        string postHighScoreURL = "http://vrcade.jamessiebert.com/api/post_score";
        
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("data", "{\"player_id\": \"" + playerId + "\" , \"room_id\": \"" + roomId + "\"  , \"score\": \"" + score + "\" }"));

        UnityWebRequest request = UnityWebRequest.Post(postHighScoreURL, formData);
        Debug.Log("Score Post Sent");

        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;

        if (callback != null)
            callback(data);
    }
    
    
    // Callback to act on our response data (both top score and post score) updates top score display
    private void TopScoreResponseCallback(string data)
    {
        // Log
        Debug.Log("Scores Response: " + data);
        lastScoreResponseData = data;

        // unpack json response
        ScoreResponse jsonResponseObject = ScoreResponse.CreateFromJSON(data);

        // Update variables with unpacked json data
        topScore = jsonResponseObject.basketballTop;
        topScoreName = jsonResponseObject.basketballTopName;
        myTopScore = jsonResponseObject.basketballPlayerBest;

        // Update high score UI - this is where you will call a UI update
        UpdateScoreUI();
    }
    
    
    public void UpdateScoreUI()
    {
        uiTopScoreText.text = "Top Score: \n" + topScoreName + ": " + topScore.ToString();
        uiMyTopScoreText.text = "Your Top Score: " + myTopScore.ToString();
    }
    
    
    
    public void RequestOwnership()
    {
        if(photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("We are already the owner");
        }
        else
        {
            Debug.Log("Call transfer ownership");
            photonView.RequestOwnership();
        }
    }
    
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        // Stops transferring ownership of all objects of same type e.g 2 Bows in scene, both change ownership
        if(targetView != photonView){
            return;
        }
    
        Debug.Log("OnOwnership requested for: " + targetView.name + " from: " + requestingPlayer.NickName);
        
        // Transfers ownership to new player
        photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Transfer is complete. New owner: " + targetView.Owner.NickName);
    }
    
}


