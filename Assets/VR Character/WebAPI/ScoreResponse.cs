using UnityEngine;

[System.Serializable]
public class ScoreResponse
{
    // Expected response variables
    public int airHockeyTop;
    public int basketballTop;
    public int archeryTop;
    
    public string airHockeyTopName;
    public string basketballTopName;
    public string archeryTopName;

    public int airHockeyPlayerBest;
    public int basketballPlayerBest;
    public int archeryPlayerBest;
    
    public static ScoreResponse CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ScoreResponse>(jsonString);
    }
}

//{"airHockeyTop":0,
//"basketballTop":0,
//"archeryTop":100,
//"airHockeyPlayerBest":0,
//"basketballPlayerBest":0,
//"archeryPlayerBest":0}
