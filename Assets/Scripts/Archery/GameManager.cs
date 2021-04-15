using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks,  IPunObservable
{
    // Network Synced
    public bool timerActive;

    public Dictionary<string, int> scoresDictionary = new Dictionary<string, int>();
    

    // Game session soundtrack length
    public float maxGameTime;
    public float secondsRemaining = 0;

    public AudioClip BGSoundtrack;
    public AudioClip GameSessionSoundtrack;
    public AudioSource audioSource;

    public Text uiScoreText;
    public Text uiTopScoreText;
    public Text uiMyTopScoreText;
    public Text timeRemainingText;

    public string topScoreName = "NA";
    public int topScore = 0;

    public int myTopScore = 0;

    public string lastScoreResponseData;

    public bool initialScoreRequestCalled;
    

    public PhotonView photonView;
    
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
    }
    
    public void Start()
    {
        maxGameTime = GameSessionSoundtrack.length;
        PlayBGMusic();
        
    } 

    void Update()
    {
        if (timerActive)
        {
            secondsRemaining -= Time.deltaTime;
            UpdateTimerUI();
            
            if (secondsRemaining <= 0)
            {
                EndGameSession();
            }
        }

        if (!initialScoreRequestCalled && photonView != null)
        {
            RequestOwnership();
            if (this.photonView.IsMine)
            {
                // Get to score values
                string playerId = photonView.Owner.NickName;
                this.StartCoroutine(this.TopScoreRequest(playerId, this.TopScoreResponseCallback));
            }
        }
    }

    public void PlayBGMusic()
    {
        audioSource.clip = BGSoundtrack;
        audioSource.Play(0);
    }
    

    public void StartGameSessionMusic()
    {
        audioSource.clip = GameSessionSoundtrack;
        audioSource.Play(0);
    }


    public void UpdateScoreUI()
    {
        if (scoresDictionary.Count == 0)
        {
            uiScoreText.text = "Start game to track scores";
        }
        else
        {
            // Create empty string
            string scoreText = "";

            // append dictionary scores to string
            foreach (KeyValuePair<string, int> kvp in scoresDictionary)
            {
                scoreText = scoreText + kvp.Key + ": " + kvp.Value.ToString() + "\n";
            }

            // print scores to game
            uiScoreText.text = "Scores: \n" + scoreText;
        }

        uiTopScoreText.text = "Top Score: \n" + topScoreName + ": " + topScore.ToString();
        uiMyTopScoreText.text = "Your Top Score: " + myTopScore.ToString();
    }

    

    
    

    public void UpdateTimerUI()
    {
        float timeText = Mathf.Round(secondsRemaining);

        if (timeText == 0f)
            timeRemainingText.text = "Game Inactive";
        else
            timeRemainingText.text = "Time Remaining: " + timeText.ToString();
    }
    
    public void LocalReset()
    {
        secondsRemaining = maxGameTime;
        UpdateScoreUI();
        UpdateTimerUI();
    }
    
    
    // ------------------ Network ------------------
    
    
    public void NetworkReset()
    {
        // I am the owner of this object, this will only execute on 1 pc
        if (this.photonView.IsMine)
        {
            scoresDictionary.Clear();
        }
    }

    public void RecordHit(string playerName)
    {
        if (timerActive)
        {
            RequestOwnership();
            if (this.photonView.IsMine)
            {
                // does the player have a score in the dictionary yet?
                if (scoresDictionary.ContainsKey(playerName))
                {
                    // player name exists in dictionary
                    scoresDictionary[playerName] = scoresDictionary[playerName] + 1;
                }
                else
                {
                    // if no create one and use the supplied name
                    scoresDictionary.Add (playerName, 1);
                }
            }

            UpdateScoreUI();
        }
    }

    public void StartGameSession()
    {
        RequestOwnership();
        
        NetworkReset(); // Set player scores to 0
        LocalReset(); // reset timer and update ui
        
        timerActive = true;
        StartGameSessionMusic();
    }
    
    public void EndGameSession()
    {
        RequestOwnership();
        
        timerActive = false;
        
        // if game was played until the end
        if (secondsRemaining <= 0)
        {
            Debug.Log("calling save game");
            SaveScores();
        }
        
        PlayBGMusic();
        LocalReset();
    }
    
    public void SaveScores()
    {
        Debug.Log("START SAVE SCORE");
        RequestOwnership();
        
        // I am the owner of this object, this will only execute on 1 pc not all clients
        if (this.photonView.IsMine)
        {
            Debug.Log("Save view is mine");
            // If this players name exists in the scores dictionary post their score
            if (scoresDictionary.ContainsKey(photonView.Owner.NickName))
            {
                Debug.Log("key exists in scores dict");
                
                string playerId = photonView.Owner.NickName;
                string roomId = "Room_Archery";
                int score = scoresDictionary[playerId];

                // Post score to server
                this.StartCoroutine(this.PostHighScoreRequest(playerId, roomId, score, this.TopScoreResponseCallback));
            }
            else
            {
                Debug.Log("ERROR: No nickname found. Nickname: " + photonView.Owner.NickName + "\n DICTIONARY DUMP:");
                foreach (KeyValuePair<string, int> kvp in scoresDictionary)
                {
                    Debug.Log(kvp.Key + ": " + kvp.Value.ToString());
                }
            }
        }
    }
    
    // For getting the latest high score
    private IEnumerator TopScoreRequest(string playerId, Action<string> callback = null)
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
    private IEnumerator PostHighScoreRequest(string playerId, string roomId, int score, Action<string> callback = null)
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
        topScore = jsonResponseObject.archeryTop;
        topScoreName = jsonResponseObject.archeryTopName;
        myTopScore = jsonResponseObject.archeryPlayerBest;

        // Update high score UI
        UpdateScoreUI();
    }
    
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // we are the owner / host - Send Data
            stream.SendNext(scoresDictionary);
            stream.SendNext(timerActive);
        }
        else
        {
            // we are the client - Get Data
            this.scoresDictionary = (Dictionary<string, int>) stream.ReceiveNext();
            this.timerActive = (bool) stream.ReceiveNext();
        }
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
