/**
 * @file   SoundtrackController.cs
 * 
 * @authors  Eduardo S Pino
 * 
 * @version 1.0
 * @date 29/03/2020 (DD/MM/YYYY)
 *
 * Controls soundtrack and provides helper function to pause and resume the current track.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackController : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    private bool gameOverFlag = false;
    private bool toggleTrack = false;

    // Start is called before the first frame update
    void Start()
    {
        GameController.pauseEvent += PauseEventHndlr;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.controler.state == GameController.STATES.PAUSE)
        {
            return;
        }

        if (GameController.controler.state == GameController.STATES.RUNNING)
        {
            if (!audioSource.isPlaying && !toggleTrack)
            {
                StartSoundtrack("Music/Easy");
                toggleTrack = true;
            }

           
            if (!audioSource.isPlaying && toggleTrack)
            {
                StartSoundtrack("Music/Hard");
                toggleTrack = false;

            }
        }

        if (GameController.controler.state == GameController.STATES.GAMEOVER)
        {
            if (!gameOverFlag)
            {
                PlayGameOver();
                gameOverFlag = true;
            }
        }

    }

    public void PlayIntro()
    {
        StartSoundtrack("Music/intro");

    }

    public void PlayGameOver()
    {
        StopSoundtrack();
        StartSoundtrack("Music/GameOver");

    }

    public bool IsIntroPlaying()
    {
        return audioSource.isPlaying;
    }


    public void StartSoundtrack(string path)
    {
        StopSoundtrack();
        var audioClip = Resources.Load<AudioClip>(path);
        audioSource.clip = audioClip;
        audioSource.Play();

    }
    public void StopSoundtrack()
    {
       
        audioSource.Stop();
    }

    public void PauseSoundtrack()
    {
        audioSource.Pause();
    }

    void PauseEventHndlr(GameController.STATES state)
    {
        if (state != GameController.STATES.PAUSE)
            audioSource.Pause();
        else
            audioSource.Play();
    }

    public void ResumeSoundtrack()
    {
        audioSource.Play();
    }
}
