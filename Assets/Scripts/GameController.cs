/**
 * @file   GameController.cs
 * 
 * @author  Eduardo S Pino
 * 
 * @version 1.0
 * @date 29/03/2020 (DD/MM/YYYY)
 *
 * This component implements the game finite state machine and helper functions to
 * keep score, save game etc..
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController controler = null; //singleton
    public PlayerMovement player;

    float nextTimeIncrease;
    float t = 0;
    float nextScoreIncrease = 0f;
    int playerScore = 0;
    int highestScore = 0;

    [SerializeField]  float spawnRate = 1f;
    [SerializeField]  float nextSpawn;

    [SerializeField] GameObject[] obstacles;
    [SerializeField] Text PlayerScoreText;
    [SerializeField] Text HigherScoreText;
    [SerializeField] Text GameOverText;
    [SerializeField] Text SkipIntroText;
    [SerializeField] Text RestartText;
    [SerializeField] SoundtrackController soundController;
    [SerializeField] Transform spawnPoint;


    public enum DIFFICULTY { EASY, HARD }; //In the future i might include difficulty, so i'll just leave it here
    public DIFFICULTY gameDifficulty;

    public enum STATES { INTRO, RUNNING, PAUSE, GAMEOVER, RESET}; //game states
    [SerializeField] public STATES state;
    private STATES previousState;

    // Start is called before the first frame update
    void Start()
    {
        RestartText.text = "";
        GameOverText.text = "";

        if (controler == null)
            controler = this;
        else if (controler != this)
            Destroy(gameObject);


        HighestScore savedScore = SaveGame.LoadScore();
        if(savedScore != null)
            highestScore = savedScore.highestScore;

        nextTimeIncrease = Time.unscaledTime;


        state = STATES.INTRO;
        soundController.PlayIntro();
    }

    // Update is called once per frame
    void Update()
    {
        FSM();
        CheckQuitGame();
        CheckPauseGame();
    }

    public void PlayerHit()
    {
        Time.timeScale = 0;
        state = STATES.GAMEOVER;
        player.animator.SetBool("GameRunning", false);
    }

    void SpawnObstacle()
    {
        nextSpawn = Time.time + spawnRate;
        int randomObstacle = Random.Range(0, obstacles.Length);
        Instantiate(obstacles[randomObstacle], spawnPoint.position, Quaternion.identity);

    }
    
    void updateScore()
    {
        if (Time.unscaledTime > nextScoreIncrease)
        {
            playerScore += 1;
            nextScoreIncrease = Time.unscaledTime + 1;
        }
        HigherScoreText.text = "Highest Score:" + highestScore;
        PlayerScoreText.text = "Player Score:" + playerScore;
    }

    void CheckIntro()
    {
        if (!soundController.IsIntroPlaying())
        {
            state = STATES.RUNNING;
        }
    }
    
    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single); //Load scene called Game
        Time.timeScale = 1;
    }

    void CheckPauseGame() //this should probably be redesign to use events instead
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (state != STATES.PAUSE)
            {
                previousState = state;
                player.animator.SetBool("GameRunning", false);
                state = STATES.PAUSE;
                soundController.PauseSoundtrack();
                player.FreezePlayer();
            }
            else
            {
                state = previousState;
                soundController.ResumeSoundtrack();
                player.UnFreezePlayer();
                player.OnCrouching(false);
            }
        }

    }

    void CheckQuitGame() //this should probably be redesign to use events instead
    {
        if (Input.GetButtonDown("QuitGame"))
        {
            Application.Quit();
        }
    }

    public int GetHighestScore()
    {
        return highestScore;
    }

    void FSM()
    {
        switch(state)
        {
            case STATES.INTRO:
                player.animator.SetBool("GameRunning", true);
                CheckIntro();

                SkipIntroText.text = "Press 's' to skip";
                if (Input.GetButtonDown("SkipIntro"))
                {
                    soundController.StopSoundtrack();
                    state = STATES.RUNNING;
                    SkipIntroText.text = "";
                }
                
                break;
            case STATES.RUNNING:
                SkipIntroText.text = "";
                player.animator.SetBool("GameRunning", true);
                if (Time.time > nextSpawn)
                    SpawnObstacle();

                if (Time.unscaledTime > nextTimeIncrease)
                {
                    nextTimeIncrease = Time.unscaledTime + 10f;
                    Time.timeScale += 0.1f;
                }
                updateScore();
                break;

            case STATES.PAUSE:
                player.animator.SetBool("GameRunning", false);
                break;

            case STATES.GAMEOVER:
                player.animator.SetBool("GameRunning", false);
                Light playerLight =  player.gameObject.GetComponentInChildren<Light>();
                Color lerpedColor = Color.Lerp(Color.white, Color.red, t);
                t += 0.005f;
                playerLight.color = lerpedColor;

                GameOverText.text = "GAME OVER";
                if (playerScore > highestScore)
                    highestScore = playerScore;

                HigherScoreText.text = "Highest Score:" + highestScore;
                SaveGame.SaveScore();

                RestartText.text = "Press 'r' to Restart";
                if (Input.GetButtonDown("Reset")) //this should probably be redesign to use events instead
                    state = STATES.RESET;
            
                break;
            case STATES.RESET: //
                ResetGame();
               
                break;

        }

        return;
    }
}
