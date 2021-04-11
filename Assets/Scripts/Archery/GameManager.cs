using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

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

    public Text player1ScoreText;
    public Text player2ScoreText;
    public Text timeRemainingText;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maxGameTime = GameSessionSoundtrack.length;
        Debug.Log("Game length: " + maxGameTime);
        
        PlayBGMusic();
        
        Reset();
    } 

    void Update()
    {
        if (timerActive)
        {
            secondsRemaining -= Time.deltaTime;
            UpdateTimerUI();
            
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
        UpdateScoreUI();
    }
    
    public void EndGameSession()
    {
        timerActive = false;
        // if game was played until the end
        if (secondsRemaining == 0)
        {
            SaveScores();
        }
        
        PlayBGMusic();
        Reset();
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
    
    public void Reset()
    {
        player1Hits = 0;
        player2Hits = 0;
        secondsRemaining = maxGameTime;
        UpdateScoreUI();
        UpdateTimerUI();
    }

    public void SaveScores()
    {
        // player api communications update score
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
        UpdateScoreUI();
    }
    
}
