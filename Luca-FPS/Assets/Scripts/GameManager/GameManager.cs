using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public HighScores highScores;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    public GameObject highScorePanel;
    public TextMeshProUGUI highScoresText;

    public Button newGameButton;
    public Button highScoresButton;

    public GameObject _targetPrefab;
    public GameObject[] _spawnSpots;
    public GameObject player;
    public Camera worldCamera;

    public AudioSource Menu;
    public AudioSource Playing;

    public float startTimerAmount = 3;
    private float startTimer;

    private float gameTimer; // counts up

    private int hitCount = 0;
    private const int maxHits = 10; // ends at 10 hits

    public enum GameState
    {
        Start,
        Playing,
        Gameover
    };

    public GameState gameState;
    public GameState State { get { return gameState; } }

    private GameObject currentTarget;

    private void Awake()
    {
        gameState = GameState.Gameover;
        Menu.Play();
        Playing.Stop();
        _spawnSpots = GameObject.FindGameObjectsWithTag("Spawn");
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        player.SetActive(false);
        worldCamera.gameObject.SetActive(true);

        startTimer = startTimerAmount;
        messageText.text = "Press Enter to Start";
        timerText.text = " ";
        scoreText.text = "Hits: 0 / 10";

        highScorePanel.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(true);
        highScoresButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (gameState == GameState.Gameover || gameState == GameState.Start)
            {
                OnNewGame();
            }
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

    private void GameStateStart()
    {
        highScorePanel.gameObject.SetActive(false);
        highScoresButton.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(false);
        startTimer -= Time.deltaTime;

        messageText.text = "Get Ready " + Mathf.CeilToInt(startTimer).ToString();

        if (startTimer <= 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            messageText.text = " ";
            gameState = GameState.Playing;
            gameTimer = 0f;
            startTimer = startTimerAmount;
            hitCount = 0;

            scoreText.text = "Hits: 0 / 10";

            player.SetActive(true);
            worldCamera.gameObject.SetActive(false);

            Menu.Stop();
            Playing.Play();

            SpawnTarget();
        }
    }

    // spawn a target
    private void SpawnTarget()
    {
        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        if (_spawnSpots.Length == 0)
        {
            Debug.LogWarning("no spawn spots");
            return;
        }

        int randomSpawnIndex = Random.Range(0, _spawnSpots.Length);
        GameObject spawnSpot = _spawnSpots[randomSpawnIndex];

        BoxCollider spawnCollider = spawnSpot.GetComponent<BoxCollider>();
        if (spawnCollider == null)
        {
            Debug.LogWarning($"spawn spot {spawnSpot.name} missing collider");
            return;
        }

        Vector3 spawnPosition = RandomPointInBounds(spawnCollider.bounds);
        currentTarget = Instantiate(_targetPrefab, spawnPosition, Quaternion.identity);

        TargetHealth targetHealth = currentTarget.GetComponent<TargetHealth>();
        if (targetHealth != null)
        {
            targetHealth.gameManager = this;
        }
        else
        {
            Debug.LogWarning("target missing TargetHealth");
        }
    }

    // random point in bounds
    public Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private void GameStatePlaying()
    {
        highScorePanel.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(false);

        gameTimer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(gameTimer / 60F);
        int seconds = Mathf.FloorToInt(gameTimer % 60F);
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    // end the game
    private void EndGame()
    {
        Cursor.lockState = CursorLockMode.Confined;

        int minutes = Mathf.FloorToInt(gameTimer / 60F);
        int seconds = Mathf.FloorToInt(gameTimer % 60F);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        messageText.text = $"game over!\ntime: {formattedTime}\npress enter to play again.";
        gameState = GameState.Gameover;

        player.SetActive(false);
        worldCamera.gameObject.SetActive(true);

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        highScores.AddTime(gameTimer);
        highScores.SaveTimesToFile();

        newGameButton.gameObject.SetActive(false);
        highScoresButton.gameObject.SetActive(true);

        Playing.Stop();
        Menu.Play();
    }

    private void GameStateGameOver()
    {
        // wait for enter
    }

    public void OnNewGame()
    {
        gameState = GameState.Start;
    }

    public void OnHighScores()
    {
        messageText.text = "";

        highScoresButton.gameObject.SetActive(false);
        highScorePanel.gameObject.SetActive(true);
        newGameButton.gameObject.SetActive(true);

        string text = "";
        for (int i = 0; i < highScores.times.Length; i++)
        {
            float time = highScores.times[i];
            if (time > 0f)
            {
                int minutes = Mathf.FloorToInt(time / 60F);
                int seconds = Mathf.FloorToInt(time % 60F);
                text += string.Format("{0:00}:{1:00}", minutes, seconds) + "\n";
            }
            else
            {
                text += "--:--\n";
            }
        }
        highScoresText.text = text;
    }

    // add a hit
    public void AddScore(int points)
    {
        hitCount++;
        scoreText.text = $"Hits: {hitCount} / {maxHits}";

        if (hitCount >= maxHits)
        {
            EndGame();
        }
        else
        {
            SpawnTarget();
        }
    }
}
