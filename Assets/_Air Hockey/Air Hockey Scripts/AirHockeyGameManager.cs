using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AirHockeyGameManager : MonoBehaviour
{
    // private
    public int player1Score = 0;
    public int player2Score = 0;
    public GameObject puck;
    
    // Scoreboard
    public TextMeshProUGUI player1ScoreTMP;
    public TextMeshProUGUI player2ScoreTMP;
    public GameObject winEffect;
    public Transform winEffectSpawnPoint;
    public Transform goalEffectSpawnPointPlayer1;
    public Transform goalEffectSpawnPointPlayer2;
    
    
    // Other
    public GameObject puckDestroyEffect;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    public GameObject puckSpawnEffect;

    
    
    // todo destroy effect instantiations
    
    private void Start()
    {
        ResetGame();
    }

    public void AddPoint(bool player1GoalBox)
    {
        Debug.Log("Add point called from goal");
        
        // Spawn puck destroy effect
        if(player1GoalBox)
            Instantiate (puckDestroyEffect, goalEffectSpawnPointPlayer1.position, transform.rotation);
        else
            Instantiate (puckDestroyEffect, goalEffectSpawnPointPlayer2.position, transform.rotation);
            

        if (player1GoalBox)
        {
            // Add score to player
            player2Score++;
            
            // Move puck ready for player 1 to use (loser)
            MovePuck(2);
            
            // If score is 10, start win process
            if (player2Score == 10) 
                StartCoroutine(ProcessWin(2));
            else
            {
                UpdateUi();
            }
        }
        else
        {
            // Add score to player
            player1Score++;
            
            // Move puck ready for player 2 to use (loser)
            MovePuck(1);
            
            // If score is 10, start win process
            if (player1Score == 10)
                StartCoroutine(ProcessWin(1));
            else
            {
                UpdateUi();
            }
        }
    }
    
    public void UpdateUi()
    {
        player1ScoreTMP.text = "Player 1: " + player1Score + "\n Player 2: " + player2Score;
        player2ScoreTMP.text = "Player 1: " + player1Score + "\n Player 2: " + player2Score;
    }

    public void ResetGame()
    {
        // Reset score counters
        player1Score = 0;
        player2Score = 0;
        
        UpdateUi();
    }
    
    
    IEnumerator ProcessWin(int player)
    {
        // todo SAVE SCORE TO PLAYER - Maybe just post to API
        // POST - Score (player_id, room_id, score: 1)
        // need to figure out how to get player id - handle parent maybe..
        // or maybe just dont score this as this isn't really a top score game.
        
        // Win Text
        player1ScoreTMP.text = "PLAYER " + player + " WINS!";
        player2ScoreTMP.text = "PLAYER " + player + " WINS!";
        
        // Spawn fireworks
        Instantiate (winEffect, winEffectSpawnPoint.position, transform.rotation);

        // Wait 5 seconds
        yield return new WaitForSeconds(5);

        ResetGame();
    }

    private void MovePuck(int player)
    {
        Debug.Log("Move puck called");
        if (player == 1)
            puck.transform.position = player1SpawnPoint.position; // Issue here, puck not moving on goal.
        else
            puck.transform.position = player2SpawnPoint.position;
        
        // Spawn effect
        Instantiate (puckSpawnEffect, goalEffectSpawnPointPlayer2.position, transform.rotation);
    }
}
