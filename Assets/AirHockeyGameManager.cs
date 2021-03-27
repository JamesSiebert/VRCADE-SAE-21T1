using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class AirHockeyGameManager : MonoBehaviour
{

    public int player1Score = 0;
    public int player2Score = 0;
    public GameObject currentPuck;
    
    public TextMeshPro scoreText;
    public GameObject puck;
    public GameObject puckDestroyEffect;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    private void Start()
    {
        ResetGame();
    }

    public void AddPoint(bool player1GoalBox)
    {
        Instantiate (puckDestroyEffect, currentPuck.transform.position, transform.rotation);
            
        // Destroy Puck
        Destroy(currentPuck);
        
        
        if (player1GoalBox)
        {
            // Add score to player
            player2Score++;
            
            // If score is 10, start win process
            if (player2Score == 10) 
                StartCoroutine(ProcessWin(2));
            else
                UpdateUi();
            
            // Spawn puck to loser side
            currentPuck = Instantiate (puck, player1SpawnPoint.position, transform.rotation);
            
        }
        else
        {
            // Add score to player
            player1Score++;
            
            // If score is 10, start win process
            if (player1Score == 10)
                StartCoroutine(ProcessWin(1));
            else
                UpdateUi();
            
            // Spawn puck to loser side
            currentPuck = Instantiate (puck, player1SpawnPoint.position, transform.rotation);
        }
    }
    
    public void UpdateUi()
    {
        scoreText.text = "Player 1: " + player1Score + " | Player 2: " + player2Score;
    }

    public void ResetGame()
    {
        // Reset score counters
        player1Score = 0;
        player2Score = 0;
        
        // Spawn puck to player 1
        currentPuck = Instantiate (puck, player1SpawnPoint.position, transform.rotation);
    }
    
    
    IEnumerator ProcessWin(int player)
    {
        // todo SAVE SCORE TO PLAYER - Maybe just post to API
        // POST - Score (player_id, room_id, score: 1)
        // need to figure out how to get player id - handle parent maybe..
        // or maybe just dont score this as this isn't really a top score game.
        
        // Win Text
        scoreText.text = "PLAYER " + player + " WINS!";

        // Wait 5 seconds
        yield return new WaitForSeconds(5);

        ResetGame();
    }
}
