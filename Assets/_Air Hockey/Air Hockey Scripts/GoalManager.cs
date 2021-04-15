using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public AirHockeyGameManager airHockeyGameManager;
    public bool isPlayer1;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Puck"))
        {
            airHockeyGameManager.AddPoint(isPlayer1);
        }
    }
}
