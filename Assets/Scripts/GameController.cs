using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController controler = null; //singleton
    public PlayerMovement player;

    private bool gameState = true;
    float nextTimeIncrease;
    float nextScoreIncrease = 0f;
    float  playerScore = 0;

    [SerializeField]  float spawnRate = 1f;
    [SerializeField]  float nextSpawn;

    [SerializeField] GameObject[] obstacles;
    [SerializeField] Text PlayerScoreText;

    [SerializeField] Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (controler == null)
            controler = this;
        else if (controler != this)
            Destroy(gameObject);

        nextTimeIncrease = Time.unscaledTime;
        player.animator.SetBool("GameRunning", gameState);
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameState)
            return;

        if (Time.time > nextSpawn)
            SpawnObstacle();

        if (Time.unscaledTime > nextTimeIncrease && gameState)
        {
            nextTimeIncrease = Time.unscaledTime + 10f;
            Time.timeScale += 0.1f;
        }
        updateScore();
    }

    public void PlayerHit()
    {
        Time.timeScale = 0;
        gameState = false;
        player.animator.SetBool("GameRunning", gameState);
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
        PlayerScoreText.text = "Player Score:" + playerScore;
    }
}
