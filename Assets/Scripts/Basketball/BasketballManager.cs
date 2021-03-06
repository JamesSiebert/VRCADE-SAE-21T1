using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballManager : MonoBehaviour
{
    //Basketballs
    public GameObject basketBall1;
    public GameObject basketBall2;
    public GameObject basketBall3;
    //Int
    public int waitTime = 3;
    
    public GameObject StartButton;
    public GameObject ballBarrier;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 Position1()
    {
        float x = -6.9f;
        float y = 2;
        float z = 1.6f;
        return new Vector3(x, y, z);
    }
    private Vector3 Position2()
    {
        float x = -6.9f;
        float y = 2;
        float z = 2;
        return new Vector3(x, y, z);
    }
    private Vector3 Position3()
    {
        float x = -6.9f;
        float y = 2;
        float z = 2.6f;
        return new Vector3(x, y, z);
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("Start Button Clicked, spawning basketballs");
        StartButton.SetActive(false);
        Instantiate(basketBall1, Position1(), Quaternion.identity);
        Instantiate(basketBall2, Position2(), Quaternion.identity);
        Instantiate(basketBall3, Position3(), Quaternion.identity);
        ballBarrier.SetActive(false);
        
    }

    public void OnResetButtonClicked()
    {
        Debug.Log("Restart button clicked, despawn basketballs");
        GameObject.Destroy(basketBall1);
        Destroy(basketBall2);
        Destroy(basketBall3);
        StartButton.SetActive(true);
        BasketballCollider.bballScore = 0;
        ballBarrier.SetActive(true);
    }

}
