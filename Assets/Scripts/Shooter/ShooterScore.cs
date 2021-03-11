using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScore : MonoBehaviour
{
    // score text
    public TextMesh textMesh = null;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    public void ShowScore(int score)
    {
        // Sets the text to the score (string)
        textMesh.text = score.ToString();
    }
}
