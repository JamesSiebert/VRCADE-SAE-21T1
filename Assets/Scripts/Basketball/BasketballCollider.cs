using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasketballCollider : MonoBehaviour
{
    //Score
    public static int bballScore1;
    public static int bballScore2;
    public static int bballScore3;
    public static int bballScore4;

    //Score Text
    public Text bballScoreText1;
    public Text bballScoreText2;
    public Text bballScoreText3;
    public Text bballScoreText4;

    //Machine Check
    public bool Machine1 = false;
    public bool Machine2 = false;
    public bool Machine3 = false;
    public bool Machine4 = false;

    void Update()
    {
        //bballScoreText.GetComponent<TMPro.TextMeshPro>().text = "Score: " + bballScore;
        bballScoreText1.text = bballScore1.ToString();
        bballScoreText2.text = bballScore2.ToString();
        bballScoreText3.text = bballScore3.ToString();
        bballScoreText4.text = bballScore4.ToString();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (Machine1 == true)
        {
            Debug.Log("Machine1 score Point");
            bballScore1++;
        }
        else if (Machine2 == true)
        {
            Debug.Log("Machine2 score Point");
            bballScore2++;
        }
        else if (Machine3 == true)
        {
            Debug.Log("Machine3 score Point");
            bballScore3++;
        }
        else if (Machine4 == true)
        {
            Debug.Log("Machine4 score Point");
            bballScore4++;
        }

    }
}
