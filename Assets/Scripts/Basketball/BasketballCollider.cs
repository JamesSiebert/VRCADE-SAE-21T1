using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasketballCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public static int bballScore;
    public GameObject bballScoreText;

    void Update()
    {
        //bballScoreText.GetComponent<TMPro.TextMeshPro>().text = "Score: " + bballScore;
        bballScoreText.GetComponent<Text>().text = "Score: " + bballScore;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        bballScore = bballScore + 1;

    }
}
