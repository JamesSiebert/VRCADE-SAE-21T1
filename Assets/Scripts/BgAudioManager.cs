using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgAudioManager : MonoBehaviour
{
    public List<AudioClip> backgroundTracks;
    public List<AudioClip> gameTracks;

    public AudioSource audioSource;

    public int savedBgTrack = 0;
    public float savedBgTime = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ResumeBgMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Play the background music
    public void ResumeBgMusic()
    {
        // resume saved track and time
        audioSource.clip = backgroundTracks[savedBgTrack];
        audioSource.Play(0);
    }
    
    // Play the hype track for the game
    public void PlayGameMusic(int trackId)
    {
        // save current BG track and time
        
        // Switch and play new game track
        audioSource.clip = gameTracks[trackId];
        audioSource.Play(0);
    }

    // returns the length of a track (for game timer)
    public float GetPlayTrackLength(int trackId)
    { 
        return gameTracks[trackId].length;
    }
}
