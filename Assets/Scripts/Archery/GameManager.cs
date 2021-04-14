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
    public Text timeRemainingText;

    public string topScoreName = "NA";
    public int topScore = 0;

    public string lastScoreResponseData;
    

    private PhotonView photonView;
    
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
    }
    
    public void Start()
    {
        maxGameTime = GameSessionSoundtrack.length;
        PlayBGMusic();
        this.StartCoroutine(this.TopScoreRequest(this.TopScoreResponseCallback));
    } 

    void Update()
    {
        if (timerActive)
        {
            secondsRemaining -= Time.deltaTime;
            UpdateTimerUI();
            
            if (secondsRemaining < 0)
            {
                EndGameSession();
            }
        }

        if (Input.GetKeyDown("space"))
        {
            StartGameSession();
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
        // Game active
        if (timerActive)
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
        else
        {
            // FREE PLAY
            uiScoreText.text = "FREE PLAY MODE\nStart game to track scores";
        }

        uiTopScoreText.text = "Top Score: \n" + topScoreName + ": " + topScore.ToString();
    }

    
    private IEnumerator TopScoreRequest(Action<string> callback = null)
    {
        string postURL = "http://vrcade.jamessiebert.com/api/get_scores";

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("data",
            "{\"player_id\": \"\"}"));
        
        UnityWebRequest request = UnityWebRequest.Post(postURL, formData);
        
        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;
        
        if (callback != null)
            callback(data);
    }
    
    
    // Callback to act on our response data
    private void TopScoreResponseCallback(string data)
    {
        Debug.Log("Get High Scores Response: " + data);
        lastScoreResponseData = data;

        // unpack json response
        ScoreResponse jsonResponseObject = ScoreResponse.CreateFromJSON(data);

        // Update variables
        topScore = jsonResponseObject.archeryTop;
        topScoreName = jsonResponseObject.archeryTopName;

        UpdateScoreUI();
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
        if (secondsRemaining == 0)
        {
            SaveScores();
        }
        
        PlayBGMusic();
        LocalReset();
    }
    
    public void SaveScores()
    {
        // I am the owner of this object, this will only execute on 1 pc
        if (this.photonView.IsMine)
        {
            Debug.Log("SAVE SCORE > API");
            
            
            this.StartCoroutine(this.TopScoreRequest(this.TopScoreResponseCallback));
        }
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
