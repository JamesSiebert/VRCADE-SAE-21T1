using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks,  IPunObservable
{
    // Network Synced
    public int player1Hits;
    public int player2Hits;
    public bool timerActive;

    // Game session soundtrack length
    public float maxGameTime;
    public float secondsRemaining = 0;

    public AudioClip BGSoundtrack;
    public AudioClip GameSessionSoundtrack;
    public AudioSource audioSource;

    public Text player1ScoreText;
    public Text player2ScoreText;
    public Text timeRemainingText;

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
        player1ScoreText.text = $"Player 1 Score: {player1Hits}";
        player2ScoreText.text = $"Player 2 Score: {player2Hits}";
        
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
            player1Hits = 0;
            player2Hits = 0;
        }
    }

    public void RecordHit(int PlayerId)
    {
        RequestOwnership();

        if (this.photonView.IsMine)
        {
            if (PlayerId == 1)
            {
                player1Hits++;
            } 
            else if (PlayerId == 2)
            {
                player2Hits++;
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
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // we are the owner / host - Send Data
            stream.SendNext(player1Hits);
            stream.SendNext(player2Hits);
            stream.SendNext(timerActive);
        }
        else
        {
            // we are the client - Get Data
            this.player1Hits = (int) stream.ReceiveNext();
            this.player2Hits = (int) stream.ReceiveNext();
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
