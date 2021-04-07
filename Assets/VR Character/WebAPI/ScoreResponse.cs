using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreResponse : MonoBehaviour
{
    // Expected response variables
    public string airHockeyTop;
    public string basketballTop;
    public string archeryTop;

    public string airHockeyPlayerBest;
    public string basketballPlayerBest;
    public string archeryPlayerBest;
    
    public static ScoreResponse CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ScoreResponse>(jsonString);
    }
}
