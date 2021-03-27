using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraLinker : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
