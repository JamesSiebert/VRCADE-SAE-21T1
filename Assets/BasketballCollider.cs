using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public int bballScore;
    public GameObject bballScoreText;

    void Update()
    {
        /*ballScoreText.GetComponent<Text>().text = "Score: " + bballScore;*/
    }

    private void OnTriggerEnter(Collider other)
    {
        bballScore = bballScore + 1;
    }
}
