using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/*
 * use XR interaction - Controller raycaster (laser) to interact with UI
 */

public class UIDemo : MonoBehaviour
{
    public GameObject UICanvas;
    public Camera UICamera;
    
    public Text displayText;
    public TextMeshProUGUI displayTextTMPro;
    
    public Text sliderPercentageText;
    public GameObject UIPanel;

    private void Start()
    {

    }

    
    // Link spawned camera to UI - expensive
    private void Update()
    {
        // Link camera (Spawned Generic VR Player) to UI canvas
        if (UICanvas.GetComponent<Canvas>().worldCamera == null)
        {
            UICanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }

    // Button
    public void Button1Press()
    {
        displayText.text = "Button 1";
        displayTextTMPro.text = "Button 1";
    }
    
    // Button
    public void Button2Press()
    {
        displayText.text = "Button 2";
        displayTextTMPro.text = "Button 2";
    }
    
    

    // Slider - Dynamic Float - https://www.youtube.com/watch?v=b3S5a_ohZZ0
    public void sliderTextUpdate(float value)
    {
        sliderPercentageText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    // Toggle - Dynamic Bool - https://www.youtube.com/watch?v=0ewSSlTG2xo
    public void PanelToggleChanged(bool newValue)
    {
        UIPanel.SetActive(newValue);
    }

    // Dropdown - Dynamic Int - https://www.youtube.com/watch?v=5onggHOiZaw
    public void handleDropdown(int val)
    {
        if (val == 0)
        {
            displayText.text = "Dropdown 1 selected";
            displayTextTMPro.text = "Dropdown 1 selected";
        }
        else if (val == 1)
        {
            displayText.text = "Dropdown 2 selected";
            displayTextTMPro.text = "Dropdown 2 selected";
        }
        else if (val == 2)
        {
            displayText.text = "Dropdown 3 selected";
            displayTextTMPro.text = "Dropdown 3 selected";
        }
    }

    // Input Text - Dynamic String - https://www.youtube.com/watch?v=u1ht64_abzM
    public void InputTextChangedEnd(string newString)
    {
        displayText.text = newString;
        displayTextTMPro.text = newString;
    }
}
