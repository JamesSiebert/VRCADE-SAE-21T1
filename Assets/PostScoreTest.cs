using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PostScoreTest : MonoBehaviour
{
    public BasketballScoreController basketballScoreController;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Start Coroutine - wait 5");

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        // POST SCORE TO SERVER
        basketballScoreController.SaveScores(1);
        
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine");
    }
}
