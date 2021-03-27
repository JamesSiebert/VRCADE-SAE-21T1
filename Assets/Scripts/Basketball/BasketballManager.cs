using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BasketballManager : MonoBehaviour
{
    //Basketballs
    public GameObject Machine1BasketBall;
    public GameObject Machine2BasketBall;
    public GameObject Machine3BasketBall;
    public GameObject Machine4BasketBall;
    public int numberOfBalls = 0;
    
    //Time Stuff
    public GameObject timeLeftText1;
    public GameObject timeLeftText2;
    public GameObject timeLeftText3;
    public GameObject timeLeftText4;

    public static float timeLeft1 = 60;
    public static float timeLeft2 = 60;
    public static float timeLeft3 = 60;
    public static float timeLeft4 = 60;

    //Start Buttons
    public GameObject StartButton1;
    public GameObject StartButton2;
    public GameObject StartButton3;
    public GameObject StartButton4;

   //Booleans
    public bool Machine1 { get; }
    public bool Machine2 { get; }
    public bool Machine3 { get; }
    public bool Machine4 { get; }
    public bool timer1IsRunning = false;
    public bool timer2IsRunning = false;
    public bool timer3IsRunning = false;
    public bool timer4IsRunning = false;

    //Reset Buttons


    //Machine booleans


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLeftText1.GetComponent<Text>().text = timeLeft1.ToString("F0");
        timeLeftText2.GetComponent<Text>().text = timeLeft1.ToString("F0");
        timeLeftText3.GetComponent<Text>().text = timeLeft1.ToString("F0");
        timeLeftText4.GetComponent<Text>().text = timeLeft1.ToString("F0");

        if (timer1IsRunning)
        {
            if (timeLeft1 > 0)
            {
                timeLeft1 -= Time.deltaTime;
                Debug.Log(timeLeft1);
            }
            else
            {
                Debug.Log("Timer has run out!");
                timeLeft1 = 0;
                timer1IsRunning = false;
            }
        }

        if (timer2IsRunning)
        {
            if (timeLeft2 > 0)
            {
                timeLeft2 -= Time.deltaTime;
                Debug.Log(timeLeft2);
            }
            else
            {
                Debug.Log("Timer has run out!");
                timeLeft2 = 0;
                timer2IsRunning = false;
            }
        }

        if (timer3IsRunning)
        {
            if (timeLeft3 > 0)
            {
                timeLeft3 -= Time.deltaTime;
                Debug.Log(timeLeft3);
            }
            else
            {
                Debug.Log("Timer has run out!");
                timeLeft3 = 0;
                timer3IsRunning = false;
            }
        }

        if (timer4IsRunning)
        {
            if (timeLeft4 > 0)
            {
                timeLeft4 -= Time.deltaTime;
                Debug.Log(timeLeft4);
            }
            else
            {
                Debug.Log("Timer has run out!");
                timeLeft4 = 0;
                timer4IsRunning = false;
            }
        }


    }
    private Vector3 Position1()
    {
        float x = -8.3f;
        float y = 2.1f;
        float z = -5.8f;
        return new Vector3(x, y, z);
    }
    private Vector3 Position2()
    {
        float x = -8.3f;
        float y = 2.1f;
        float z = -1.848f;
        return new Vector3(x, y, z);
    }
    private Vector3 Position3()
    {
        float x = -8.3f;
        float y = 2.1f;
        float z = 2.053f;
        return new Vector3(x, y, z);
    }

    private Vector3 Position4()
    {
        float x = -8.3f;
        float y = 2.1f;
        float z = 6.075f;
        return new Vector3(x, y, z);
    }

    //public void OnStartButtonClicked()
    //{
    //    MachineCheck();
    //}

    //public void MachineCheck()
    //{
    //    if (GameObject.Find("ScoreTrigger1").GetComponent<BasketballManager>().Machine1 == true)
    //    {
    //        Machine1Spawn();

    //        //Instantiate(basketBall1, Position1(), Quaternion.identity);
    //        //Instantiate(basketBall2, Position2(), Quaternion.identity);
    //        //Instantiate(basketBall3, Position3(), Quaternion.identity);
    //    }
    //    else if (GameObject.Find("ScoreTrigger1").GetComponent<BasketballManager>().Machine2 == true)
    //    {
    //        Machine1Spawn();
    //    }
    //    else if (GameObject.Find("ScoreTrigger1").GetComponent<BasketballManager>().Machine3 == true)
    //    {
    //        Machine1Spawn();
    //    }
    //    else if (GameObject.Find("ScoreTrigger1").GetComponent<BasketballManager>().Machine4 == true)
    //    {
    //        Machine1Spawn();
    //    }
    //}

    public void Machine1Spawn()
    {
        Debug.Log("Start Button Clicked, spawning basketballs");
        StartButton1.SetActive(false);
        timer1IsRunning = true;

        for (int i = 0; i < numberOfBalls; i++)
        {
            Instantiate(Machine1BasketBall, Position1(), Quaternion.identity);
            Debug.Log("Spawned Basketball in machine 1");
        }
    }
    public void Machine2Spawn()
    {
        Debug.Log("Start Button Clicked, spawning basketballs");
        StartButton2.SetActive(false);
        timer2IsRunning = true;
        
        for (int i = 0; i < numberOfBalls; i++)
        {
            Instantiate(Machine2BasketBall, Position2(), Quaternion.identity);
            Debug.Log("Spawned Basketball in machine 2");
        }
    }
    public void Machine3Spawn()
    {
        Debug.Log("Start Button Clicked, spawning basketballs");
        StartButton3.SetActive(false);
        timer3IsRunning = true;

        for (int i = 0; i < numberOfBalls; i++)
        {
            Instantiate(Machine3BasketBall, Position3(), Quaternion.identity);
            Debug.Log("Spawned Basketball in machine 3");
        }
    }
    public void Machine4Spawn()
    {
        Debug.Log("Start Button Clicked, spawning basketballs");
        StartButton4.SetActive(false);
        timer4IsRunning = true;

        for (int i = 0; i < numberOfBalls; i++)
        {
            Instantiate(Machine4BasketBall, Position4(), Quaternion.identity);
            Debug.Log("Spawned Basketball in machine 4");
        }
    }

    public void Machine1Reset()
    {
        Debug.Log("Restart button clicked, despawn basketballs");
        StartButton1.SetActive(true);
        BasketballCollider.bballScore1 = 0;
        timer1IsRunning = false;
        timeLeft1 = 60;
        timeLeftText1.GetComponent<Text>().text = "00";

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Machine1Ball");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }

    }

    public void Machine2Reset()
    {
        Debug.Log("Restart button clicked, despawn basketballs");
        StartButton2.SetActive(true);
        BasketballCollider.bballScore2 = 0;
        timer2IsRunning = false;
        timeLeft2 = 60;
        timeLeftText2.GetComponent<Text>().text = "00";

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Machine2Ball");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
    }

    public void Machine3Reset()
    {
        Debug.Log("Restart button clicked, despawn basketballs");
        StartButton3.SetActive(true);
        BasketballCollider.bballScore3 = 0;
        timer3IsRunning = false;
        timeLeft3 = 60;
        timeLeftText3.GetComponent<Text>().text = "00";

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Machine3Ball");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
    }

    public void Machine4Reset()
    {
        Debug.Log("Restart button clicked, despawn basketballs");
        StartButton4.SetActive(true);
        BasketballCollider.bballScore4 = 0;
        timer4IsRunning = false;
        timeLeft4 = 60;
        timeLeftText4.GetComponent<Text>().text = "00";

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Machine4Ball");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
    }
}

