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

    }

    // Update is called once per frame
    void Update()
    {

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
        var audioClip = Resources.Load<AudioClip>(path);
        audioSource.PlayOneShot(audioClip);

    }
    public void StopSoundtrack()
    {
        audioSource.Stop();
    }

}
