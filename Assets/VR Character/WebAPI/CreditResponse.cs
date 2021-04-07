using UnityEngine;

[System.Serializable]
public class CreditResponse
{
    // expected response variables
    public int balance;
    public string playerId;
    
    public static CreditResponse CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<CreditResponse>(jsonString);
    }
}
