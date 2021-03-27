using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckInteractions : MonoBehaviour
{
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Handle"))
        {
            audioSource.Play();
        }
    }
}
