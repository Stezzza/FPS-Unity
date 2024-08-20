using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TargetHealth[] targets;
    public GameObject player;
    public Camera worldCamera;

    public float startTimerAmount = 3;
    private float startTimer;

    public float targetActivateTimerAmount = 1;
    private float targetActivateTimer;
    

    public float gametimerAmount = 60;
    private float gameTimer;

    private int score = 0;

    public enum GameState
    {
        Start,
        Playing,
        Gameover
    };

    GameState gameState;
    public GameState State { get { return gameState; } }



    private void Awake()
    {
        gameState = GameState.Gameover;
    }
    private void Start()
    {
        player.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].gameManager = this;
            targets[i].gameObject.SetActive(false);
        }
        startTimer = startTimerAmount;
    }
    private void Update()
    {

        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }

            switch (gameState)
            {
                case GameState.Start:
                    GameStateStart();
                    break;
                case GameState.Playing:
                    GameStatePlaying();
                    break;
                case GameState.Gameover:
                    GameStateGameOver();
                    break;
            }
        }
    }
    private void GameStateStart()
    {
        startTimerAmount -= Time.deltaTime;
        if (startTimerAmount < 0)
        {
            gameState = GameState.Playing;
            gameTimer = gametimerAmount;
            startTimer = startTimerAmount;
            score = 0;

            player.SetActive(true);
            worldCamera.gameObject.SetActive(false);
        }
    }

    private void GameStatePlaying()
    {
        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0)
        {
            Debug.Log("Game Over Score: " + score);
            gameState = GameState.Gameover;
            player.SetActive(false);
            worldCamera.gameObject.SetActive(true);
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].gameObject.SetActive(false);
            }
        }
        //timer before activating target
        targetActivateTimer -= Time.deltaTime;
        if (targetActivateTimer <= 0)
        {
            ActivateRandomTarget();
            targetActivateTimer = targetActivateTimerAmount;
        }
    }
    private void GameStateGameOver()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            gameState = GameState.Start;
        }
    }

    private void ActivateRandomTarget()
    {
        int randomindex = Random.Range(0, targets.Length);
        targets[randomindex].gameObject.SetActive(true);
    }

    public void AddScore(int points)
    {
        score += points;
    }
}


