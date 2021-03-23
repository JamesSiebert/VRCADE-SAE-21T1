using UnityEngine;

[System.Serializable]
public class CreditResponse
{
    public int balance;
    public string playerId;
    
    public static CreditResponse CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<CreditResponse>(jsonString);
    }
    
    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}
