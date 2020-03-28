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
    [SerializeField] SoundtrackController soundController;

    [SerializeField] Transform spawnPoint;


    public enum MODE { EASY, HARD }; //to be used in the future!
    public MODE gameMode;

    public enum STATES { INTRO, RUNNING, PAUSE, GAMEOVER, RESET};
    [SerializeField] public STATES state;

    // Start is called before the first frame update
    void Start()
    {

        if (controler == null)
            controler = this;
        else if (controler != this)
            Destroy(gameObject);


        HighestScore savedScore = SaveGame.LoadScore();
        if(savedScore != null)
            highestScore = savedScore.highestScore;

        nextTimeIncrease = Time.unscaledTime;


        state = STATES.INTRO;
        gameMode = MODE.EASY;
        soundController.PlayIntro();
    }

    // Update is called once per frame
    void Update()
    {
        FSM();
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

    public int GetHighestScore()
    {
        return highestScore;
    }
    void FSM()
    {
        switch(state)
        {
            case STATES.INTRO: //New Game / Intro
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
            case STATES.RUNNING: //Game

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
            case STATES.PAUSE: //
                player.animator.SetBool("GameRunning", false);
                break;
            case STATES.GAMEOVER:
                player.animator.SetBool("GameRunning", false);
                Light playerLight =  player.gameObject.GetComponentInChildren<Light>();
                Color lerpedColor = Color.Lerp(Color.white, Color.red, t);
                t += 0.005f;
                playerLight.color = lerpedColor;
                //playerLight.color = Color.red;

                GameOverText.text = "GAME OVER";
                if (playerScore > highestScore)
                    highestScore = playerScore;

                HigherScoreText.text = "Highest Score:" + highestScore;

                SaveGame.SaveScore();

                if (Input.GetButtonDown("Reset"))
                {
                    state = STATES.RESET;
                }

                break;
            case STATES.RESET: //
                player.animator.SetBool("GameRunning", false);
                ResetGame();
               
                break;

        }

        return;
    }
}
