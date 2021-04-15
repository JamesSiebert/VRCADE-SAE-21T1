using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    public Material touchStartMat;
    public Material touchEndMat;
    public Material grabStartMat;
    public Material grabEndMat;
    public Material triggerStartMat;
    public Material triggerEndMat;
    
    public void TouchStart()
    {
        GetComponent<MeshRenderer>().material = touchStartMat;
    }
    
    public void TouchEnd()
    {
        GetComponent<MeshRenderer>().material = touchEndMat;
    }
    
    public void GrabStart()
    {
        GetComponent<MeshRenderer>().material = grabStartMat;
    }
    
    public void GrabEnd()
    {
        GetComponent<MeshRenderer>().material = grabEndMat;
    }
    
    public void TriggerStart()
    {
        GetComponent<MeshRenderer>().material = triggerStartMat;
    }
    
    public void TriggerEnd()
    {
        GetComponent<MeshRenderer>().material = triggerEndMat;
    }
}
