using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMusicManager : MonoBehaviour { 


    public AudioSource Track1;
    public AudioSource Track2;
    public AudioSource Track3;
    public AudioSource Track4;
    public AudioSource Track5;
    public AudioSource Track6;
    public AudioSource Track7;
    public AudioSource Track8;

    public int trackSelector;

    public int trackHistory;

    private int maxTracks = 8;
    

    // Start is called before the first frame update
    void Start()
    {
        trackSelector = Random.Range(0, maxTracks);

        if(trackSelector == 0)
        {
            Track1.Play();
            trackHistory = 1;
        }else if (trackSelector == 1)
        {
            Track2.Play();
            trackHistory = 2;
        }else if (trackSelector == 2)
        {
            Track3.Play();
            trackHistory = 3;
        }else if (trackSelector == 3)
        {
            Track4.Play();
            trackHistory = 4;
        }
        else if (trackSelector == 4)
        {
            Track5.Play();
            trackHistory = 5;
        }
        else if (trackSelector == 5)
        {
            Track6.Play();
            trackHistory = 6;
        }
        else if (trackSelector == 6)
        {
            Track7.Play();
            trackHistory = 7;
        }
        else if (trackSelector == 3)
        {
            Track8.Play();
            trackHistory = 8;
        }




    }

    // Update is called once per frame
    void Update()
    {
        if(Track1.isPlaying == false && Track2.isPlaying == false && Track3.isPlaying == false && Track4.isPlaying == false && Track5.isPlaying == false
            && Track6.isPlaying == false && Track7.isPlaying == false && Track8.isPlaying == false)
        {
            trackSelector = Random.Range(0, maxTracks);

            if (trackSelector == 0 && trackHistory != 1)
            {
                Track1.Play();
                trackHistory = 1;
            }
            else if (trackSelector == 1 && trackHistory != 2)
            {
                Track2.Play();
                trackHistory = 2;
            }
            else if (trackSelector == 2 && trackHistory != 3)
            {
                Track3.Play();
                trackHistory = 3;
            }
            else if (trackSelector == 3 && trackHistory != 4)
            {
                Track4.Play();
                trackHistory = 4;
            }
            else if (trackSelector == 4 && trackHistory != 5)
            {
                Track5.Play();
                trackHistory = 5;
            }
            else if (trackSelector == 5 && trackHistory != 6)
            {
                Track6.Play();
                trackHistory = 6;
            }
            else if (trackSelector == 6 && trackHistory != 7)
            {
                Track7.Play();
                trackHistory = 7;
            }
            else if (trackSelector == 7 && trackHistory != 8)
            {
                Track8.Play();
                trackHistory = 8;
            }
        }
    }
}
