using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMusicManager : MonoBehaviour { 


    public AudioSource Track1;
    public AudioSource Track2;
    public AudioSource Track3;
    public AudioSource Track4;

    public int trackSelector;

    public int trackHistory;

    private int maxTracks = 4;
    

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
            Track1.Play();
            trackHistory = 2;
        }else if (trackSelector == 2)
        {
            Track1.Play();
            trackHistory = 3;
        }else if (trackSelector == 3)
        {
            Track1.Play();
            trackHistory = 4;
        }




    }

    // Update is called once per frame
    void Update()
    {
        if(Track1.isPlaying == false && Track2.isPlaying == false && Track3.isPlaying == false && Track4.isPlaying == false)
        {
            trackSelector = Random.Range(0, maxTracks);

            if (trackSelector == 0 && trackHistory != 1)
            {
                Track1.Play();
                trackHistory = 1;
            }
            else if (trackSelector == 1 && trackHistory != 2)
            {
                Track1.Play();
                trackHistory = 2;
            }
            else if (trackSelector == 2 && trackHistory != 3)
            {
                Track1.Play();
                trackHistory = 3;
            }
            else if (trackSelector == 3 && trackHistory != 4)
            {
                Track1.Play();
                trackHistory = 4;
            }
        }
    }
}
