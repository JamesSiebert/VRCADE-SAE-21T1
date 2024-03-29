﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    

    public GameObject UI_VRMenuGameobject;
    public GameObject UI_OpenWorldsGameobject;



    void Start()
    {
        UI_VRMenuGameobject.SetActive(false);
        UI_OpenWorldsGameobject.SetActive(true);
    }

    public void OnWorldsButtonClicked(){
        Debug.Log("Worlds button clicked");
        if(UI_OpenWorldsGameobject != null){
            UI_OpenWorldsGameobject.SetActive(true);
        }
    }

    
    public void OnGoHomeButtonClicked(){
        Debug.Log("Go Home button clicked");
    }

    
    public void OnChangeAvatarButtonClicked(){
        Debug.Log("Change Avatar button clicked");
        AvatarSelectionManager.Instance.ActivateAvatarSelectionPlatform();
    }

}
