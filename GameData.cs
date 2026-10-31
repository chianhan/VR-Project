using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameFlow { Menu, Playing, Pause, GameOver, GameClear }

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public GameFlow gameFlow;           // Current game state
    public bool isInitialize = false;   // Whether initialization is complete

    [Header("Items")]
    public int collectedItemCount = 0;
    public HashSet<string> collectedItems = new HashSet<string>();
    public bool isCollectItem = false; // Whether an item has been collected

    [Header("Timer")]
    public int gameTime;                // Countdown duration
    public float currentGameTime;       // Remaining countdown time
    public bool isTimerRunning = false; // Whether the timer is running
    public bool isTimerPaused = false;  // Whether the timer is paused
    public bool isTimeup = false;       // Whether the countdown has ended

    private GameObject player;

    // Track portals that have already been used
    public HashSet<string> usedDoors = new HashSet<string>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Initialize();
        player = GameObject.FindWithTag("Player");
    }

    public void Initialize()
    {
        SetGameFlow(GameFlow.Menu);
        isInitialize = true;
    }

    private void Update()
    {
        if (isTimerRunning && !isTimerPaused)
        {
            currentGameTime -= Time.deltaTime;
            if (currentGameTime <= 0)
            {
                currentGameTime = 0;
                OnTimerFinish();
            }
        }
    }

    public void OnCollectItem(string itemName)
    {
        collectedItems.Add(itemName);
        collectedItemCount++;
        if (collectedItemCount >= 5)
        {
            SetGameFlow(GameFlow.GameClear);
        }

        #region Game Timer
        public void StartTimer()
        {
            if (!isTimerRunning)
            {
                currentGameTime = gameTime;
                isTimerRunning = true;
                isTimerPaused = false;
                isTimeup = false;
                Debug.Log("Timer started");
            }
        }

        public void PauseTimer()
        {
            if (isTimerRunning && !isTimerPaused)
            {
                isTimerPaused = true;
                Debug.Log("Timer paused");
            }
        }

        public void ContinueTimer()
        {
            if (isTimerRunning && isTimerPaused)
            {
                isTimerPaused = false;
                Debug.Log("Timer resumed");
            }
        }

        public void OnTimerFinish()
        {
            isTimerRunning = false;
            isTimerPaused = false;
            isTimeup = true;
            Debug.Log("Timer finished");
            GameManager.instance?.TimerFinish();
        }

        public string GetTimerText()
        {
            int minutes = Mathf.FloorToInt(currentGameTime / 60F);
            int seconds = Mathf.FloorToInt(currentGameTime % 60F);
            return $"{minutes:00}:{seconds:00}";
        }

        public void MarkDoorAsUsed(string doorName)
        {
            if (!usedDoors.Contains(doorName))
            {
                usedDoors.Add(doorName);
            }
        }

        public bool IsDoorUsed(string doorName)
        {
            return usedDoors.Contains(doorName);
        }

        public void SetGameFlow(GameFlow _gameFlow)
        {
            Debug.Log("SetGameFlow : " + _gameFlow);
            if (!isInitialize)
            {
                gameFlow = _gameFlow;
            }
            else if (gameFlow != _gameFlow)
            {
                gameFlow = _gameFlow;
            }

            switch (gameFlow)
            {
                case GameFlow.Menu:
                    isTimerRunning = false;
                    isTimerPaused = false;
                    break;
                case GameFlow.Playing:
                    if (!isTimerRunning)
                    {
                        StartTimer();
                    }
                    else if (isTimerPaused)
                    {
                        ContinueTimer();
                    }
                    break;
                case GameFlow.Pause:
                    if (isTimerRunning && !isTimerPaused)
                    {
                        PauseTimer();
                        GameManager.instance?.PauseGame(false);
                    }
                    break;
                case GameFlow.GameOver:
                    PauseTimer();
                    isTimerRunning = false;
                    isTimerPaused = false;
                    GameManager.instance?.OnGameOver();
                    break;
                case GameFlow.GameClear:
                    Debug.Log("	Game cleared!");

                    ChangeScene("produce");
                    break;
            }
        }

        public void ChangeScene(string _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
            Debug.Log("	Switching to scene: " + _sceneName);
            player.transform.position = Vector3.zero;
        }
        #endregion
    }
