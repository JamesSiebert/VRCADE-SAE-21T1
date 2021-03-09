using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int player1Hits;
    public int player2Hits;

    // Game session soundtrack length
    public float maxGameTime;
    public bool timerActive;
    public float secondsRemaining = 0;

    public AudioClip BGSoundtrack;
    public AudioClip GameSessionSoundtrack;
    public AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maxGameTime = GameSessionSoundtrack.length;
        Debug.Log("Game length: " + maxGameTime);
        
        PlayBGMusic();
    } 

    void Update()
    {
        if (timerActive)
        {
            secondsRemaining -= Time.deltaTime;
            // update ui every second
            
            if (secondsRemaining < 0)
            {
                timerActive = false;
                EndGameSession();
            }
        }

        if (Input.GetKeyDown("space"))
        {
            StartGameSession();
        }
    }
    
    public void StartGameSession()
    {
        Reset();
        StartGameSessionMusic();
        UpdateUI();
    }
    
    public void EndGameSession()
    {
        // if game was played until the end
        if (secondsRemaining == 0)
        {
            SaveScores();
        }
        
        PlayBGMusic();
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
        timerActive = true;
    }


    public void UpdateUI()
    {
        
    }
    
    public void Reset()
    {
        player1Hits = 0;
        player2Hits = 0;
        secondsRemaining = maxGameTime;
        
    }

    public void SaveScores()
    {
        
    }

    public void RecordHit(int PlayerId)
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
    
}
